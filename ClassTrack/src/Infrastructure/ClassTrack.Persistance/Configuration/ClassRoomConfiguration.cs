using ClassTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ClassTrack.Persistance.Configuration
{
    internal class ClassRoomConfiguration : IEntityTypeConfiguration<ClassRoom>
    {
        public void Configure(EntityTypeBuilder<ClassRoom> builder)
        {
            builder.Property(c => c.Name)
                .IsRequired()
                .HasColumnType("NVARCHAR(150)");

            builder.Property(c => c.AvgPoint)
                .IsRequired()
                .HasColumnType("DECIMAL(5,2)");

            builder.Property(c => c.PrivateKey)
                .HasColumnType("CHAR(8)")
                .IsRequired();
        }
    }
}
