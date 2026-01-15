

namespace ClassTrack.Domain.Entities
{
    public class Teacher:AppUser
    {


        //Relations
        public ICollection<TeacherClass> TeacherClasses { get; set; }
    }
}
