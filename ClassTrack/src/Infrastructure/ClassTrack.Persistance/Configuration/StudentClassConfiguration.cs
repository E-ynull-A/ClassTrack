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
    internal class StudentClassConfiguration : IEntityTypeConfiguration<StudentClass>
    {
        public void Configure(EntityTypeBuilder<StudentClass> builder)
        {
            builder.Property(sc => sc.AvgPoint)
                .IsRequired()
                .HasColumnType("DECIMAL(5,2)");

            builder.HasKey(sc => new { sc.StudentId, sc.ClassId });
        }
    }
}
