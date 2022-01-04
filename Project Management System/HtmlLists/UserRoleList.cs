using System.Collections.Generic;
namespace Project_Management_System.HtmlLists
{
    public static class UserRoleList
    {
        public static IEnumerable<Status> userRole = new List<Status> {
        new Status {
            Name = "Admin",
            Value = "Admin"
        },
        new Status {
            Name = "User",
            Value = "User"
        }
    };
    }

}