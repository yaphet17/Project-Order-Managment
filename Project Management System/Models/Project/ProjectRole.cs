using System.ComponentModel.DataAnnotations;
namespace Project_Management_System.Models
{
    public class ProjectRole
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Role name is field required.")]
        [MaxLength(50, ErrorMessage = "Role name can't be more than 50 characters long.")]
        [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Invalid role name.")]
        public string RoleName { get; set; }

    }
}