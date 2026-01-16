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
    internal class OptionConfiguration : IEntityTypeConfiguration<Option>
    {
        public void Configure(EntityTypeBuilder<Option> builder)
        {
            builder.Property(o => o.Variant)
                .IsRequired()
                .HasColumnType("NVARCHAR(500)");

            builder.Property(o => o.IsCorrect)
                .HasDefaultValue(false);
        }
    }
}
