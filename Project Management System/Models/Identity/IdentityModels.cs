using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Project_Management_System.Models
{
    public class ApplicationUser : IdentityUser
    {

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

        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        public virtual ICollection<ProjectTask> ProjectTask { get; set; }
        public virtual ICollection<TaskComment> TaskComment { get; set; }
        public virtual ICollection<TeamMembers> TeamMembers { get; set; }

    }

    public class ApplicationPrivilege
    {
        [Key]
        public string Id { get; set; }
        [Required(ErrorMessage = "Action must be specified")]
        [Remote(action: "IsPrivilegeExists", controller: "privilege")]
        public string Action { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ApplicationRolePrivilege> RolePrivileges { get; set; }
    }

    public class ApplicationRole : IdentityRole
    {
        public virtual string Description { get; set; }
        public virtual ICollection<ApplicationRolePrivilege> RolePrivileges { get; set; }
    }
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public ApplicationUserRole()
            : base()
        { }
        public virtual ApplicationRole Role { get; set; }
    }
    public class ApplicationRolePrivilege
    {
        public string RoleId { get; set; }
        public string PrivilegeId { get; set; }

        [ForeignKey("RoleId")]
        public virtual ApplicationRole Role { get; set; }
        [ForeignKey("PrivilegeId")]
        public virtual ApplicationPrivilege Privilege { get; set; }
    }

}