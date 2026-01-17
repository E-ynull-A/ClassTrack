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
    internal class TaskWorkRepository:Repository<TaskWork>,ITaskWorkRepository
    {
        public TaskWorkRepository(AppDbContext context):base(context)
        {
            
        }
    }
}
