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
    internal class QuizConfiguration : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {


            builder.Property(q => q.Name)
                .IsRequired()
                .HasColumnType("NVARCHAR(90)");

            builder.Property(q => q.Duration)
                 .HasColumnType("TIME")
                 .IsRequired();

            builder.Property(q => q.StartTime)
                .HasColumnType("DATETIME2")
                .IsRequired();
        }
    }
}
