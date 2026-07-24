using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StajBackendProject.Models.Dto
{
    public class AddNewUserDto
    {
        [Required(ErrorMessage = "First Name cannot be empty.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First Name must be at least 2 and at most 50 characters long.")]
        [DefaultValue("John")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name cannot be empty.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last Name must be at least 2 and at most 50 characters long.")]
        [DefaultValue("Doe")]

        public string LastName { get; set; }

        [Required(ErrorMessage = "Email cannot be empty.")]
        [EmailAddress(ErrorMessage = "Please enter valid Email address.")]
        [DefaultValue("johndoe@example.com")]
        public string Email { get; set; }

        [StringLength(128, MinimumLength = 8, ErrorMessage = "Password must be at least 8 and at most 128 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&.,#])[A-Za-z\d@$!%*?&.,#]{8,128}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        [DefaultValue("Password123!")]
        public string Password { get; set; }

        [RegularExpression(@"^(05|5)[0-9][0-9][\s]([0-9]{3})[\s]([0-9]{2})[\s]([0-9]{2})$|^((05|5)[0-9]{9})$", 
            ErrorMessage = "Please enter valid turkish phone number.")]
        public string PhoneNumber { get; set; }
    }
}
