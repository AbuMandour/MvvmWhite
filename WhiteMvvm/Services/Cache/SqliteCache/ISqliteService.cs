using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WhiteMvvm.Bases;

namespace WhiteMvvm.Services.Cache.SqliteCache
{
    public interface ISqliteService
    {
        /// <summary>
        /// method to create tables after check if it exists
        /// </summary>
        /// <returns></returns>
        bool CreateDatabaseTables<T>(IList<T> tables) where T : BaseTransitional;
        /// <summary>
        /// Method to save data in cache return true whenever data are saved
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        T SaveItem<T>(T item) where T : BaseTransitional, new();

        /// <summary>
        /// Method to save list in cache return true whenever list are saved
        /// </summary>
        /// <typeparam name="TList"></typeparam>
        /// <typeparam name="TBaseTransitional"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        bool SaveList<TBaseTransitional>(List<TBaseTransitional> items) where TBaseTransitional : BaseTransitional, new();
        /// <summary>
        /// save item and it's children
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        void SaveItemRecursive<T>(T item) where T : BaseTransitional, new();
        /// <summary>
        /// save items and it's children
        /// </summary>
        /// <typeparam name="TBaseTransitional"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        void SaveListRecursive<TBaseTransitional>(List<TBaseTransitional> items) where TBaseTransitional : BaseTransitional, new();
        /// <summary>
        /// get item and it's children
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        T GetItemRecursive<T>(Expression<Func<T, bool>> query) where T : BaseTransitional, new();
        /// <summary>
        /// get items and it's children
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<T> GetItemsRecursive<T>() where T : BaseTransitional, new();
        /// <summary>
        /// Method to get item from cache return object inherited from base model depend on linq query 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetItem<T>(Expression<Func<T, bool>> query) where T : BaseTransitional, new();
        /// <summary>
        /// Method get all list from object inherited from base model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<T> GetList<T>() where T : BaseTransitional, new();
        /// <summary> 
        /// Method to get some data from list of object depend on linq query 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        List<T> GetList<T>(Expression<Func<T, bool>> query) where T : BaseTransitional, new();
        /// <summary>
        /// Method to delete one object from cache return true whenever object deleted
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        bool DeleteItem<T>(T deletedObject) where T : BaseTransitional, new();
        /// <summary>
        /// Method to delete all data of object from cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        bool DeleteList<T>() where T : BaseTransitional, new();
        /// <summary>
        /// Method to delete all data of object from cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        bool DeleteList<T>(Expression<Func<T, bool>> query) where T : BaseTransitional, new();
        /// <summary>
        /// update item with it children
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        void UpdateItemRecursive<T>(T item) where T : BaseTransitional, new();
    }
}
