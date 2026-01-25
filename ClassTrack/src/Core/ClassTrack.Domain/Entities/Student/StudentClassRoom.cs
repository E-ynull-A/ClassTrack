

namespace ClassTrack.Domain.Entities
{
    public class StudentClassRoom
    {
        public long StudentId { get; set; }
        public long ClassRoomId { get; set; }

        public Student Student { get; set; }
        public ClassRoom ClassRoom { get; set; }

        public decimal AvgPoint { get; set; }
    }
}
