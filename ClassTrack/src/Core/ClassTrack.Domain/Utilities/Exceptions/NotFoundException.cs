using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Domain.Utilities
{
    public class NotFoundException:Exception
    {
        public NotFoundException(string message):base(message)
        {
            
        }

        public NotFoundException():base("404 Not Found"){}
    }
}
