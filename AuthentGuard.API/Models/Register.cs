using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AuthentGuard.API.Models
{
    public class Register
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please provide an email.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Email must be between 3 and 255 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please provide a password.")]
        [StringLength(255, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters.")]
        public string Password { get; set; }

        [Required]
        public bool AgrredToTerms { get; set; }

        [Required]
        public long CreationDateUnixTimestamp { get; set; }

        public long LastLoginUnixTimestamp { get; set; }
    }
}
