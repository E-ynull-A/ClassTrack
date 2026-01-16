using Microsoft.EntityFrameworkCore;
using ClassTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ClassTrack.Persistance.Configuration
{
    internal class TaskConfiguration : IEntityTypeConfiguration<Domain.Entities.Task>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Task> builder)
        {
            builder.Property(t => t.Title)
                .IsRequired()
                .HasColumnType("NVARCHAR(300)");

            builder.Property(t => t.MainPart)
                .IsRequired()
                .HasColumnType("NVARCHAR(MAX)");

            builder.Property(t => t.StartDate)
                .IsRequired()
                .HasColumnType("DATETIME2");

            builder.Property(t => t.EndDate)
               .IsRequired()
               .HasColumnType("DATETIME2");

        }
    }
}
