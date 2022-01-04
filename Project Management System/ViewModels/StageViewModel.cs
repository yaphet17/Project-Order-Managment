using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Project_Management_System.ViewModels.ProjectStructure
{
    public class AddStageTasksViewModel
    {

        public int SId { get; set; }

        public int TId { get; set; }

        public string TaskName { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public bool CheckboxAnswer { get; set; }

    }

    public class StageDetailViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Stage Name")]
        public string StageName { get; set; }

        public int Duration { get; set; }

        public List<StageDetailInfoViewModel> StageTasks { get; set; }

    }

    public class StageDetailInfoViewModel
    {

        public int Id { get; set; }

        public int TId { get; set; }

        [Display(Name = "Task Name")]
        public string TaskName { get; set; }

        [Display(Name = "Start Date")]
        public string StartDate { get; set; }

        [Display(Name = "End Date")]
        public string EndDate { get; set; }

    }
}