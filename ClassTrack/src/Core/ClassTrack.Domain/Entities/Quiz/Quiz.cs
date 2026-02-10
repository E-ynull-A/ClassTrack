

namespace ClassTrack.Domain.Entities
{
    public class Quiz:BaseNamebleEntity
    {
        public ICollection<ChoiceQuestion>? ChoiceQuestions { get; set; }
        public ICollection<OpenQuestion>? OpenQuestions { get; set; }
        public decimal FullPoint { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime StartTime { get; set; }


        //Relations
        public ICollection<StudentQuiz> StudentQuizes { get; set; }

        public long ClassRoomId { get; set; }
        public ClassRoom ClassRoom { get; set; }
    }
}
