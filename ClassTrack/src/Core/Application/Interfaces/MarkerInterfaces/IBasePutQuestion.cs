using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.Interfaces
{
    public interface IBasePutQuestion
    {
        public string Title { get; }
        public decimal Point { get; }
    }
}
