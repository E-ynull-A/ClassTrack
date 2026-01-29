using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Persistance.Implementations.Services
{
    internal class StudentQuizService:IStudentQuizService
    {
        private readonly IStudentQuizRepository _studentQuizRepository;

        public StudentQuizService(IStudentQuizRepository studentQuizRepository)
        {
            _studentQuizRepository = studentQuizRepository;
        }
    }
}
