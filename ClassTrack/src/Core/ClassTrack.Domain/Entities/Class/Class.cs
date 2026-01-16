

namespace ClassTrack.Domain.Entities
{
    public class Class:BaseNamebleEntity
    {
        public decimal AvgPoint { get; set; }
        public string PrivateKey { get; set; }


        //Relations

        public ICollection<StudentClass> StudentClasses { get; set; }
        public ICollection<TeacherClass> TeacherClasses { get; set; }
        public ICollection<TaskWork> TaskWorks { get; set; }
        public ICollection<Quiz> Quizes { get; set; }
    }
}
