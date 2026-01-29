using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Domain;
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
    internal class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        private readonly AppDbContext _context;

        public QuestionRepository(AppDbContext context) : base(context) { }


    }


}
