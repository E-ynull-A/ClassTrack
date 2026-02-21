using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Domain.Entities;
using ClassTrack.Domain.Utilities;
using ClassTrack.Persistance.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Persistance.Implementations.Repositories
{
    internal class ClassRoomRepository:Repository<ClassRoom>,IClassRoomRepository
    {
        private readonly AppDbContext _context;

        public ClassRoomRepository(AppDbContext context):base(context)
        {
            _context = context;
        }

        public async Task BreakStudentClassRoomAsync(long classRoomId, string userId)
        {
            StudentClassRoom? studentClass = await _context.StudentClasses.FirstOrDefaultAsync(sc => sc.ClassRoomId == classRoomId
                                                         && sc.Student.AppUserId == userId);

            if (studentClass == null)
            {
                throw new NotFoundException("The Class Room not Found");
            }

            _context.StudentClasses.Remove(studentClass);
                
        }
    }
}
