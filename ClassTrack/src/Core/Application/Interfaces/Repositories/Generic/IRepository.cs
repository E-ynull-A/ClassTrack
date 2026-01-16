using ClassTrack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace ClassTrack.Application.Interfaces.Repositories
{
    public interface IRepository<T> where T:BaseEntity
    {
        IEnumerable<T> GetAllAsync(
            Expression<Func<T, bool>>? function,
            Expression<Func<T, object>>? sort,
            int page = 0,
            int take = 0,
            bool isIgnore = false,
            params string[] includes
            );
        T GetById(
            long id,
            bool isIgnore = false,
            params string[] includes);
        void Add(long id);
        void Update(T entity);
        void Delete(long id);
        System.Threading.Tasks.Task SaveChangeAsync();
    }
}
