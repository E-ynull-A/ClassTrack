

namespace ClassTrack.Domain.Entities
{
    public class TaskWork:BaseAccountableEntity
    {
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string MainPart { get; set; }

        //Relations
        public ICollection<StudentTaskWork> StudentTaskWorks { get; set; }
        public long ClassId { get; set; }
        public Class Class { get; set; }
    }
}
