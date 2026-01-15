
using ClassTrack.Domain.Enums;

namespace ClassTrack.Domain.Entities
{
    public class Student:AppUser
    {

        //Relation
        public ICollection<StudentQuiz> StudentQuizes { get; set; }
        public ICollection<StudentTask> StudentTasks { get; set; }
        public ICollection<StudentClass> StudentClasses { get; set; }
    }
}
