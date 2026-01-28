using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Domain.Entities;
using ClassTrack.Persistance.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Persistance.Implementations.Repositories
{
    internal class QuizAnswerRepository:Repository<QuizAnswer>,IQuizAnswerRepository
    {
        public QuizAnswerRepository(AppDbContext context) : base(context) { }
       
    }
}
