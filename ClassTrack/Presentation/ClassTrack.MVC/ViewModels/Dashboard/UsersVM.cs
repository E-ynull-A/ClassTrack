using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ClassTrack.MVC.ViewModels
{
    public record UsersVM(

        [ValidateNever]
        GetUserPagedItemVM UserVM,
        PostBanUserVM PostBan);
  
}
