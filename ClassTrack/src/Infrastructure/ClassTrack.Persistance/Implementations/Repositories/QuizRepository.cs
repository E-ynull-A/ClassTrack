using ClassTrack.Application.Interfaces;
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
    internal class QuizRepository:Repository<Quiz>,IQuizRepository
    {
        public QuizRepository(AppDbContext context) : base(context) { }

       

        public void GetAllowCreateOrUpdateQuestion(Quiz quiz)
        {
            if (quiz.StartTime <= DateTime.UtcNow && quiz.StartTime.Add(quiz.Duration) > DateTime.UtcNow)
                throw new Exception("Couldn't Add or Modify New Question during a Quiz Interval!");

            if(quiz.StartTime.Add(quiz.Duration) < DateTime.UtcNow)
                throw new Exception("Couldn't Add or Modify New Question after the Quiz!");
        }
        
    }
}
