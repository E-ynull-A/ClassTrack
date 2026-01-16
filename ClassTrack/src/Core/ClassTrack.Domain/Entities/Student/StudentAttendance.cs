



namespace ClassTrack.Domain.Entities
{
    public class StudentAttendance:BaseEntity
    {      
        public string StudentId { get; set; }
        public Attendance Attendance { get; set; }
        public DateOnly LessonDate { get; set; }
        public bool IsActive { get; set; }
    }
}
