namespace ClassTrack.Application.DTOs
{
    public record GetQuestionItemDTO(
        long Id,
        string QuestionType,
        string Title,
        decimal Point,        
        ICollection<GetOptionItemInQuestionDTO>? Options=null)
    {
        public GetQuestionItemDTO() : this(0,string.Empty,string.Empty, 0, new List<GetOptionItemInQuestionDTO>()) { }
    };


                                        
}
