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
    internal class StudentTaskConfiguration : IEntityTypeConfiguration<StudentTask>
    {
        public void Configure(EntityTypeBuilder<StudentTask> builder)
        {
            builder.HasKey(st => new { st.StudentId, st.TaskId });

            builder.Property(st=>st.Point)                
                .HasColumnType("DECIMAL(5,2)");

            builder.Property(st => st.StudentAnswer)
                .IsRequired(false)
                .HasColumnType("NVARCHAR(MAX)");

        }
    }
}
