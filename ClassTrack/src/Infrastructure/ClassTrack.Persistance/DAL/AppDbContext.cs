using ClassTrack.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ClassTrack.Persistance.DAL
{
    internal class AppDbContext:IdentityDbContext<AppUser>
    {

    }
}
