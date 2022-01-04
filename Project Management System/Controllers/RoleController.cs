using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Project_Management_System.Data;
using Project_Management_System.Models;
using Project_Management_System.ViewModels.Identity;

namespace Project_Management_System.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {

        private readonly ApplicationDbContext _db;

        public RoleController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        [Route("role")]
        [Route("role/index")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Index()
        {

            IEnumerable<ApplicationRole> model = await _db.applicationRole.OrderBy(e => e.Name).ToListAsync();
            return View(model);

        }

        [HttpGet]
        [Route("role/create")]
        [ProjectOrderManagementAuthorizationFilter]
        public IActionResult Create()
        {
            var model = new ApplicationRole();
            return View(model);
        }

        [HttpPost]
        [Route("role/create")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Create(ApplicationRole model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Some error occurred.");
                return View(model);
            }
            if (IsRoleNameTaken(model.Name))
            {
                ViewBag.message = "Role already exists.";
                return View(model);
            }

            await _db.applicationRole.AddAsync(model);
            await _db.SaveChangesAsync();
            TempData["rMessage"] = "Role successfully created.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("role/detail")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Detail(string id)
        {

            var role = await _db.applicationRole.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            var roleprivileges = await _db.applicationRolePrivilege.Where(e => e.RoleId == role.Id).Select(e => e.PrivilegeId).ToListAsync();
            var allPrivilege = await _db.applicationPrivilege.ToListAsync();
            var privileges = new List<ApplicationPrivilege>();
            foreach (var privilege in allPrivilege)
            {
                if (roleprivileges.Contains(privilege.Id))
                {
                    privileges.Add(privilege);
                }
            }

            var model = new RoleDetailViewModel
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                Privileges = privileges

            };
            return View(model);

        }
        [HttpGet]
        [Route("role/edit")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Edit(string id)
        {

            var model = await _db.applicationRole.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        [Route("role/edit")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Edit(ApplicationRole model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Some error occurred.");
                return View(model);
            }
            var newModel = await _db.applicationRole.FindAsync(model.Id);
            if (newModel == null)
            {
                TempData["errorRMessage"] = "Role not found.";
                return View(model);
            }
            if (IsRoleNameTaken(model.Name) && newModel.Name != model.Name)
            {
                ViewBag.message = "Role already exists.";
                return View(model);
            }
            newModel.Name = model.Name;
            newModel.Description = model.Description;
            await _db.SaveChangesAsync();
            TempData["rMessage"] = "Role successfully updated.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("role/delete")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Delete(string id)
        {

            var model = await _db.applicationRole.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            _db.applicationRole.Remove(model);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("role/assign-privilege")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> AssignPrivilege(string id)
        {
            var role = await _db.applicationRole.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            var model = new List<AssignPrivilegeViewModel>();
            var privileges = await _db.applicationPrivilege.ToListAsync();
            foreach (var privilege in privileges)
            {
                var temp = new AssignPrivilegeViewModel
                {
                    RId = id,
                    PrId = privilege.Id,
                    Description = privilege.Description,
                    CheckboxAnswer = false
                };
                model.Add(temp);
            }
            return View(model);

        }

        [HttpPost]
        [Route("role/assign-privilege")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> AssignPrivilege(List<AssignPrivilegeViewModel> model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Some error occured.");
                return View(model);
            }

            foreach (var privilege in model)
            {
                if (privilege.CheckboxAnswer)
                {
                    var roleprivilege = new ApplicationRolePrivilege
                    {
                        RoleId = privilege.RId,
                        PrivilegeId = privilege.PrId
                    };
                    await _db.applicationRolePrivilege.AddAsync(roleprivilege);
                }

            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Detail", "Role", new { id = model[0].RId });


        }

        [HttpPost]
        [Route("role/remove-privilege")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> RemovePrivilege(string rid, string prid)
        {

            var privilege = await _db.applicationPrivilege.FindAsync(prid);
            var role = await _db.applicationRole.FindAsync(rid);
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Some error occured.");
                return View();
            }

            if (privilege == null || role == null)
            {
                return NotFound();
            }
            var model = await _db.applicationRolePrivilege.Where(e => e.RoleId == role.Id && e.PrivilegeId == privilege.Id).ToListAsync();
            if (model == null)
            {
                TempData["errorRMessage"] = "Can't remove privilege.";
                return RedirectToAction("Detail", "Role", new { id = rid });
            }
            _db.applicationRolePrivilege.Remove(model[0]);
            await _db.SaveChangesAsync();
            TempData["rMessage"] = "Privilege sucessfully removed.";
            return RedirectToAction("Detail", "Role", new { id = rid });
        }

        public bool IsRoleNameTaken(string roleName)
        {

            var role = _db.applicationRole.Where(e => e.Name == roleName).ToList();
            if (role.Count() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

    }

}