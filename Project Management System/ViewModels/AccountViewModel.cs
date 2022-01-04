using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Project_Management_System.Models;
namespace Project_Management_System.ViewModels.Identity
{

    public class LogInViewModel
    {

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }


    }
    public class RegisterViewModel
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
        [Remote(action: "IsEmailInUse", controller: "Account")]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password can't be less than 50 characters long.")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password doesn't much.")]
        public string ConfirmPassword { get; set; }

    }

    public class EditUserInfoViewModel : EditProfileViewModel
    {
        public IEnumerable<string> Role { get; set; }
        [Display(Name = "Role")]
        public IEnumerable<ApplicationRole> Roles{ get; set; }
    }
    public class ResetPasswordViewModel
    {

        [Required(ErrorMessage = "User must be selected.")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password can't be less than 50 characters long.")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Password doesn't much.")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

    }
    public class UsersListViewModel
    {

        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}