

namespace ClassTrack.Domain.Entities
{
    public class ChoiceQuestion:Question
    {             
        public bool IsMultiple { get; set; }
        public ICollection<Option> Options { get; set; }
    }
}
