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
    public class TeamController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _iwebhost;

        public TeamController(ApplicationDbContext db, IWebHostEnvironment iwebhost)
        {
            _db = db;
            _iwebhost = iwebhost;
        }

        [HttpGet]
        [Route("project/team/create")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Create(int pid)
        {
            var project = await _db.project.FindAsync(pid);
            if (project == null)
            {
                return NotFound();
            }
            var model = new ProjectTeam
            {
                PId = pid
            };

            return View(model);
        }

        [HttpPost]
        [Route("project/team/create")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Create(ProjectTeam model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Some error occured.");
            }
            await _db.projectTeam.AddAsync(model);
            await _db.SaveChangesAsync();
            var newModel = new ProjectTeam
            {
                PId = model.PId
            };
            TempData["tmMessage"] = "Team created successfully.";
            return RedirectToAction("AddMember", "Team", new { id = model.Id });
        }

       

        [Route("project/team/detail")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Detail(int id)
        {

            var projectTeam = await _db.projectTeam.FindAsync(id);
            if (projectTeam == null)
            {
                return NotFound();
            }
            var project = await _db.project.FindAsync(projectTeam.PId);
            List<TeamMembers> teamMembers = await _db.teamMember.Where(e => e.TmId == id).ToListAsync();
            List<TeamMemberInfoViewModel> teamMembersInfo = new List<TeamMemberInfoViewModel>();

            foreach (var item in teamMembers)
            {
                var user = await _db.applicationUser.FindAsync(item.UId);
                TeamMemberInfoViewModel _model = new TeamMemberInfoViewModel
                {
                    UId = item.UId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    TeamRole = item.TeamRole
                };
                teamMembersInfo.Add(_model);
            }
            var model = new TeamDetailViewModel
            {
                TmId = id,
                TeamName = projectTeam.TeamName,
                PId = projectTeam.PId,
                ProjectName = project.ProjectName,
                TeamMembersInfo = teamMembersInfo
            };
            return View(model);
        }

        [HttpGet]
        [Route("project/team/edit")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _db.projectTeam.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        [Route("project/team/edit")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Edit(ProjectTeam model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Some error occurred.");
                return View(model);
            }
            var newTeam = await _db.projectTeam.FindAsync(model.Id);
            newTeam.Id = model.Id;
            newTeam.TeamName = model.TeamName;
            await _db.SaveChangesAsync();
            return RedirectToAction("Detail", "Team", new { id = model.Id });
        }

        [Route("project/team/delete")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Delete(int id)
        {

            var projectTeam = await _db.projectTeam.FindAsync(id);
            if (projectTeam == null)
            {
                return NotFound();
            }
            var teamMembers = await _db.teamMember.Where(e => e.TmId == id).ToListAsync();
            var projectTasks = await _db.projectTask.Where(e => e.PId == projectTeam.PId).ToListAsync();
            foreach (var task in projectTasks)
            {
                for (int i = 0; i < teamMembers.Count(); i++)
                {
                    if (task.UId != null)
                    {
                        if (teamMembers[i].UId.Contains(task.UId))
                        {
                            TempData["errorTmMessage"] = "Team members are assgined to task.Team can't be deleted.";
                            return RedirectToAction("Detail", "Team", new { pid = projectTeam.PId });
                        }
                    }

                }
            }
            _db.projectTeam.Remove(projectTeam);
            await _db.SaveChangesAsync();
            return RedirectToAction("Detail", "Team", new { pid = projectTeam.PId });
        }

        [HttpGet]
        [Route("project/team/add-member")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> AddMember(int id)
        {
            var team = await _db.projectTeam.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            List<ApplicationUser> users = await _db.applicationUser.ToListAsync();
            List<AddTeamMemberViewModel> model = new List<AddTeamMemberViewModel>();
            foreach (var user in users)
            {
                var _model = new AddTeamMemberViewModel
                {
                    TmId = id,
                    UId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    CheckboxAnswer = false
                };
                model.Add(_model);
            }
            return View(model);
        }


        [HttpPost]
        [Route("project/team/add-member")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> AddMember(List<AddTeamMemberViewModel> model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Some error occurred.");
                return View(model);
            }

            for (int i = 0; i < model.Count(); i++)
            {
                if (model[i].CheckboxAnswer == true)
                {
                    //Check if user is already in the team
                    var checkUser = await _db.teamMember.Where(e => e.TmId == model[i].TmId && e.UId == model[i].UId).ToListAsync();
                    if (checkUser.Count() > 0)
                    {
                        var user = await _db.applicationUser.FindAsync(model[i].UId);
                        TempData["errorTmMessage"] = $"User {user.FirstName + " " + user.LastName} is aleardy in the team.";
                        continue;
                    }
                    var teamMember = new TeamMembers
                    {
                        TmId = model[i].TmId,
                        UId = model[i].UId,
                        TeamRole = "TeamMember"//Default role of a user in a team
                    };
                    await _db.teamMember.AddAsync(teamMember);
                }
            }
            await _db.SaveChangesAsync();
            TempData["tmMessage"] = "Team members added successfully";
            return RedirectToAction("Detail", "Team", new { id = model[0].TmId });
        }

        [HttpGet]
        [Route("project/team/change-member-role")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> ChangeMemberRole(int id, string uid)
        {
            var projectTeam = await _db.projectTeam.FindAsync(id);
            var member = await _db.applicationUser.FindAsync(uid);
            var teamRole = await _db.teamMember.Where(e => e.TmId == id && e.UId == uid).Select(e => e.TeamRole).ToListAsync();
            if (projectTeam == null || member == null)
            {
                return NotFound();
            }
            var model = new ChangeMemberRoleViewModel
            {
                TmId = id,
                UId = uid,
                FirstName = member.FirstName,
                LastName = member.LastName,
                TeamRole = teamRole[0]
            };
            return View(model);

        }
        
        [Route("project/team/remove-member")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> RemoveMember(int tmid, string uid, int pid)
        {
            var user = await _db.applicationUser.FindAsync(uid);
            var project = await _db.project.FindAsync(pid);
            if (user == null || project == null)
            {
                return NotFound();
            }
            var projectTask = await _db.projectTask.Where(e => e.UId == uid).ToListAsync();
            if (projectTask.Count() != 0)
            {
                TempData["errorTmMessage"] = "Task assigned users can't be deleted.";
                return Redirect(Request.Headers["Referer"].ToString());
            }

            List<TeamMembers> member = await _db.teamMember.Where(e => e.TmId == tmid && e.UId == uid).ToListAsync();
            if (member.Count() > 0)
            {
                _db.teamMember.Remove(member[0]);
                await _db.SaveChangesAsync();
                TempData["tmMessage"] = "Member successfully removed.";
                return Redirect(Request.Headers["Referer"].ToString());
            }
            return NotFound();
        }
        [HttpPost]
        [Route("project/team/change-member-role")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> ChangeMemberRole(ChangeMemberRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Some error occurred.");
                return View(model);
            }
            if (model.TeamRole == "ProjectManager")
            {
                var tempTeamMember = await _db.teamMember.Where(e => e.TmId == model.TmId && e.TeamRole == "ProjectManager").ToListAsync();
                if (tempTeamMember.Count() > 0)
                {
                    TempData["errorTmMessage"] = "Project manager already assigned for this project.";
                    return RedirectToAction("Detail", "Team", new { id = model.TmId });
                }
            }
            else if (model.TeamRole == "Executive")
            {
                var tempTeamMember = await _db.teamMember.Where(e => e.TmId == model.TmId && e.TeamRole == "Executive").ToListAsync();
                if (tempTeamMember.Count() > 0)
                {
                    TempData["errorTmMessage"] = "Executive already assigned for this project.";
                    return RedirectToAction("Detail", "Team", new { id = model.TmId });
                }
            }

            var teamMember = await _db.teamMember.Where(e => e.TmId == model.TmId && e.UId == model.UId).ToListAsync();
            teamMember[0].TeamRole = model.TeamRole;
            await _db.SaveChangesAsync();
            TempData["tmMessage"] = "Successfully updated.";
            return RedirectToAction("Detail", "Team", new { id = model.TmId });
        }


    }

}