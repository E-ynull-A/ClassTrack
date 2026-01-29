

namespace ClassTrack.Domain.Entities
{
    public class StudentQuiz:BaseEntity
    {
        public long StudentId { get; set; }
        public long QuizId { get; set; }

        public Student Student { get; set; }
        public Quiz Quiz { get; set; }


        public decimal TotalPoint { get; set; }
        public string QuizStatus { get; set; }
    }
}
