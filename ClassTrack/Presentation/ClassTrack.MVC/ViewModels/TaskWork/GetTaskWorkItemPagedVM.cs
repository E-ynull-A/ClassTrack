using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.MVC.ViewModels
{
    public record GetTaskWorkItemPagedVM(
        
        ICollection<GetTaskWorkItemVM> TaskWorkItem,
        int TotalCount);
   
}
