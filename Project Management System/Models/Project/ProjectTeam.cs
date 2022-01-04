using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Management_System.Models
{
    public class ProjectTeam
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Project not selected.")]
        public int PId { get; set; }

        [Required(ErrorMessage = "Team name is required.")]
        public string TeamName { get; set; }

        [ForeignKey("PId")]
        public Project Project { get; set; }


    }
}