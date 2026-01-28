namespace ClassTrack.Domain.Entities
{
    public class QuizAnswer:BaseEntity
    {

        public decimal EarnedPoint { get; set; }

        //Relations
        public long StudentQuizId { get; set; }
        public long? AnswerId { get; set; }
        public long QuestionId { get; set; }
        public string? AnswerText { get; set; }
        public bool IsEvaluated { get; set; }

        //relation 

        public StudentQuiz StudentQuiz { get; set; }
        public Question Question { get; set; }

    }
}
