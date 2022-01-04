using System.Collections.Generic;
namespace Project_Management_System.HtmlLists
{
    public static class ProjectStatusList
    {
        public static IEnumerable<Status> projectStatus = new List<Status> {
        new Status {
        Name = "Initiated",
        Value = "Initiated"
        },
        new Status {
            Name = "Started",
            Value = "Started"
        },
        new Status {
            Name = "Delayed",
            Value = "Delayed"
        },
        new Status {
            Name = "Canceled",
            Value = "Canceled"
        },
        new Status {
            Name = "Completed",
            Value = "Completed"
        }
    };
    }

}