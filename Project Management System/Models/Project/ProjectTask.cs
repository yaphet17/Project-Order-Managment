using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Management_System.Models
{
    public class ProjectTask
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Project must be selected.")]
        public int PId { get; set; }

        [Required(ErrorMessage = "Task name is required.")]
        [Display(Name = "Task Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Task description is required.")]
        [Display(Name = "Task Description")]
        public string Description { get; set; }

        public string UId { get; set; }

        [Required(ErrorMessage = "Starting date is required.")]
        [Display(Name = "Starting Date")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public string StartDate { get; set; }

        [Required(ErrorMessage = "Due date is required.")]
        [Display(Name = "Due Date")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public string EndDate { get; set; }

        [Required(ErrorMessage = "Task weight is required.")]
        [Display(Name = "Task Weight")]
        public double Weight { get; set; }

        [Required]
        [Display(Name = "Status")]
        [RegularExpression(@"^(Open|InProgress|InReview|ToBeTested|OnRole|Delayed|Cancelled|Completed)$", ErrorMessage = "Invalid status value.")]
        public string Status { get; set; }

        [ForeignKey("PId")]
        public Project Project { get; set; }

        [ForeignKey("UId")]
        public virtual ApplicationUser User { get; set; }

        public virtual StageTasks StageTasks{ get; set; }


    }

}