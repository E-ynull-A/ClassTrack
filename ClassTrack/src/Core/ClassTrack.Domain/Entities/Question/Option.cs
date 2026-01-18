

namespace ClassTrack.Domain.Entities
{
    public class Option:BaseAccountableEntity
    {
        public string Variant { get; set; }
        public bool IsCorrect { get; set; }

        //Relations
        public long ChoiceQuestionId { get; set; }
        public ChoiceQuestion ChoiceQuestion { get; set; }
    }
}
