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
    internal class StudentQuizConfiguration : IEntityTypeConfiguration<StudentQuiz>
    {
        public void Configure(EntityTypeBuilder<StudentQuiz> builder)
        {
            builder.HasKey(sq=>new { sq.QuizId,sq.StudentId});

            builder.Property(sq=>sq.TotalPoint)
                .IsRequired()
                .HasColumnType("DECIMAL(5,2)");

        }
    }
}
