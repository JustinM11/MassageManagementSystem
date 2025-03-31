using Microsoft.AspNetCore.Identity;

namespace MassageManagementSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsAdmin { get; set; } = false;

    }
}
