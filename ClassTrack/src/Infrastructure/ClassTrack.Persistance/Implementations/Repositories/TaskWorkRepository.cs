using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Domain.Entities;
using ClassTrack.Persistance.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Persistance.Implementations.Repositories
{
    internal class TaskWorkRepository:Repository<TaskWork>,ITaskWorkRepository
    {
        private readonly AppDbContext _context;

        public TaskWorkRepository(AppDbContext context):base(context)
        {
            _context = context;
        }

        public async Task<StudentTaskWork?> GetStudentTaskWorkAsync(long taskWorkId,long studentId)
        {
          return await _context.StudentTaskWorks
                .FirstOrDefaultAsync(st => st.TaskWorkId == taskWorkId 
                                        && st.StudentId == studentId);
        }

        public async Task<decimal> GetStudentTaskPointAvgAsync(long classRoomId,long studentId)
        {
            return await _context.StudentTaskWorks
                .Where(st=>st.TaskWork.ClassRoomId == classRoomId && st.StudentId == studentId)
                    .AverageAsync(st => st.Point);
        }
    }
}
