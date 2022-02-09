using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;
using WhiteMvvm.Bases;
using WhiteMvvm.Exceptions;
using WhiteMvvm.Services.DeviceUtilities;


namespace WhiteMvvm.Services.Cache.SqliteCache
{
    public class SqliteServiceAsync : ISqliteServiceAsync
    {
        private readonly IFileSystem _fileSystem;
        private readonly SQLiteAsyncConnection _sqLiteConnection;
        public SqliteServiceAsync(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            _sqLiteConnection = CreateConnection();
        }
        private SQLiteAsyncConnection CreateConnection()
        {
            return new SQLiteAsyncConnection(_fileSystem.CacheDirectory + "database.db", SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.FullMutex);
        }
        public async Task<bool> DeleteItemAsync<T>(T deletedObject) where T : BaseTransitional, new()
        {
            var result = await _sqLiteConnection.DeleteAsync(deletedObject);
            return result > 0;
        }
        public async Task<int> DeleteListAsync<T>() where T : BaseTransitional, new()
        {
            var tableName = typeof(T).Name;
            var queryResult = await _sqLiteConnection.ExecuteAsync($"DELETE FROM {tableName}");
            return queryResult;
        }
        public async Task<int> DeleteListAsync<T>(Expression<Func<T, bool>> query) where T : BaseTransitional, new()
        {
            var list = await GetListAsync<T>(query);
            var deletedItemsCount = 0;
            foreach (var item in list)
            {
                var result = await _sqLiteConnection.DeleteAsync(item);
                if (result > 0)
                    deletedItemsCount ++;
            }
            return deletedItemsCount;
        }
        public async Task<T> GetItemRecursiveAsync<T>(Expression<Func<T, bool>> query) where T : BaseTransitional, new()
        {
            var list = await _sqLiteConnection.GetAllWithChildrenAsync(query, true);
            return list?.FirstOrDefault();
        }
        public Task<List<T>> GetItemsRecursiveAsync<T>() where T : BaseTransitional, new()
        {
            return _sqLiteConnection.GetAllWithChildrenAsync<T>(recursive: true);            
        }
        public Task<List<T>> GetItemsRecursiveAsync<T>(Expression<Func<T, bool>> query) where T : BaseTransitional, new()
        {
            return _sqLiteConnection.GetAllWithChildrenAsync(query, true);
        }
        public Task<T> GetItemAsync<T>(Expression<Func<T, bool>> query) where T : BaseTransitional, new()
        {
            return _sqLiteConnection.Table<T>().Where(query).FirstOrDefaultAsync();            
        }
        public Task<List<T>> GetListAsync<T>() where T : BaseTransitional, new()
        {
            return _sqLiteConnection.Table<T>().ToListAsync();            
        }
        public Task<List<T>> GetListAsync<T>(Expression<Func<T, bool>> query) where T : BaseTransitional, new()
        {
            return _sqLiteConnection.Table<T>().Where(query).ToListAsync();            
        }
        public async Task<T> SaveItemByIdAsync<T>(T item) where T : BaseTransitional, new()
        {
            if (item != null)
            {
                int result;
                var existItem = await GetItemAsync<T>(x => x.Id == item.Id);
                if (existItem != null)
                {
                    item.BaseId = existItem.BaseId;
                    result = await _sqLiteConnection.UpdateAsync(item);
                }
                else
                {
                    result = await _sqLiteConnection.InsertAsync(item);
                }
                return result > 0 ? item : null;
            }
            return null;
        }
        public async Task<T> SaveItemByBaseIdAsync<T>(T item) where T : BaseTransitional, new()
        {
            if (item != null)
            {
                int result;
                var existItem = await GetItemAsync<T>(x => x.BaseId == item.BaseId);
                if (existItem != null)
                {
                    result = await _sqLiteConnection.UpdateAsync(item);
                }
                else
                {
                    result = await _sqLiteConnection.InsertAsync(item);
                }
                return result > 0 ? item : null;
            }
            return null;
        }
        public async Task<bool> SaveListWithIdAsync<TBaseTransitional>(IEnumerable<TBaseTransitional> items) where TBaseTransitional : BaseTransitional, new()
        {
            if (items == null)
                return false;
            var baseList = items.ToList();
            if (!baseList.Any())
                return false;
            var result = false;
            foreach (var item in baseList)
            {
                var savedItem = await SaveItemByIdAsync(item);
                result = savedItem != null;
            }
            return result;
        }
        public async Task<bool> SaveListWithBaseIdAsync<TBaseTransitional>(IEnumerable<TBaseTransitional> items) where TBaseTransitional : BaseTransitional, new()
        {
            if (items == null)
                return false;
            var baseList = items.ToList();
            if (!baseList.Any())
                return false;
            var result = false;
            foreach (var item in baseList)
            {
                var savedItem = await SaveItemByBaseIdAsync(item);
                result = savedItem != null;
            }
            return result;
        }
        public async Task SaveItemRecursiveAsync<T>(T item) where T : BaseTransitional, new()
        {
            var existItem = await GetItemAsync<T>(x => x.Id == item.Id);
            if (existItem != null)
            {
                item.BaseId = existItem.BaseId;
                await _sqLiteConnection.UpdateWithChildrenAsync(item);
            }
            else
            {
                await _sqLiteConnection.InsertWithChildrenAsync(item, true);
            }
        }
        public  Task SaveListRecursiveAsync<TBaseTransitional>(IEnumerable<TBaseTransitional> items) where TBaseTransitional : BaseTransitional, new()
        {
            return _sqLiteConnection.InsertAllWithChildrenAsync(items, true);
        }
        public async Task UpdateItemRecursiveAsync<T>(T item) where T : BaseTransitional, new()
        {
            var existItem = await GetItemAsync<T>(x => x.Id == item.Id);
            if (existItem != null)
            {
                item.BaseId = existItem.BaseId;
                await _sqLiteConnection.UpdateWithChildrenAsync(item);
            }
        }
        public async Task<bool> CreateDatabaseTablesAsync<T>(IList<T> tables) where T : BaseTransitional
        {
            var tablesArray = tables.Select(x => x.GetType()).ToArray();
            var result = await _sqLiteConnection.CreateTablesAsync(CreateFlags.None, tablesArray);
            var isCreated = result.Results.Any(x => x.Value == 0);
            return isCreated;
        }
    }
}
