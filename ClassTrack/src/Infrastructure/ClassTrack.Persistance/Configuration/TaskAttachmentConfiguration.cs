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
    internal class TaskAttachmentConfiguration : IEntityTypeConfiguration<TaskWorkAttachment>
    {
        public void Configure(EntityTypeBuilder<TaskWorkAttachment> builder)
        {
            builder.Property(ta => ta.FileName)
                .IsRequired();
            
            builder.Property(ta => ta.FileUrl)
                .IsRequired()
                .HasMaxLength(2048);

            builder.HasIndex(ta => ta.PublicId)
                .IsUnique();
        }
    }
}
