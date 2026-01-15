

namespace ClassTrack.Domain.Entities
{
    public class Option:BaseAccountableEntity
    {
        public string Variant { get; set; }
        public bool IsCorrect { get; set; }

        //Relations
        public long QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
