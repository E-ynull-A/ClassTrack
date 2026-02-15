using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.MVC.ViewModels
{
    public record GetSimpleStudentItemVM(
        
        long Id,
        string Name,
        string Surname);
 
}
