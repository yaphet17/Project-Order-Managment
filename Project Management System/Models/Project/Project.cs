using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Project_Management_System.Models
{
    public class Project
    {
        [Key]
        [Display(Name = "Project Id")]
        public int PId { get; set; }

        [Required(ErrorMessage = "Project name must be specifed.")]
        [Display(Name = "Project Name")]
        [Remote(action: "IsProjectNameTaken", controller: "project")]
        public string ProjectName { get; set; }

        [Required(ErrorMessage = "Project must have description.")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Status")]
        [RegularExpression(@"^(Initiated|Started|Delayed|Cancelled|Completed)$", ErrorMessage = "Invalid status value.")]
        public string Status { get; set; }


        [Required(ErrorMessage = "Priority not selected.")]
        [Display(Name = "Priority")]
        [RegularExpression(@"^(High|Medium|Low)$", ErrorMessage = "Invalid priority value.")]
        public string Priority { get; set; }

        [Required]
        [Display(Name = "Creation Date")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public string CreatedDate { get; set; }

        [Required(ErrorMessage = "Starting date is required.")]
        [Display(Name = "Starting Date")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public string StartDate { get; set; }

        [Required(ErrorMessage = "Due date is required.")]
        [Display(Name = "Due Date")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public string EndDate { get; set; }

    }

}