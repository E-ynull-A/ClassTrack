namespace ClassTrack.Application.DTOs
{
    public record GetQuestionItemDTO(
        long Id,
        string QuestionType,
        string Title)
    {
        public GetQuestionItemDTO() : this(0,string.Empty,string.Empty) { }
    };


                                        
}
