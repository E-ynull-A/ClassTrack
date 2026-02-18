using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Domain.Utilities
{
    public class BusinessLogicException:Exception
    {
        public BusinessLogicException(string message):base(message)
        {
            
        }

        public BusinessLogicException() : base("Business rule violation occurred") { }
      
    }
}
