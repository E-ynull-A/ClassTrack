

namespace ClassTrack.Domain.Entities
{
    public class Teacher:BaseEntity
    {

        //Relations

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public ICollection<TeacherClassRoom> TeacherClassRooms { get; set; }
    }
}
