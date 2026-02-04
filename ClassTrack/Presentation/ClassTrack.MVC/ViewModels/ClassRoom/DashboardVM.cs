using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ClassTrack.MVC.ViewModels
{
    public record DashboardVM(

        [ValidateNever]
        IEnumerable<GetClassRoomItemVM> Classes,
        PostClassRoomVM PostClass = null);
   
}
