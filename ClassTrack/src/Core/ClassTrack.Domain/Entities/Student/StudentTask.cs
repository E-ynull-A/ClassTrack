

namespace ClassTrack.Domain.Entities
{
    public class StudentTask
    {
        public long StudentId { get; set; }
        public long TaskId { get; set; }

        public Student Student { get; set; }
        public Task Task { get; set; }


        public decimal Point { get; set; }
        public string? StudentAnswer { get; set; }
    }
}
