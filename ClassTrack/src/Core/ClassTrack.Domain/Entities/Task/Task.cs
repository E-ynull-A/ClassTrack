

namespace ClassTrack.Domain.Entities
{
    public class Task:BaseAccountableEntity
    {
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }

        //Relations
        public ICollection<StudentTask> StudentTasks { get; set; }
        public long ClassId { get; set; }
        public Class Class { get; set; }
    }
}
