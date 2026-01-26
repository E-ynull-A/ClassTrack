
using ClassTrack.Domain.Enums;

namespace ClassTrack.Domain.Entities
{
    public class Student:BaseEntity
    {
        //Relation
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }


        public ICollection<StudentAttendance> StudentAttendances { get; set; }
        public ICollection<StudentQuiz> StudentQuizes { get; set; }
        public ICollection<StudentTaskWork> StudentTaskWorks { get; set; }
        public ICollection<StudentClassRoom> StudentClasses { get; set; }
    }
}
