using ClassTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Persistance.Context
{
    internal static class AppDbContextHelper
    {
        public static void ApplyAllQueryFilters(this ModelBuilder builder)
        {
            builder._applyQueryFilters<ClassRoom>();
            builder._applyQueryFilters<Student>();
            builder._applyQueryFilters<Teacher>();
            builder._applyQueryFilters<Question>();          
            builder._applyQueryFilters<Option>();
            builder._applyQueryFilters<Quiz>();
            builder._applyQueryFilters<QuizAnswer>();
            builder._applyQueryFilters<StudentAttendance>();
            builder._applyQueryFilters<TaskWork>();
            builder._applyQueryFilters<StudentQuiz>();
            builder._applyQueryFilters<TaskWorkAttachment>();
            builder._applyQueryFilters<RefreshToken>();
        }

        private static void _applyQueryFilters<T>( this ModelBuilder builder)
            where T : BaseEntity
        {
            builder.Entity<T>().HasQueryFilter(x => !x.IsDeleted);
        }

        public static void SaveChangeInterseptor(this ChangeTracker changeTracker)
        {
            var entities = changeTracker.Entries<BaseAccountableEntity>();

            foreach (var entity in entities)
            {
                switch (entity.State)
                {
                    case EntityState.Modified:
                        var isRemovedChange = entity.OriginalValues.GetValue<bool>(nameof(entity.Entity.IsDeleted))
                            != entity.CurrentValues.GetValue<bool>(nameof(entity.Entity.IsDeleted));
                        if (!isRemovedChange)
                        {
                            entity.Entity.UpdatedAt = DateTime.UtcNow;
                        }
                        break;
                    case EntityState.Added:
                        entity.Entity.CreatedBy = "User";
                        entity.Entity.CreatedAt = DateTime.UtcNow;
                        break;
                }
            }           
        }
    }
}
