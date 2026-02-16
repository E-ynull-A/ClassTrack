

namespace ClassTrack.Domain.Entities
{
    public class TaskWork:BaseAccountableEntity
    {
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string MainPart { get; set; }



        //Relations
        public ICollection<TaskAttachment> TaskAttachments { get; set; }
        public ICollection<StudentTaskWork> StudentTaskWorks { get; set; }
        public long ClassRoomId { get; set; }
        public ClassRoom ClassRoom { get; set; }
    }
}
