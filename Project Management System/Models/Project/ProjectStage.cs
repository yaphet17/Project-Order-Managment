using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Management_System.Models
{
    public class ProjectStage
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Project must be selected.")]
        public int PId { get; set; }

        [Required(ErrorMessage = "Stage name is required.")]
        [MaxLength(50, ErrorMessage = "Stage name can't be more than 50 characters long.")]
        [Display(Name = "Stage Name")]
        [Remote(action: "IsStageNameTaken", controller: "project")]
        public string StageName { get; set; }

        [Required(ErrorMessage = "Stage weight required.")]
        [Display(Name = "Stage Weight")]
        public double StageWeight { get; set; }

        public int Duration { get; set; }

        [ForeignKey("PId")]
        public Project Project { get; set; }
    }
}