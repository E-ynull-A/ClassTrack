

namespace ClassTrack.Domain.Entities
{
    public class StudentTaskWork
    {
        public long StudentId { get; set; }
        public long TaskId { get; set; }

        public Student Student { get; set; }
        public TaskWork TaskWork { get; set; }


        public bool IsEvaluated { get; set; }
        public decimal? Point { get; set; }
        public string? StudentAnswer { get; set; }
    }
}
