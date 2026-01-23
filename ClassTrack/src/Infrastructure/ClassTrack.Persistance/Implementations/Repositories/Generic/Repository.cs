using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Domain.Entities;
using ClassTrack.Persistance.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace ClassTrack.Persistance.Implementations.Repositories
{
    internal class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbset;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbset = context.Set<T>();
        }

        public void Add(T entity)
        {
            _dbset.Add(entity);
        }

        public void AddRange(ICollection<T> entities)
        {
            foreach (T entity in entities)
            {
                Add(entity);
            }
        }


        public void Delete(T removed)
        {
            _dbset.Remove(removed);
        }

        public void DeleteRange(ICollection<T> removeds)
        {
            foreach (T removed in removeds)
            {
                Delete(removed);
            }
        }

        public IQueryable<T> GetAll(Expression<Func<T,
                                    bool>>? function = null,
                                    Expression<Func<T, object>>? sort = null,
                                    int page = 0, int take = 0,
                                    bool isIgnore = false,
                                    params string[]? includes)
        {
            IQueryable<T> query = _dbset;

            if (function is not null)
                query = query.Where(function);


            if (sort is not null)
                query = query.OrderBy(sort);


            if (isIgnore is not false)
                query = query.IgnoreQueryFilters();

            if (includes is not null)
                query = _addIncludes(query, includes);

            if (take > 0 && page > 0)
            {
                query = query
                            .Skip((page - 1) * take)
                            .Take(take);
            }
           
            return query;
        }

     

        public async Task<T> GetByIdAsync(long id, bool isIgnore = false, params string[] includes)
        {
            IQueryable<T> query = _dbset.AsNoTracking();

            if (isIgnore is true)
                query = query.IgnoreQueryFilters();
            if (includes.Length>0)
                query = _addIncludes(query, includes);           
                

            return await query.FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _dbset.Update(entity);
        }       

        protected IQueryable<T> _addIncludes(IQueryable<T> query, params string[] includes)
        {

            foreach(string include in includes)
            {
                query = query.Include(include);                
            }         
           
            return query;
        }

        public async Task<bool> AnyAsync(Expression<Func<T,bool>> expression)
        {
            return await _dbset.AnyAsync(expression);          
        }
    }
}
