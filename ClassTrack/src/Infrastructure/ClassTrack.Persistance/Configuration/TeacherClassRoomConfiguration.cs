using ClassTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassTrack.Persistance.Configuration
{
    internal class TeacherClassRoomConfiguration : IEntityTypeConfiguration<TeacherClassRoom>
    {
        public void Configure(EntityTypeBuilder<TeacherClassRoom> builder)
        {
            builder.HasKey(tc => new { tc.TeacherId, tc.ClassRoomId });
            builder.HasQueryFilter(tc => tc.ClassRoom != null && !tc.ClassRoom.IsDeleted);
        }
    }
}
