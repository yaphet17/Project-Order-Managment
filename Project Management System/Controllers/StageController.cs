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
    public class StageController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _iwebhost;

        public StageController(ApplicationDbContext db, IWebHostEnvironment iwebhost)
        {
            _db = db;
            _iwebhost = iwebhost;
        }
        [HttpGet]
        [Route("project/stage/create")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Create(int pid)
        {
            var project = await _db.project.FindAsync(pid);
            if (project == null)
            {
                return NotFound();
            }
            var model = new ProjectStage
            {
                PId = pid
            };
            return View(model);
        }

        [HttpPost]
        [Route("project/stage/create")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Create(ProjectStage model)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Some error occured.");
                return View(model);
            }

            ProjectStage projectStage = new ProjectStage
            {
                PId = model.PId,
                StageName = model.StageName,
                StageWeight = model.StageWeight,
                Duration = 0//Default duration value :0
            };

            await _db.projectStage.AddAsync(projectStage);
            await _db.SaveChangesAsync();
            return RedirectToAction("AddTask", "Stage", new { id = projectStage.Id });
        }

        [HttpGet]
        [Route("project/stage/add-task")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> AddTask(int id)
        {

            var projectStage = await _db.projectStage.FindAsync(id);
            if (projectStage == null)
            {
                return NotFound();
            }
            List<AddStageTasksViewModel> model = new List<AddStageTasksViewModel>();
            var projectTasks = await _db.projectTask.Where(e => e.PId == projectStage.PId).ToListAsync();
            if (projectTasks.Count() == 0)
            {
                TempData["errorTkMessage"] = "No task found.Please create task first.";
                return RedirectToAction("Create", "Stage");
            }
            foreach (var task in projectTasks)
            {
                var tempModel = new AddStageTasksViewModel
                {
                    SId = id,
                    TId = task.Id,
                    TaskName = task.Name,
                    StartDate = task.StartDate,
                    EndDate = task.EndDate,
                    CheckboxAnswer = false
                };
                model.Add(tempModel);
            }
            return View(model);
        }

        

        [Route("project/stage/detail")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Detail(int id)
        {

            var projectStage = await _db.projectStage.FindAsync(id);
            if (projectStage == null)
            {
                return NotFound();
            }
            var stageTasks = await _db.stageTasks.Where(e => e.SId == projectStage.Id).ToListAsync();
            List<StageDetailInfoViewModel> tasks = new List<StageDetailInfoViewModel>();
            foreach (var task in stageTasks)
            {

                var tempTask = await _db.projectTask.FindAsync(task.TId);
                var sdInfo = new StageDetailInfoViewModel
                {
                    Id = task.Id,
                    TId = tempTask.Id,
                    TaskName = tempTask.Name,
                    StartDate = tempTask.StartDate,
                    EndDate = tempTask.EndDate
                };
                tasks.Add(sdInfo);
            }
            var model = new StageDetailViewModel
            {
                Id = id,
                StageName = projectStage.StageName,
                Duration = projectStage.Duration,
                StageTasks = tasks
            };

            return View(model);
        }
        [HttpGet]
        [Route("project/stage/edit")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _db.projectStage.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        [Route("project/stage/edit")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Edit(ProjectStage model)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Some error occured.");
                return View(model);
            }
            var newModel = await _db.projectStage.FindAsync(model.Id);
            newModel.StageName = model.StageName;
            newModel.StageWeight = model.StageWeight;
            await _db.SaveChangesAsync();
            return RedirectToAction("Detail", "Stage", new { id = model.Id });
        }

        [Route("project/stage/delete")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Some error occured");
            }
            var projectStage = await _db.projectStage.FindAsync(id);
            if (projectStage == null)
            {
                return NotFound();
            }

            var stageTasks = await _db.stageTasks.Where(e => e.SId == id).ToListAsync();
            _db.stageTasks.RemoveRange(stageTasks);
            await _db.SaveChangesAsync();
            return RedirectToAction("Detail", "Stage", new { id = projectStage.PId });
        }

        [HttpPost]
        [Route("project/stage/add-task")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> AddTask(List<AddStageTasksViewModel> model)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Some error occurred.");
                return View(model);
            }

            List<DateTime> startDates = new List<DateTime>();
            List<DateTime> endDates = new List<DateTime>();

            for (int i = 0; i < model.Count(); i++)
            {
                if (model[i].CheckboxAnswer == true)
                {
                    var check = await _db.stageTasks.Where(e => e.TId == model[i].TId).ToListAsync();
                    if (check.Count() > 0)
                    {
                        TempData["errorStMessage"] = "Some tasks are already added to a stage.";
                        return RedirectToAction("Detail", "Stage", new { id = model[0].SId });
                    }
                    var tempModel = new StageTasks
                    {
                        SId = model[i].SId,
                        TId = model[i].TId,
                    };
                    startDates.Add(DateTime.Parse(model[i].StartDate));
                    endDates.Add(DateTime.Parse(model[i].EndDate));
                    await _db.stageTasks.AddAsync(tempModel);
                }
            }

            var stage = await _db.projectStage.FindAsync(model[0].SId);
            double duration = calculateDuration(stage.Duration, startDates, endDates);
            stage.Duration = (int)duration;
            await _db.SaveChangesAsync();
            TempData["stMessage"] = "Tasks successfully added.";
            return RedirectToAction("Detail", "Stage", new { id = model[0].SId });
        }

        [Route("project/stage/remove-task")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> RemoveTask(int id)
        {

            var stageTask = await _db.stageTasks.FindAsync(id);
            if (stageTask == null)
            {
                return NotFound();
            }
            _db.stageTasks.Remove(stageTask);
            await _db.SaveChangesAsync();
            TempData["stMessage"] = "Task successfully remove from the stage.";
            return RedirectToAction("Detail", "Stage", new { id = stageTask.SId });
        }

        public int calculateDuration(int duration, List<DateTime> startDates, List<DateTime> endDates)
        {

            int totalDuration = duration + (int)(endDates.Max() - startDates.Min()).TotalDays;

            return totalDuration;
        }


    }
}