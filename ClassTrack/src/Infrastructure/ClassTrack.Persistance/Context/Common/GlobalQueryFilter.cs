using ClassTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Persistance.Context
{
    internal static class GlobalQueryFilter
    {

        public static void ApplyAllQueryFilters(this ModelBuilder builder)
        {
            builder._applyQueryFilters<ClassRoom>();
            builder._applyQueryFilters<Question>();          
            builder._applyQueryFilters<Option>();
            builder._applyQueryFilters<Quiz>();
            builder._applyQueryFilters<QuizAnswer>();
            builder._applyQueryFilters<StudentAttendance>();
            builder._applyQueryFilters<TaskWork>();
            builder._applyQueryFilters<StudentQuiz>();
        }

        private static void _applyQueryFilters<T>( this ModelBuilder builder)
            where T : BaseEntity
        {
            builder.Entity<T>().HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
