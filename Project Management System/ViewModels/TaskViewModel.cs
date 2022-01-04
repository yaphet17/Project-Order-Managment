using System.ComponentModel.DataAnnotations;

namespace Project_Management_System.ViewModels.ProjectStructure
{


    public class AddTaskAttachmentViewModel
    {

        public int TId { get; set; }

        public string Path { get; set; }
    }
    public class AssignTaskOwnerViewModel
    {
        public int TId { get; set; }

        public string TeamName { get; set; }

        public string TeamRole { get; set; }

        public string UId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool CheckboxAnswer { get; set; }

    }
    public class EditTaskViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Task name is required.")]
        [Display(Name = "Task Name")]
        public string TaskName { get; set; }

        [Required(ErrorMessage = "Task description is required.")]
        [Display(Name = "Task Description")]
        public string Description { get; set; }

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
    }

    public class ChangeTaskStatusViewModel
    {
        public int TId { get; set; }

        [Display(Name = "Task Name")]
        public string TaskName { get; set; }
        public string Status { get; set; }

    }

    public class TaskViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Task Name")]
        public string TaskName { get; set; }

        public int PId { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Display(Name = "Task Owner")]
        public string TaskOwner { get; set; }

        public string Description { get; set; }

        [Display(Name = "Start Date")]
        public string StartDate { get; set; }

        [Display(Name = "End Date")]
        public string EndDate { get; set; }

        public double Weight { get; set; }

        public string Status { get; set; }

        [Display(Name = "Task Attachment")]
        public string AttachmentPath { get; set; }
    }

}