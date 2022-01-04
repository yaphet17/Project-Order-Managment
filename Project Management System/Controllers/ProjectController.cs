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
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _iwebhost;

        public ProjectController(ApplicationDbContext db, IWebHostEnvironment iwebhost)
        {
            _db = db;
            _iwebhost = iwebhost;
        }

        [Route("project")]
        [Route("project/index")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Project> projects = await _db.project.ToListAsync();
            return View(projects);
        }

        [HttpGet]
        [Route("project/create")]
        [ProjectOrderManagementAuthorizationFilter]
        public IActionResult Create()
        {
            var model = new Project();
            return View(model);
        }


        [HttpPost]
        [Route("project/create")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Create(Project model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Some error occurred.");
                return View(model);
            }
            await _db.project.AddAsync(model);
            await _db.SaveChangesAsync();
            return RedirectToAction("Detail", "Project", new { pid = model.PId });
        }

        [Route("project/detail")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Detail(int pid)
        {
            var project = await _db.project.FindAsync(pid);
            if (project == null)
            {
                return NotFound();
            }
            //Get project manager name
            var projectManagerRole = await _db.projectMembersRoles.Where(e => e.PId == pid && e.RId == 1).ToListAsync();
            string projectManagerName;
            if (projectManagerRole.Count() > 0)
            {
                var projectManager = await _db.applicationUser.FindAsync(projectManagerRole[0].UId);
                projectManagerName = projectManager.FirstName + " " + projectManager.LastName;
            }
            else
            {
                projectManagerName = "Not assigned";
            }
            //Get project executive name
            var executiveRole = await _db.projectMembersRoles.Where(e => e.PId == pid && e.RId == 2).ToListAsync();
            string executiveName;
            if (executiveRole.Count() > 0)
            {
                var executive = await _db.applicationUser.FindAsync(executiveRole[0].UId);
                executiveName = executive.FirstName + " " + executive.LastName;
            }
            else
            {
                executiveName = "Not assigned";
            }

            Dictionary<Dictionary<int, string>, List<ProjectTask>> stageTaskDict = new Dictionary<Dictionary<int, string>, List<ProjectTask>>();
            var projectStages = await _db.projectStage.Where(e => e.PId == pid).ToListAsync();
            var projectTasks = await _db.projectTask.Where(e => e.PId == project.PId).ToListAsync();
            List<ProjectTask> tempList;
            Dictionary<int, string> tempDict;
            foreach (var stage in projectStages)
            {
                var stageTasks = await _db.stageTasks.Where(e => e.SId == stage.Id).ToListAsync();
                tempList = new List<ProjectTask>();
                tempDict = new Dictionary<int, string>();
                foreach (var task in stageTasks)
                {
                    var _task = await _db.projectTask.FindAsync(task.TId);
                    tempList.Add(_task);
                }
                tempDict.Add(stage.Id, stage.StageName);
                stageTaskDict.Add(tempDict, tempList);
            }
            var projectTeams = await _db.projectTeam.Where(e => e.PId == project.PId).ToListAsync();
            var projectProducts = await _db.projectProduct.Where(e => e.PId == project.PId).ToListAsync();
            var model = new DetailViewModel
            {
                PId = project.PId,
                ProjectName = project.ProjectName,
                Description = project.Description,
                Status = project.Status,
                Priority = project.Priority,
                CreatedDate = project.CreatedDate,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                ProjectManager = projectManagerName,
                Executive = executiveName,
                StageTaskDict = stageTaskDict,
                ProjectProducts = projectProducts,
                ProjectTasks = projectTasks,
                ProjectTeams = projectTeams
            };
            return View(model);
        }

        [HttpGet]
        [Route("project/edit")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Edit(int pid)
        {
            var user = await _db.project.FindAsync(pid);
            if (user == null)
            {
                return NotFound();
            }
            var model = new Project
            {
                PId = user.PId,
                ProjectName = user.ProjectName,
                Description = user.Description,
                Status = user.Status,
                Priority = user.Priority,
                CreatedDate = user.CreatedDate,
                StartDate = user.StartDate,
            };
            return View(model);

        }
        [HttpPost]
        [Route("project/edit")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Edit(Project model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Some error occurred.");
                return View(model);
            }
            var newProject = await _db.project.FindAsync(model.PId);
            newProject.ProjectName = model.ProjectName;
            newProject.Description = model.Description;
            newProject.Priority = model.Priority;
            newProject.StartDate = model.StartDate;
            await _db.SaveChangesAsync();
            return RedirectToAction("Detail", "Project", new { pid = model.PId });
        }

        [Route("project/delete")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Delete(int pid)
        {
            var project = await _db.project.FindAsync(pid);
            if (project == null)
            {
                return NotFound();
            }
            if (!string.Equals(project.Status, "Initiated") && !string.Equals(project.Status, "Cancelled"))
            {
                TempData["errorPMessage"] = $"You can't delete {project.Status} projects.";
                return RedirectToAction("Index", "Project");
            }
            _db.project.Remove(project);
            await _db.SaveChangesAsync();
            TempData["pMessage"] = "Project successfully deleted.";
            return RedirectToAction("Index", "Project");
        }
        [HttpGet]
        [Route("project/change-status")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> ChangeStatus(int pid)
        {
            var project = await _db.project.FindAsync(pid);
            if (project == null)
            {
                return NotFound();
            }
            var model = new ChangeProjectStatusViewModel
            {
                PId = project.PId,
                ProjectName = project.ProjectName,
                Status = project.Status
            };
            return View(model);
        }

        [HttpPost]
        [Route("project/change-status")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> ChangeStatus(ChangeProjectStatusViewModel model)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Some error occurred.");
                return View(model);
            }
            var newProject = await _db.project.FindAsync(model.PId);
            //Prevent change of status of completed projects
            if (string.Equals(newProject.Status, "Completed"))
            {
                TempData["errorPMessage"] = "Status can't be changed..";
                return RedirectToAction("Detail", new { pid = model.PId });
            }

            newProject.Status = model.Status;
            await _db.SaveChangesAsync();
            TempData["pMessage"] = "Status successfully updated.";
            return RedirectToAction("Detail", "Project", new { pid = model.PId });
        }

        [HttpGet]
        [Route("project/delay")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Delay(int pid)
        {

            var project = await _db.project.FindAsync(pid);
            if (project == null)
            {
                return NotFound();
            }
            var model = new DelayViewModel
            {
                Id = project.PId,
                EndDate = project.EndDate
            };
            return View(model);
        }

        [HttpPost]
        [Route("project/delay")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Delay(DelayViewModel model)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Some error occurred.");
                return View(model);
            }
            var delayedProject = await _db.project.FindAsync(model.Id);
            DateTime oldEDate = DateTime.Parse(delayedProject.EndDate);
            DateTime newEDate = DateTime.Parse(model.EndDate);
            if (newEDate <= oldEDate)
            {
                ViewBag.message = "New due date must be greater than previous due date.";
                return View(model);
            }
            delayedProject.EndDate = model.EndDate;
            delayedProject.Status = "Delayed";
            await _db.SaveChangesAsync();
            TempData["pMessage"] = "Project successfully delayed.";
            return RedirectToAction("Detail", "Project", new { pid = model.Id });
        }

        [HttpGet]
        [Route("project/assign-role")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> AssignRole(int pid, int rid)
        {

            var project = await _db.project.FindAsync(pid);
            var role = await _db.projectRole.FindAsync(rid);
            if (project == null || role == null)
            {
                return NotFound();
            }

            if (rid == 1 || rid == 2)
            {
                //Check if project manager is already assgined for specified project
                var check = await _db.projectMembersRoles.Where(e => e.PId == pid && e.RId == rid).ToListAsync();
                if (check.Count() > 0)
                {
                    if (rid == 1)
                    {
                        TempData["errorPMessage"] = "Project manager already assigned for this project.";
                        return RedirectToAction("Detail", "Project", new { pid = pid });
                    }
                    else
                    {
                        TempData["errorPMessage"] = "Executive already assigned for this project.";
                        return RedirectToAction("Detail", "Project", new { pid = pid });
                    }
                }

            }
            var users = await _db.applicationUser.ToListAsync();
            var model = new List<AssignRoleViewModel>();
            foreach (var user in users)
            {
                var tempModel = new AssignRoleViewModel
                {
                    UId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PId = pid,
                    RId = rid,
                    CheckboxAnswer = false
                };
                model.Add(tempModel);
            }
            return View(model);
        }
        [HttpPost]
        [Route("project/assign-role")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> AssignRole(List<AssignRoleViewModel> model)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Some error occured.");
                return View(model);
            }

            bool isChangesMade = false;
            for (int i = 0; i < model.Count(); i++)
            {
                if (model[i].CheckboxAnswer == true)
                {
                    isChangesMade = true;
                    var newModel = new ProjectMembersRole
                    {
                        UId = model[i].UId,
                        PId = model[i].PId,
                        RId = model[i].RId
                    };
                    await _db.projectMembersRoles.AddAsync(newModel);
                    if (newModel.RId == 1 || newModel.RId == 2)
                    {
                        break;
                    }
                }
            }
            if (isChangesMade)
            {
                await _db.SaveChangesAsync();
                TempData["pMessage"] = "Users successfully assigned.";
            }
            else
            {
                TempData["errorPMessage"] = "No user selected.";
            }
            return RedirectToAction("Detail", "Project", new { pid = model[0].PId });
        }

        [Route("porject/remove-role")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> RemoveRole(int pid, int rid)
        {

            var projectRoles = await _db.projectMembersRoles.Where(e => e.PId == pid && e.RId == rid).ToListAsync();
            if (projectRoles.Count() == 0)
            {
                return NotFound();
            }
            _db.projectMembersRoles.Remove(projectRoles[0]);
            await _db.SaveChangesAsync();
            return RedirectToAction("Detail", "Project", new { pid = pid });
        }




        [HttpGet]
        [HttpPost]
        [Route("project/is-project-name-taken")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> IsProjectNameTaken(string projectName)
        {

            var project = await _db.project.Where(e => e.ProjectName == projectName).ToListAsync();
            if (project.Count() == 0)
            {
                return Json(true);
            }
            else
            {
                return Json($"Project name is already taken.");
            }

        }
        [HttpGet]
        [HttpPost]
        [Route("project/is-stage-name-taken")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> IsStageNameTaken(string stageName)
        {

            var stage = await _db.projectStage.Where(e => e.StageName == stageName).ToListAsync();
            if (stage.Count() == 0)
            {
                return Json(true);
            }
            else
            {
                return Json("Stage name is already taken.");
            }

        }



    }
}