using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Management_System.Models
{
    public class StageTasks
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Stage must be selected.")]
        public int SId { get; set; }

        [Required(ErrorMessage = "Task must be selected.")]
        public int TId { get; set; }

        [ForeignKey("SId")]
        public ProjectStage ProjectStage { get; set; }

        [ForeignKey("TId")]
        public ProjectTask ProjectTask { get; set; }
    }
}