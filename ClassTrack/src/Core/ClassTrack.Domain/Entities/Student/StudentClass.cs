

namespace ClassTrack.Domain.Entities
{
    public class StudentClass
    {
        public long StudentId { get; set; }
        public long ClassId { get; set; }

        public Student Student { get; set; }
        public Class Class { get; set; }

        public decimal AvgPoint { get; set; }
    }
}
