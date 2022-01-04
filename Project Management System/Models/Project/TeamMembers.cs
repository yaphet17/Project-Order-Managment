using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Project_Management_System.Models;

namespace Project_Management_System.Models
{
    public class TeamMembers
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Team must be selected.")]
        public int TmId { get; set; }

        [Required(ErrorMessage = "User must be selected.")]
        public string UId { get; set; }

        [Required(ErrorMessage = "Role is required")]
        [Display(Name = "Role")]
        [RegularExpression(@"^(ProjectManager|SeniorSupplier|Executive|ProjectBoard|TeamMember|TeamManager)$", ErrorMessage = "Invalid role value.")]
        public string TeamRole { get; set; }

        [ForeignKey("TmId")]
        public ProjectTeam ProjectTeam { get; set; }

        [ForeignKey("UId")]
        public virtual ApplicationUser User { get; set; }

    }
}