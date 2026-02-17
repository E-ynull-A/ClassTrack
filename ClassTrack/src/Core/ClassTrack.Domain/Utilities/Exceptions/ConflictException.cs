using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Domain.Utilities
{
    public class ConflictException:Exception
    {
        public ConflictException(string message):base(message)
        {
            
        }

        public ConflictException() : base("This Item already Excists") { }
    
    }
}
