using Project_Management_System.Models;
using System.Collections.Generic;

namespace Project_Management_System.ViewModels.Identity{
    public class RoleDetailViewModel{
        
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<ApplicationPrivilege> Privileges { get; set; }
    }
}