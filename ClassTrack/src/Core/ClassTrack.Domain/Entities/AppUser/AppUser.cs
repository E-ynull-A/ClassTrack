using ClassTrack.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace ClassTrack.Domain.Entities
{
    public abstract class AppUser:IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public byte Age { get; set; }
        public DateOnly BirthDate { get; set; }
        public UserRole UserRole { get; set; }   

    }
}
