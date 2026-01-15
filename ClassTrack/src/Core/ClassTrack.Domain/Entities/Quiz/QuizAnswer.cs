namespace ClassTrack.Domain.Entities
{
    public class QuizAnswer
    {
        public long Id { get; set; }

        //Relations
        public long StudentQuizId { get; set; }
        public long? AnswerId { get; set; }
        public long QuestionId { get; set; }
        public string? AnswerText { get; set; }
        public decimal EarnedPoint { get; set; }

    }
}
