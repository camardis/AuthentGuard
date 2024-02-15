using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthentGuard.Models
{
    public class RegisterModel
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
        public long CreationDateUnixTimestamp { get; set; }
    }
}
