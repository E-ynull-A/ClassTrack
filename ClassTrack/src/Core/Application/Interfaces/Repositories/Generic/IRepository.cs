using D = ClassTrack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace ClassTrack.Application.Interfaces.Repositories
{
    public interface IRepository<T> where T:D.BaseEntity
    {
        IQueryable<T> GetAll(
            Expression<Func<T, bool>>? function = null,
            Expression<Func<T, object>>? sort = null,
            int page = 0,
            int take = 0,
            bool isIgnore = false,
            params string[]? includes
            );
        Task<T> GetByIdAsync(
            long id,
            bool isIgnore = false,
            params string[] includes);
        void Add(T entity);
        void AddRange(ICollection<T> entities);
        void Update(T entity);
        void Delete(T removed);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        Task SaveChangeAsync();
    }
}
