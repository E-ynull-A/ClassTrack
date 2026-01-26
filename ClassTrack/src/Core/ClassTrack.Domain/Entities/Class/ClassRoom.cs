

namespace ClassTrack.Domain.Entities
{
    public class ClassRoom:BaseNamebleEntity
    {
        public decimal AvgPoint { get; set; }
        public string PrivateKey { get; set; }


        //Relations
        public ICollection<StudentAttendance>? StudentAttendance { get; set; }

        public ICollection<StudentClassRoom> StudentClasses { get; set; }
        public ICollection<TeacherClassRoom> TeacherClasses { get; set; }
        public ICollection<TaskWork> TaskWorks { get; set; }
        public ICollection<Quiz> Quizes { get; set; }
    }
}
