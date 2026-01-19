

namespace ClassTrack.Domain.Entities
{
    public abstract class Question:BaseAccountableEntity
    {
        public string Title { get; set; }
        public decimal Point { get; set; }

        //Relations
        public long QuizId { get; set; }
        public Quiz Quiz { get; set; }
        public ICollection<Option> Options { get; set; } = new List<Option>();

    }
}
