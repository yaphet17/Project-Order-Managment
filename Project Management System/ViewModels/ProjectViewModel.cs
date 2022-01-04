using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Project_Management_System.Models;

namespace Project_Management_System.ViewModels.ProjectStructure
{
    public class AssignRoleViewModel
    {

        [Required(ErrorMessage = "User must be selected.")]
        public string UId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required(ErrorMessage = "Project must be selected.")]
        public int PId { get; set; }

        [Required(ErrorMessage = "Role must be specified.")]
        public int RId { get; set; }

        public bool CheckboxAnswer { get; set; }

    }
    public class ChangeProjectStatusViewModel
    {
        public int PId { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }
        public string Status { get; set; }

    }

    public class DetailViewModel : Project
    {
        public string ProjectManager { get; set; }
        public string Executive { get; set; }
        public Dictionary<Dictionary<int, string>, List<ProjectTask>> StageTaskDict { get; set; }
        public List<ProjectProduct> ProjectProducts { get; set; }
        public List<ProjectTask> ProjectTasks { get; set; }
        public List<ProjectTeam> ProjectTeams { get; set; }
    }

}