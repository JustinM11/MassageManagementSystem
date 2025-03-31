using Microsoft.AspNetCore.Identity;

namespace MassageManagementSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Additional user properties can be added here.
        public bool IsAdmin { get; set; } = false;

    }
}
