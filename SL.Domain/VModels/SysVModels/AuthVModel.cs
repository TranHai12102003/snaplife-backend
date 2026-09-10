using System.ComponentModel.DataAnnotations;

namespace SL.Domain.VModels.SysVModels
{
    public class LoginVModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email is invalid!")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; set; }
    }

    public class RegisterVModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email is invalid!")]
        public required string Email { get; set; }

        public string? UserName { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "ConfirmPassword is required")]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public required string ConfirmPassword { get; set; }
    }

    public class LoginResponse
    {
        public string? Token { get; set; }
        public string? Message { get; set; }
        public bool IsSuccess { get; set; }
        public MeVModel? User { get; set; }
    }

    public class RegisterResponse
    {
        public string? Message { get; set; }
        public bool IsSuccess { get; set; }
        public MeVModel? User { get; set; }
    }

    public class MeVModel
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? AvatarUrl { get; set; }
        public bool? Sex { get; set; }
        public DateOnly? Birthday { get; set; }
        public string? Address { get; set; }
        public string? Bio { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();
    }
}

