
namespace ClassTrack.Domain.Entities
{
    public abstract class BaseAccountableEntity:BaseEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; } = "Admin";
    }
}
