

namespace ClassTrack.Domain.Entities
{
    public class StudentQuiz:BaseEntity
    {
        public string StudentId { get; set; }
        public long QuizId { get; set; }

        public Student Student { get; set; }
        public Quiz Quiz { get; set; }


        public decimal TotalPoint { get; set; }
    }
}
