using System.ComponentModel.DataAnnotations;

namespace Project_Management_System.ViewModels
{
    public class DelayViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Due date is required.")]
        [Display(Name = "Due Date")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public string EndDate { get; set; }

    }

}