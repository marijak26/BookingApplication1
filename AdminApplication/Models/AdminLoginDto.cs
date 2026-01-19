using System.ComponentModel.DataAnnotations;

namespace AdminApplication.Models
{
    public class AdminLoginDto
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
