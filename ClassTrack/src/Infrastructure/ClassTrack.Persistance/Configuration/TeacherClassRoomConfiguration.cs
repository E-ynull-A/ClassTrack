using ClassTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Persistance.Configuration
{
    internal class TeacherClassRoomConfiguration : IEntityTypeConfiguration<TeacherClassRoom>
    {
        public void Configure(EntityTypeBuilder<TeacherClassRoom> builder)
        {
            builder.HasKey(tc => new { tc.TeacherId, tc.ClassRoomId });
        }
    }
}
