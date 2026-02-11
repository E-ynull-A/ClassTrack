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
    internal class StudentQuizRepository:Repository<StudentQuiz>,IStudentQuizRepository
    {
        private readonly AppDbContext _context;

        public StudentQuizRepository(AppDbContext context):base(context)
        {
            _context = context;
        }

    }
}
