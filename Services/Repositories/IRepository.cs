using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Models;

namespace Services.Repositories {
    internal interface IRepository<T> where T : Entity {
        Task<T> Get(Guid id);
        Task<IEnumerable<T>> GetAll();
        IAsyncEnumerable<T> GetAllAsync();
        Task<T> Add(T entity);
        Task<T> Update(T entity);

        Task<T> Upsert(T entity);

        Task<bool> Delete(Guid id);
        Task<bool> Delete(T entity);
    }
}