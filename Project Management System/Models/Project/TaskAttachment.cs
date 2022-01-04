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
    public class TaskAttachment{
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage="Task must be selected.")]
        public int TId { get; set; }

        [Required(ErrorMessage="Attachment file must be selected.")]
        public string Path { get; set; }

        [ForeignKey("TId")]
        public ProjectTask ProjectTask { get; set; }

    }
    
}