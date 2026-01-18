using ClassTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ClassTrack.Persistance.Configuration
{
    internal class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {

            builder.HasDiscriminator<string>("QuestionType")
                .HasValue("ChoiceQuestion")
                .HasValue("OpenQuestion");       

            builder.Property(q => q.Point)
                .IsRequired()
                .HasColumnType("DECIMAL(5,2)");

        }
    }
}
