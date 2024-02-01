namespace AuthentGuard.Models
{
    public class RegisterModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateOnly CreationDate { get; set; }

    }
}
