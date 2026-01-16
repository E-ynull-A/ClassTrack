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
    internal class TeacherClassConfiguration : IEntityTypeConfiguration<TeacherClass>
    {
        public void Configure(EntityTypeBuilder<TeacherClass> builder)
        {
            builder.HasKey(tc => new { tc.TeacherId, tc.ClassId });
        }
    }
}
