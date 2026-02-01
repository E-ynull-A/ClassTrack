using ClassTrack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.Interfaces.Repositories
{
    public interface ITeacherRepository:IRepository<Teacher>
    {
        Task<Teacher> GetTeacherByUserId(string userId);
    }
}
