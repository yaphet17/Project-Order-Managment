using System.Collections.Generic;
namespace Project_Management_System.HtmlLists
{
    public static class TeamMemberRoleList
    {
        public static IEnumerable<Status> memberRole = new List<Status> {
        new Status {
            Name = "Team Member",
            Value = "TeamMember"
        },
        new Status {
            Name = "Team Manager",
            Value = "TeamManager"
        }
    };
    }

}