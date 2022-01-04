using System.ComponentModel.DataAnnotations;

namespace Project_Management_System.ViewModels.Identity
{
    public class EditProfileViewModel
    {

        public string Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [Display(Name = "First Name")]
        [MaxLength(50, ErrorMessage = "Name can't be more than 50 characters long.")]
        [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Invalid first name.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [Display(Name = "Last Name")]
        [MaxLength(50, ErrorMessage = "Name can't be more than 50 characters long.")]
        [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Invalid last name.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

    }
    //This class inherit all properties of ResetPasswordViewModel
    public class ChangePasswordViewModel : ResetPasswordViewModel
    {

        [Required(ErrorMessage = "Old password is required.")]
        [Display(Name = "Old Password")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

    }
}