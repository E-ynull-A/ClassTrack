



namespace ClassTrack.Domain.Entities
{
    public class StudentAttendance:BaseEntity
    {
        public Student Student { get; set; }
        public long StudentId { get; set; }
        public Attendance Attendance { get; set; }
        public DateOnly LessonDate { get; set; }

        public long ClassRoomId { get; set; }
        public ClassRoom ClassRoom { get; set; }
    }
}
