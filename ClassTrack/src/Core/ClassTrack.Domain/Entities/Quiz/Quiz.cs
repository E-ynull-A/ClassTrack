

namespace ClassTrack.Domain.Entities
{
    public class Quiz:BaseNamebleEntity
    {
        public ICollection<ChoiceQuestion>? ChoiceQuestions { get; set; }
        public ICollection<OpenQuestion>? OpenQuestions { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime StartTime { get; set; }

        //Relations
        public ICollection<StudentQuiz> StudentQuizes { get; set; }

        public long ClassId { get; set; }
        public Class Class { get; set; }




    }
}
