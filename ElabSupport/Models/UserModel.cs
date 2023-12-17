using System.ComponentModel.DataAnnotations;

namespace ElabSupport.Models
{
    public class UserModel
    {
        public string UserId { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public string DeviceId { get; set; }

        [Required(ErrorMessage = "UserRole is required")]
        public int UserRoleId { get; set; }

        [Required(ErrorMessage = "RateType is required")]
        public int RateType { get; set; }

        [RegularExpression("^[0-9]+$", ErrorMessage = "Rates should be a valid integer.")]
        public string Rates { get; set; }

        public int Active { get; set; }

        [Required(ErrorMessage = "EmailId is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "MobileNumber is required")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Invalid Mobile Number")]
        public string MobileNumber { get; set; }

        public string Address { get; set; }
    }
}
