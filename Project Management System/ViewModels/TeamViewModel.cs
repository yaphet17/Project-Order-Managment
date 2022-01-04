using System.Collections.Generic;
using Project_Management_System.Models;

namespace Project_Management_System.ViewModels.ProjectStructure
{
    public class AddTeamMemberViewModel
    {
        public int TmId { get; set; }

        public string UId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool CheckboxAnswer { get; set; }
    }

    public class ChangeMemberRoleViewModel
    {
        public int TmId { get; set; }

        public string UId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string TeamRole { get; set; }
    }

    public class TeamDetailViewModel
    {
        public int TmId { get; set; }

        public string TeamName { get; set; }

        public int PId { get; set; }

        public string ProjectName { get; set; }

        public List<TeamMemberInfoViewModel> TeamMembersInfo { get; set; }

    }

    public class TeamMemberInfoViewModel
    {
        public string UId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string TeamRole { get; set; }

    }

}