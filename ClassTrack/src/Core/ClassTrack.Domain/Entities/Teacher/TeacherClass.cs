


namespace ClassTrack.Domain.Entities
{
    public class TeacherClass
    {
        public string TeacherId { get; set; }
        public long ClassId { get; set; }

        public Teacher Teacher { get; set; }
        public Class Class { get; set; }
    }
}
