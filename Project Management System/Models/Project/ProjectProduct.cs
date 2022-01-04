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
    public class ProjectProduct{
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage="Project must be selected.")]
        public int PId { get; set; }

        [Required(ErrorMessage="Product name is required.")]
        [Display(Name="Product Name")]
        public string ProductName { get; set; }

        [Required(ErrorMessage="Prescription plan is required.")]
        [Display(Name="Prescription Plan")]
        [DataType(DataType.Date,ErrorMessage="Invalid date format.")]
        public string PrescPlan { get; set; }

        [Required(ErrorMessage="Prescription actual is required.")]
        [Display(Name="Prescription Actual")]
        [DataType(DataType.Date,ErrorMessage="Invalid date format.")]
        public string PrescActual { get; set; }

        [Required(ErrorMessage="Draft ready plan is required.")]
        [Display(Name="Draft Ready Plan")]
        [DataType(DataType.Date,ErrorMessage="Invalid date format.")]
        public string DraftRPlan { get; set; }

        [Required(ErrorMessage="Draft ready actual is required.")]
        [Display(Name="Draft Ready Actual")]
        [DataType(DataType.Date,ErrorMessage="Invalid date format.")]
        public string DraftAPlan { get; set; }

        [Required(ErrorMessage="Final quality check completed plan is required.")]
        [Display(Name="Final Quality Check Completed Plan")]
        [DataType(DataType.Date,ErrorMessage="Invalid date format.")]
        public string Fqccp { get; set; }

        [Required(ErrorMessage="Final quality check completed actual is required.")]
        [Display(Name="Final Quality Check Completed Actual")]
        [DataType(DataType.Date,ErrorMessage="Invalid date format.")]
        public string Fqcca { get; set; }

        [Required(ErrorMessage="Approved plan is required.")]
        [Display(Name="Approved Plan")]
        [DataType(DataType.Date,ErrorMessage="Invalid date format.")]
        public string ApprovedPlan { get; set; }

        [Required(ErrorMessage="Approved actual is required.")]
        [Display(Name="Approved Actual")]
        [DataType(DataType.Date,ErrorMessage="Invalid date format.")]
        public string ApprovedActual { get; set; }

        [Required(ErrorMessage="Handle over plan is required.")]
        [Display(Name="Handle Over Plan")]
        [DataType(DataType.Date,ErrorMessage="Invalid date format.")]
        public string HandleOPlan { get; set; }

        [Required(ErrorMessage="Handle over actual is required.")]
        [Display(Name="Handle Over Actual")]
        [DataType(DataType.Date,ErrorMessage="Invalid date format.")]
        public string HandleAPlan { get; set; }

        [ForeignKey("PId")]
        public Project Project { get; set; }


    }
}