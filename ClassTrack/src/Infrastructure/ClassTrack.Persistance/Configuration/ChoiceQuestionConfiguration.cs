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
    internal class ChoiceQuestionConfiguration : IEntityTypeConfiguration<ChoiceQuestion>
    {
        public void Configure(EntityTypeBuilder<ChoiceQuestion> builder)
        {
            builder.Property(cq => cq.IsMultiple)
                 .HasDefaultValue(false);

            builder.HasIndex(cq => cq.Title)
                .IsUnique();
                
        }
    }
}
