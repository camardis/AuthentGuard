using Microsoft.AspNetCore.Identity;

namespace AuthentGuard.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime RegistrationDate { get; set; }
        public bool IsEmailConfirmed { get; set; }

        public ApplicationUser()
        {
            RegistrationDate = DateTime.UtcNow;
            IsEmailConfirmed = false;
        }
    }
}
