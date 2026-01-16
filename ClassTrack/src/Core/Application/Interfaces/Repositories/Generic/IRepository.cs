using D = ClassTrack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace ClassTrack.Application.Interfaces.Repositories
{
    public interface IRepository<T> where T:D.BaseEntity
    {
        IQueryable<T> GetAllAsync(
            Expression<Func<T, bool>>? function,
            Expression<Func<T, object>>? sort,
            int page = 0,
            int take = 0,
            bool isIgnore = false,
            params string[] includes
            );
        Task<T> GetByIdAsync(
            long id,
            bool isIgnore = false,
            params string[] includes);
        void Add(T entity);
        void Update(T entity);
        void Delete(T removed);
        Task SaveChangeAsync();
    }
}
