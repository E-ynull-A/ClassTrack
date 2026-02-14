using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Domain.Entities
{
    public class RefreshToken:BaseEntity
    {
        public string Token { get; set; }
        public DateTime ExpiryTime { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}
