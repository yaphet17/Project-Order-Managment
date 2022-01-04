using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Project_Management_System.Models;

namespace Project_Management_System.Models
{
    public class ProjectMembersRole
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "User must be selected.")]
        public string UId { get; set; }

        [Required(ErrorMessage = "Project must be selected")]
        public int PId { get; set; }

        [Required(ErrorMessage = "Role must be selected.")]
        public int RId { get; set; }

        [ForeignKey("UId")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("PId")]
        public virtual Project Project { get; set; }

        [ForeignKey("RId")]
        public virtual ProjectRole ProjectRole { get; set; }

    }
}