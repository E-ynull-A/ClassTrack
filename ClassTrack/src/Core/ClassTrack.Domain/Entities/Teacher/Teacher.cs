

namespace ClassTrack.Domain.Entities
{
    public class Teacher:AppUser
    {
        //Relations
        public ICollection<TeacherClassRoom> TeacherClassRooms { get; set; }
    }
}
