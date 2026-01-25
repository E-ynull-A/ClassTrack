using ClassTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Persistance.Configuration
{
    internal class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {

            builder.Property(p => p.Name)
                .HasColumnType("NVARCHAR(60)")
                .IsRequired();

            builder.Property(p => p.Surname)
                .HasColumnType("NVARCHAR(60)")         
                .IsRequired();

            builder.Property(p => p.Age)
                .HasColumnType("TINYINT")
                .IsRequired();

            builder.Property(p => p.BirthDate)
                .HasColumnType("DATE")
                .IsRequired();
                
            builder.Property(p => p.PhoneNumber)
                .HasColumnType("NVARCHAR(25)");

            builder.Property(p => p.UserName)
                .HasColumnType("NVARCHAR(80)")
                .IsRequired();

            builder
                .HasOne(p => p.Student)
                .WithOne(s => s.AppUser)
                .HasForeignKey<Student>(s => s.AppUserId);

            builder
                .HasOne(p => p.Teacher)
                .WithOne(t => t.AppUser)
                .HasForeignKey<Teacher>(t => t.AppUserId);
        }
    }
}
