
using ClassTrack.Domain.Enums;

namespace ClassTrack.Domain.Entities
{
    public class Student:AppUser
    {
        //Relation
        public ICollection<StudentQuiz> StudentQuizes { get; set; }
        public ICollection<StudentTaskWork> StudentTaskWorks { get; set; }
        public ICollection<StudentClassRoom> StudentClasses { get; set; }
    }
}
