namespace ClassTrack.Application.DTOs
{
    public record GetQuestionItemDTO(
        string Type,
        string Title,
        decimal Point,        
        ICollection<GetOptionItemInQuestionDTO>? Options = null);                                    
}
