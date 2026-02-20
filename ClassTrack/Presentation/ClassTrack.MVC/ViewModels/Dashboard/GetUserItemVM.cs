


namespace ClassTrack.MVC.ViewModels
{
    public record GetUserItemVM(
        
        string UserId,
        string Email,
        string UserFullName,
        ICollection<string> Roles);
    
}
