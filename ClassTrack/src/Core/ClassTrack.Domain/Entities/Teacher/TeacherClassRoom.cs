


namespace ClassTrack.Domain.Entities
{
    public class TeacherClassRoom
    {
        public long TeacherId { get; set; }
        public long ClassRoomId { get; set; }

        public Teacher Teacher { get; set; }
        public ClassRoom ClassRoom { get; set; }


    }
}
