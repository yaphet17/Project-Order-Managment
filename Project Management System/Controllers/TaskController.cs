using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Management_System.Models;
using Project_Management_System.ViewModels;
using Project_Management_System.ViewModels.ProjectStructure;
using Project_Management_System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Project_Management_System.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _iwebhost;

        public TaskController(ApplicationDbContext db, IWebHostEnvironment iwebhost)
        {
            _db = db;
            _iwebhost = iwebhost;
        }

        [HttpGet]
        [Route("project/task/create")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Create(int pid)
        {
            var project = await _db.project.FindAsync(pid);
            if (project == null)
            {
                return NotFound();
            }
            var model = new ProjectTask
            {
                PId = pid
            };
            return View(model);
        }

        [HttpPost]
        [Route("project/task/create")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Create(ProjectTask model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Some error occurred.");
                return View(model);
            }

            var project = await _db.project.FindAsync(model.PId);
            DateTime projectSDate = DateTime.Parse(project.StartDate);
            DateTime taskSDate = DateTime.Parse(model.StartDate);
            //Display error if task starting date is before project starting date
            if (taskSDate.Date < projectSDate.Date)
            {
                ViewBag.message = $"Task can't start before {project.StartDate}.";
                return View(model);
            }
            DateTime projectEDate = DateTime.Parse(project.EndDate);
            DateTime taskEDate = DateTime.Parse(model.EndDate);
            //Display error if task due date is after project due date 
            if (taskEDate > projectEDate)
            {
                ViewBag.message = $"Task must be completed before {project.EndDate}.";
                return View(model);
            }
            await _db.projectTask.AddAsync(model);
            await _db.SaveChangesAsync();
            return RedirectToAction("AddAttachment", "Task", new { id = model.Id });
        }

        [Route("project/task/detail")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Detail(int id)
        {

            var projectTask = await _db.projectTask.FindAsync(id);
            if (projectTask == null)
            {
                return NotFound();
            }
            string taskOwner;
            if (projectTask.UId == "" || projectTask.UId == null)
            {
                taskOwner = "Owner not assigned";
            }
            else
            {
                var user = await _db.applicationUser.FindAsync(projectTask.UId);
                taskOwner = user.FirstName + " " + user.LastName;
            }
            var project = await _db.project.FindAsync(projectTask.PId);
            var taskAttachment = await _db.taskAttachment.Where(e => e.TId == id).ToListAsync();
            string path;
            if (taskAttachment.Count() > 0)
            {
                path = taskAttachment[0].Path;
            }
            else
            {
                path = "No Attachment";
            }
            var model = new TaskViewModel
            {
                Id = projectTask.Id,
                TaskName = projectTask.Name,
                PId = project.PId,
                ProjectName = project.ProjectName,
                TaskOwner = taskOwner,
                Description = projectTask.Description,
                StartDate = projectTask.StartDate,
                EndDate = projectTask.EndDate,
                Weight = projectTask.Weight,
                Status = projectTask.Status,
                AttachmentPath = path
            };
            return View(model);

        }

        [HttpGet]
        [Route("project/task/edit")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Edit(int id)
        {
            var projectTask = await _db.projectTask.FindAsync(id);
            if (projectTask == null)
            {
                return NotFound();
            }
            var model = new EditTaskViewModel
            {
                Id = id,
                TaskName = projectTask.Name,
                Description = projectTask.StartDate,
                StartDate = projectTask.StartDate,
                EndDate = projectTask.EndDate,
                Weight = projectTask.Weight
            };
            return View(model);
        }

        [HttpPost]
        [Route("project/task/edit")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Edit(EditTaskViewModel model)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Some error occurred");
            }

            var newTask = await _db.projectTask.FindAsync(model.Id);
            DateTime oldSDate = DateTime.Parse(model.StartDate);
            DateTime newSDate = DateTime.Parse(newTask.StartDate);
            //Prevent change of starting date of on progress task
            if (!string.Equals(newTask.Status, "Open") && !string.Equals(newTask.Status, "Delayed") && oldSDate != newSDate)
            {
                ViewBag.message = $"You can't edit starting date of {newTask.Status} tasks.";
                return View(model);
            }
            var project = await _db.project.FindAsync(newTask.PId);
            DateTime projectSDate = DateTime.Parse(project.StartDate);
            DateTime modelSDate = DateTime.Parse(model.StartDate);
            //Checking project's task starting date is greater than projects start date 
            if (projectSDate.Date > modelSDate.Date)
            {
                DateTime projectEDate = DateTime.Parse(project.StartDate);
                DateTime modelEDate = DateTime.Parse(model.StartDate);
                //Checking project's task end/due date is less than projects end/due date 
                if (modelEDate < projectEDate)
                {
                    ViewBag.message = "Task end date must be greater than or equal to project end date.";
                    return View(model);
                }
                ViewBag.message = "Task start date must be less than or equal to project start date.";
                return View(model);
            }
            newTask.Id = model.Id;
            newTask.Name = model.TaskName;
            newTask.Description = model.Description;
            newTask.StartDate = model.StartDate;
            newTask.EndDate = model.EndDate;
            newTask.Weight = model.Weight;
            await _db.SaveChangesAsync();
            TempData["tkmessage"] = "Task Successfully Updated";
            return RedirectToAction("Detail", "Task", new { id = model.Id });

        }

        [Route("project/task/delete")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Delete(int id)
        {
            var projectTask = await _db.projectTask.FindAsync(id);
            if (projectTask == null)
            {
                return NotFound();
            }
            var Status = projectTask.Status;
            if (!string.Equals(Status, "Open") && !string.Equals(Status, "Delayed") && !string.Equals(Status, "Cancelled"))
            {
                TempData["errorTkMessage"] = "Task can't be deleted.";
                return Redirect(Request.Headers["Referer"].ToString());
            }
            _db.projectTask.Remove(projectTask);
            await _db.SaveChangesAsync();
            TempData["tkMessage"] = "Task successfully deleted.";
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpGet]
        [Route("project/task/change-status")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> ChangeStatus(int id)
        {

            var projectTask = await _db.projectTask.FindAsync(id);
            if (projectTask == null)
            {
                return NotFound();
            }
            var model = new ChangeTaskStatusViewModel
            {
                TId = projectTask.Id,
                TaskName = projectTask.Name,
                Status = projectTask.Status
            };
            return View(model);
        }

        [HttpPost]
        [Route("project/task/change-status")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> ChangeStatus(ChangeTaskStatusViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Some error occurred.");
                return View(model);
            }
            var newTask = await _db.projectTask.FindAsync(model.TId);
            //Prevent change of status of completed projects
            if (string.Equals(newTask.Status, "Completed"))
            {
                TempData["errorTkMessage"] = "Status can't be changed..";
                return RedirectToAction("Detail", new { pid = model.TId });
            }

            newTask.Status = model.Status;
            await _db.SaveChangesAsync();
            TempData["tkMessage"] = "Status successfully updated.";
            return RedirectToAction("Detail", "Task", new { id = model.TId });

        }

        [HttpGet]
        [Route("project/task/delay")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Delay(int id)
        {

            var projectTask = await _db.projectTask.FindAsync(id);
            if (projectTask == null)
            {
                return NotFound();
            }
            var model = new DelayViewModel
            {
                Id = projectTask.Id,
                EndDate = projectTask.EndDate
            };
            return View(model);
        }

        [HttpPost]
        [Route("project/task/delay")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Delay(DelayViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Some error occurred.");
                return View(model);
            }
            var delayedTask = await _db.projectTask.FindAsync(model.Id);
            var project = await _db.project.FindAsync(delayedTask.PId);
            DateTime projectEDate = DateTime.Parse(project.EndDate);
            DateTime modelEDate = DateTime.Parse(model.EndDate);
            if (modelEDate < projectEDate)
            {
                ViewBag.message = "Task due date must be greater than or equal to project due date.";
                return View(model);
            }
            DateTime oldEDate = DateTime.Parse(delayedTask.EndDate);
            if (modelEDate <= oldEDate)
            {
                ViewBag.message = "New due date must be greater old due date.";
                return View(model);
            }
            delayedTask.EndDate = model.EndDate;
            delayedTask.Status = "Delayed";
            await _db.SaveChangesAsync();
            TempData["tkMessage"] = "Task successfully delayed.";
            return RedirectToAction("Detail", "Task", new { id = model.Id });

        }

        [HttpGet]
        [Route("project/task/add-attachment")]
        [ProjectOrderManagementAuthorizationFilter]
        public IActionResult AddAttachment(int id)
        {

            var model = new AddTaskAttachmentViewModel
            {
                TId = id,
            };
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        [Route("project/task/add-attachment")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> AddAttachment(IFormFile file, AddTaskAttachmentViewModel model)
        {
            string fileExt = Path.GetExtension(file.FileName);
            long maxFileSize = 20 * 1024 * 1024;//limiting maximum allowable file size to 20MB
            if (string.Equals(fileExt, ".pdf") || string.Equals(fileExt, ".docx") || string.Equals(fileExt, ".ppt"))
            {
                if (file.Length < maxFileSize)
                {
                    var saveFile = Path.Combine(_iwebhost.WebRootPath, "Task-Attachments", file.FileName);
                    FileStream stream = new FileStream(saveFile, FileMode.Create);
                    await file.CopyToAsync(stream);
                    var newModel = new TaskAttachment
                    {
                        TId = model.TId,
                        Path = saveFile
                    };
                    await _db.taskAttachment.AddAsync(newModel);
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Detail", "Task", new { id = model.TId });
                }
                ViewBag.message = "File size too large.";
                return View(model);
            }
            ViewBag.message = "File extension is no allowed.";
            return View(model);
        }


        [HttpGet]
        [Route("project/task/assign-owner")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> AssignOwner(int id)
        {

            var projectTask = await _db.projectTask.FindAsync(id);
            if (projectTask == null)
            {
                return NotFound();
            }
            var projectTeams = await _db.projectTeam.Where(e => e.PId == projectTask.PId).Select(e => e.Id).ToListAsync();
            var teamMembers = await _db.teamMember.Where(e => projectTeams.Contains(e.TmId)).ToListAsync();
            List<AssignTaskOwnerViewModel> model = new List<AssignTaskOwnerViewModel>();
            foreach (var user in teamMembers)
            {
                var tempUser = await _db.applicationUser.FindAsync(user.UId);
                var team = await _db.projectTeam.FindAsync(user.TmId);
                var tempModel = new AssignTaskOwnerViewModel
                {
                    TId = projectTask.Id,
                    TeamName = team.TeamName,
                    TeamRole = user.TeamRole,
                    UId = user.UId,
                    FirstName = tempUser.FirstName,
                    LastName = tempUser.LastName,
                    CheckboxAnswer = false
                };
                model.Add(tempModel);
            }
            return View(model);
        }

        [HttpPost]
        [Route("project/task/assign-owner")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> AssignOwner(List<AssignTaskOwnerViewModel> model)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Some error occured.");
                return View(model);
            }
            var projectTask = await _db.projectTask.FindAsync(model[0].TId);
            if (projectTask.UId != null)
            {
                TempData["errorTkMessage"] = "Task owner already assigned";
                return RedirectToAction("Detail", "Task", new { id = model[0].TId });
            }
            for (int i = 0; i < model.Count(); i++)
            {
                if (model[i].CheckboxAnswer == true)
                {
                    var newModel = await _db.projectTask.FindAsync(model[i].TId);
                    newModel.UId = model[i].UId;
                    await _db.SaveChangesAsync();
                    TempData["tkMessage"] = "Task owner successfully assgined";
                    return RedirectToAction("Task", new { id = model[i].TId });
                }

            }
            return RedirectToAction("Detail", "Task", new { id = model[0].TId });
        }

        [Route("project/task/delete-owner")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> DeleteOwner(int id)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Some error occured.");
                return RedirectToAction("Detail", "Task", new { id = id });
            }
            var projectTask = await _db.projectTask.FindAsync(id);
            if (projectTask.UId == null)
            {
                TempData["errorTkMessage"] = "User not assigned.";
                return RedirectToAction("Detail", "Task", new { id = id });
            }
            projectTask.UId = null;
            await _db.SaveChangesAsync();
            return RedirectToAction("Detail", "Task", new { id = id });

        }
    }
}