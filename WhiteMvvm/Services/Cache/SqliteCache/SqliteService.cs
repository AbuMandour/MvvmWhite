using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AppCenter.Crashes;
using SQLite;
using SQLiteNetExtensions.Extensions;
using WhiteMvvm.Bases;
using WhiteMvvm.Exceptions;
using WhiteMvvm.Services.DeviceUtilities;

namespace WhiteMvvm.Services.Cache.SqliteCache
{
    public class SqliteService : ISqliteService
    {
        private readonly IFileSystem _fileSystem;
        private readonly SQLiteConnection _sqLiteConnection;
        public SqliteService(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            _sqLiteConnection = CreateConnection();
        }
        private SQLiteConnection CreateConnection()
        {
            return new SQLiteConnection(_fileSystem.CacheDirectory + "database2.db");
        }
        public bool DeleteItem<T>(T deletedObject) where T : BaseTransitional, new()
        {
            var result = _sqLiteConnection.Delete(deletedObject);
            return result > 0;
        }
        public bool DeleteList<T>() where T : BaseTransitional, new()
        {
            var tableName = typeof(T).Name;
            var queryResult = _sqLiteConnection.Execute($"DELETE FROM {tableName}");
            return queryResult > 0;
        }
        public bool DeleteList<T>(Expression<Func<T, bool>> query) where T : BaseTransitional, new()
        {
            var list = GetList<T>(query);
            var result = 0;
            foreach (var item in list)
            {
                result = _sqLiteConnection.Delete(item);
            }
            return result > 0;
        }
        public List<T> GetItemsRecursive<T>() where T : BaseTransitional, new()
        {
            var list = _sqLiteConnection.GetAllWithChildren<T>(recursive: true);
            return list;
        }
        public T GetItemRecursive<T>(Expression<Func<T, bool>> query) where T : BaseTransitional, new()
        {
            var list = _sqLiteConnection.GetAllWithChildren(query, true);
            return list?.FirstOrDefault();
        }
        public T GetItem<T>(Expression<Func<T, bool>> query) where T : BaseTransitional, new()
        {
            var item = _sqLiteConnection.Table<T>().Where(query).FirstOrDefault();
            return item;
        }
        public List<T> GetList<T>() where T : BaseTransitional, new()
        {
            return _sqLiteConnection.Table<T>().ToList();
        }
        public List<T> GetList<T>(Expression<Func<T, bool>> query) where T : BaseTransitional, new()
        {
            return _sqLiteConnection.Table<T>()
                .Where(query)
                .ToList();
        }
        public T SaveItem<T>(T item) where T : BaseTransitional, new()
        {
            if (item != null)
            {
                int result = 0;
                var existItem = GetItem<T>(x => x.Id == item.Id);
                if (existItem != null)
                {
                    item.BaseId = existItem.BaseId;
                    result = _sqLiteConnection.Update(item);
                }
                else
                {
                    result = _sqLiteConnection.Insert(item);
                }
                return result > 0 ? item : null;
            }
            return null;
        }
        public bool SaveList<TBaseTransitional>(List<TBaseTransitional> items) where TBaseTransitional : BaseTransitional, new()
        {
            if (items is null)
            {
                return false;
            }
            var result = _sqLiteConnection.InsertAll(items);
            return result > 0;
        }
        public void SaveItemRecursive<T>(T item) where T : BaseTransitional, new()
        {
            var existItem = GetItem<T>(x => x.Id == item.Id);
            if (existItem != null)
            {
                item.BaseId = existItem.BaseId;
                _sqLiteConnection.UpdateWithChildren(item);
            }
            else
            {
                _sqLiteConnection.InsertWithChildren(item, true);
            }
        }
        public void SaveListRecursive<TBaseTransitional>(List<TBaseTransitional> items) where TBaseTransitional : BaseTransitional, new()
        {
            _sqLiteConnection.InsertAllWithChildren(items, true);
        }
        public void UpdateItemRecursive<T>(T item) where T : BaseTransitional, new()
        {
            var existItem = GetItem<T>(x => x.Id == item.Id);
            if (existItem != null)
            {
                item.BaseId = existItem.BaseId;
                _sqLiteConnection.UpdateWithChildren(item);
            }
        }
        public bool CreateDatabaseTables<T>(IList<T> tables) where T : BaseTransitional
        {
            var tablesArray = tables.Select(x => x.GetType()).ToArray();
            var result = _sqLiteConnection.CreateTables(CreateFlags.None, tablesArray);
            var isCreated = result.Results.Any(x => x.Value == 0);
            return isCreated;
        }
    }
}
