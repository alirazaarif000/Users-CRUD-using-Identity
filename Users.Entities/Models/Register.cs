using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Users.Entities.Models
{
    public class Register
    {
        [ValidateNever]
        public string Id { get; set; }

        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }

        [ValidateNever]
        public IEnumerable<string> RoleList { get; set; }
    }
}
