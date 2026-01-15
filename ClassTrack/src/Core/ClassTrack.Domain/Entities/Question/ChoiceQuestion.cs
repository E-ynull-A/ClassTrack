

namespace ClassTrack.Domain.Entities
{
    public class ChoiceQuestion:Question
    {
        public ICollection<Option> Options { get; set; }       
        public bool IsMultiple { get; set; }
    }
}
