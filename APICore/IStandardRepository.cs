// Created By abdulrahman elheyb
// 2021-12-01 5:15 PM

using System.Collections.Generic;

namespace APICore
{
    /// <summary>
    /// Standard Repository is contains standard CRUD Operations
    /// (GetAll, Get, Add, Update, Delete)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IStandardRepository<T>
    {
        /// <summary>
        /// Gets the all records from repository
        /// </summary>
        /// <returns></returns>
        public List<T> GetAll();
        
        /// <summary>
        /// Gets the record properties by identity number
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Get(int id);
        
        /// <summary>
        /// Adds the record to repository
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public T Add(T model);
        
        /// <summary>
        /// Updates the record from repository
        /// </summary>
        /// <param name="model">Record object of model</param>
        /// <returns></returns>
        public T Update(T model);
        
        /// <summary>
        /// Deletes the record from repository
        /// </summary>
        /// <param name="id">Record identity</param>
        /// <returns></returns>
        public bool Delete(int id);
    }
}