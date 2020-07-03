using System;
using SQLite;
using WhiteMvvm.Services.Locator;
using WhiteMvvm.Services.Logging;

namespace WhiteMvvm.Bases
{
    public class BaseTransitional
    {
        [PrimaryKey, AutoIncrement]
        public int? BaseId { get; set; }
        public virtual string Id { get; set; }
        /// <summary>
        /// convert transitional object to model object
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TTransitional"></typeparam>
        /// <returns></returns>
        public TModel ToModel<TTransitional, TModel>(Func<TTransitional?, TModel> mapping) where TModel : BaseModel, new() where TTransitional : BaseTransitional, new()
        {
            var model = mapping.Invoke(this as TTransitional);
            return model;
        }
    }
}
