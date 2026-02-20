namespace ClassTrack.Application.DTOs
{
    public record GetUserItemDTO(
        
        string UserId,
        string Email,
        string UserFullName,
        ICollection<string> Roles);
    
}
