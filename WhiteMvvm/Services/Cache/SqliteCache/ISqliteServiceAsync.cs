using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WhiteMvvm.Bases;

namespace WhiteMvvm.Services.Cache.SqliteCache
{
    public interface ISqliteServiceAsync
    {
        /// <summary>
        /// method to create tables after check if it exists
        /// </summary>
        /// <returns></returns>
        Task<bool> CreateDatabaseTablesAsync<T>(IList<T> tables) where T : BaseTransitional;
        /// <summary>
        /// Method to save data with id in cache return true whenever data are saved
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<T> SaveItemByIdAsync<T>(T item) where T : BaseTransitional, new();
        /// <summary>
        /// Method to save data with base id in cache return true whenever data are saved
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<T> SaveItemByBaseIdAsync<T>(T item) where T : BaseTransitional, new();
        /// <summary>
        /// Method to save list in cache with Id return true whenever list are saved
        /// </summary>
        /// <typeparam name="TList"></typeparam>
        /// <typeparam name="TBaseTransitional"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        Task<bool> SaveListWithIdAsync<TBaseTransitional>(IEnumerable<TBaseTransitional> items) where TBaseTransitional : BaseTransitional, new();
        /// <summary>
        /// Method to save list in cache with base Id return true whenever list are saved
        /// </summary>
        /// <typeparam name="TList"></typeparam>
        /// <typeparam name="TBaseTransitional"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        Task<bool> SaveListWithBaseIdAsync<TBaseTransitional>(IEnumerable<TBaseTransitional> items)
            where TBaseTransitional : BaseTransitional, new();
        /// <summary>
        /// save item and it's children
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        Task SaveItemRecursiveAsync<T>(T item) where T : BaseTransitional, new();
        /// <summary>
        /// save items and it's children
        /// </summary>
        /// <typeparam name="TBaseTransitional"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        Task SaveListRecursiveAsync<TBaseTransitional>(IEnumerable<TBaseTransitional> items) where TBaseTransitional : BaseTransitional, new();
        /// <summary>
        /// get item and it's children
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<T> GetItemRecursiveAsync<T>(Expression<Func<T, bool>> query) where T : BaseTransitional, new();
        /// <summary>
        /// get items and it's children
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<List<T>> GetItemsRecursiveAsync<T>(Expression<Func<T, bool>> query) where T : BaseTransitional, new();
        /// <summary>
        /// get items and it's children
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<List<T>> GetItemsRecursiveAsync<T>() where T : BaseTransitional, new();
        /// <summary>
        /// Method to get item from cache return object inherited from base model depend on linq query 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> GetItemAsync<T>(Expression<Func<T, bool>> query) where T : BaseTransitional, new();
        /// <summary>
        /// Method get all list from object inherited from base model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<List<T>> GetListAsync<T>() where T : BaseTransitional, new();
        /// <summary> 
        /// Method to get some data from list of object depend on linq query 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<T>> GetListAsync<T>(Expression<Func<T, bool>> query) where T : BaseTransitional, new();
        /// <summary>
        /// Method to delete one object from cache return true whenever object deleted
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<bool> DeleteItemAsync<T>(T deletedObject) where T : BaseTransitional, new();
        /// <summary>
        /// Method to delete all data of object from cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<int> DeleteListAsync<T>() where T : BaseTransitional, new();
        /// <summary>
        /// Method to delete all data of object from cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<int> DeleteListAsync<T>(Expression<Func<T, bool>> query) where T : BaseTransitional, new();
        /// <summary>
        /// update item with it children
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        Task UpdateItemRecursiveAsync<T>(T item) where T : BaseTransitional, new();
    }
}
