using System.Collections.Generic;
namespace Project_Management_System.HtmlLists
{
    public static class ProjectMemberRoleList
    {
        public static IEnumerable<Status> memberRole = new List<Status> {
        new Status {
            Name = "Executive",
            Value = "Executive"
        },
        new Status {
            Name = "Senior Supplier",
            Value = "SeniorSupplier"
        },
        new Status {
            Name = "Senior User",
            Value = "SeniorUser"
        },new Status {
            Name = "Project Manager",
            Value = "ProjectManager"
        }
    };
    }

}