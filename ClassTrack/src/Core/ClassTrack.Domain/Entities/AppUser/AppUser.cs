using ClassTrack.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace ClassTrack.Domain.Entities
{
    public class AppUser:IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public byte Age { get; set; }
        public DateOnly BirthDate { get; set; }

        //Relations

        public long StudentId { get; set; }
        public Student Student { get; set; }

        public long TeacherId { get; set; }
        public Teacher Teacher { get; set; }




    }
}
