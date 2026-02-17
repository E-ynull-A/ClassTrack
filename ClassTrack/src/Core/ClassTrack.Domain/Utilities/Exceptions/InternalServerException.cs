using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Domain.Utilities
{
    public class InternalServerException:Exception
    {
        public InternalServerException(string message):base(message)
        {
            
        }

        public InternalServerException() : base("An unexpected error occurred on the server side") { }
      
    }
}
