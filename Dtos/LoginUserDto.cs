using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos
{
    public class LoginUserDto
    {
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [MaxLength(100), MinLength(6)]
        [EmailAddress(ErrorMessage = "Email address is not valid")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;

        public bool IsAdmin { get; set; } = false;
    }
}