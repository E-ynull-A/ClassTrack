using ClassTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ClassTrack.Persistance.Configuration
{
    internal class OpenQuestionConfiguration : IEntityTypeConfiguration<OpenQuestion>
    {
        public void Configure(EntityTypeBuilder<OpenQuestion> builder)
        {
                      
        }
    }
}
