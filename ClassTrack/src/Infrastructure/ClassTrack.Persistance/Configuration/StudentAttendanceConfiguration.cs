using ClassTrack.Domain;
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
    internal class StudentAttendanceConfiguration : IEntityTypeConfiguration<StudentAttendance>
    {
        public void Configure(EntityTypeBuilder<StudentAttendance> builder)
        {
            builder.Property(sa => sa.LessonDate)
                .HasColumnType("DATETIME2")
                .IsRequired();         

            builder.Property(sa => sa.Attendance)
                .HasConversion<int>()
                .HasDefaultValue(Attendance.Absent);           
        }
    }
}
