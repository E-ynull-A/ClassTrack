namespace ClassTrack.Application.DTOs
{
    public record GetQuestionItemDTO(
        string Type,
        string Title,
        decimal Point,        
        ICollection<GetOptionItemInQuestionDTO>? Options=null)
    {
        public GetQuestionItemDTO() : this(string.Empty,string.Empty, 0, new List<GetOptionItemInQuestionDTO>()) { }
    };


                                        
}
