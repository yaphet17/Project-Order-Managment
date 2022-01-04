using System.Collections.Generic;
namespace Project_Management_System.HtmlLists
{
    public static class TaskStatusList
    {
        public static IEnumerable<Status> taskStatus = new List<Status> {
        new Status {
        Name = "Open",
        Value = "Open"
        },
        new Status {
            Name = "InProgress",
            Value = "InProgress"
        },
        new Status {
            Name = "InReview",
            Value = "InReview"
        },
         new Status {
            Name = "OnRole",
            Value = "OnRole"
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