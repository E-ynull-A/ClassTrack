using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Domain.Utilities
{
    public class BadRequestException:Exception
    {
        public BadRequestException(string message):base(message)
        {
            
        }
        public BadRequestException():base("Bad Request"){}
      

    }
}
