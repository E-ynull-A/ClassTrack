namespace ClassTrack.MVC.ViewModels
{
    public record GetUserPagedItemVM(
        
        ICollection<GetUserItemVM> UserItems,
        int TotalCount);
    
}
