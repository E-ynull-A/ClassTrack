



namespace ClassTrack.Domain.Entities
{
    public class StudentAttendance
    {
        public long Id { get; set; }
        public long StudentId { get; set; }
        public bool IsAbsent { get; set; }
        public DateOnly LessonDate { get; set; }
    }
}
