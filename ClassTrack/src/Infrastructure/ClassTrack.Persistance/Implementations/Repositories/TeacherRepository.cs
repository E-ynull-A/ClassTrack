using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Domain.Entities;
using ClassTrack.Persistance.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Persistance.Implementations.Repositories
{
    internal class TeacherRepository:Repository<Teacher>, ITeacherRepository
    {
        private readonly AppDbContext _context;

        public TeacherRepository(AppDbContext context):base(context)
        {
            _context = context;
        }


        public async Task<Teacher?> GetTeacherByUserIdAsync(string userId,params string[] includes)
        {
            IQueryable<Teacher> query = _context.Teachers.AsQueryable();

            query = _addIncludes(query, includes);
               return await query.FirstOrDefaultAsync(t => t.AppUserId == userId);
        }      
    }
}
