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
    internal class StudentTaskConfiguration : IEntityTypeConfiguration<StudentTaskWork>
    {
        public void Configure(EntityTypeBuilder<StudentTaskWork> builder)
        {
            builder.HasKey(st => new { st.StudentId, st.TaskWorkId });

            builder.Property(st=>st.Point)                
                .HasColumnType("DECIMAL(5,2)");

            builder.Property(st => st.StudentAnswerText)
                .IsRequired(false)
                .HasColumnType("NVARCHAR(MAX)");

            builder.Property(st => st.StudentAnswerLink)
                .IsRequired(false)
                .HasColumnType("NVARCHAR(MAX)");

            builder.HasQueryFilter(st =>st.Student != null && !st.Student.IsDeleted);
        }
    }
}
