using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Project_Management_System.Data;
using Project_Management_System.Models;

namespace  Project_Management_System.Controllers
{
    public class PrivilegeController:Controller{

        private readonly ApplicationDbContext _db;

        public PrivilegeController(ApplicationDbContext db){
            _db=db;
        }
        [Route("privilege")]
        [Route("privilege/index")]
        public async Task<IActionResult> Index(){
            List<ApplicationPrivilege> roles=await _db.applicationPrivilege.OrderBy(e=>e.Action).ThenBy(e=>e.Description).ToListAsync();
            return View(roles);

        }

        [HttpGet]
        [Route("privilege")]
        [Route("privilege/create")]
        public IActionResult Create(){
            var model= new ApplicationRolePrivilege();
            return View(model);
        }

        [HttpPost]
        [Route("privilege/create")]
        public async Task<IActionResult> Create(ApplicationPrivilege model){
            if(!ModelState.IsValid){
                ModelState.AddModelError("", "Some error occurred.");
                return View(model);
            }
            await _db.applicationPrivilege.AddAsync(model);
            await _db.SaveChangesAsync();
            TempData["priMessage"]="Privilege successfully created.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("privilege/detail")]
        public async Task<IActionResult> Detail(string id){

            var model=await _db.applicationPrivilege.FindAsync(id);
            if(model==null){
                return NotFound();
            }
            return View(model);

        }
        [HttpGet]
        [Route("priviledge/edit")]
        public async Task<IActionResult> Edit(string id){

            var model= await _db.applicationPrivilege.FindAsync(id);
            if(model==null){
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        [Route("privilege/edit")]
        public async Task<IActionResult> Edit(ApplicationPrivilege model){
            if(!ModelState.IsValid){
                ModelState.AddModelError("", "Some error occurred.");
                return View(model);
            }
            var newModel=await _db.applicationPrivilege.FindAsync(model.Id);
            newModel.Action=model.Action;
            newModel.Description=model.Description;
            await _db.SaveChangesAsync();
            TempData["priMessage"]="Privilege successfully updated.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("privilege/delete")]
        public async Task<IActionResult> Delete(string id){

            var model=await _db.applicationPrivilege.FindAsync(id);
            if(model==null){
                return NotFound();
            }
            _db.applicationPrivilege.Remove(model);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [HttpPost]
        [Route("privilege/is-privilege-exists")]
        public async Task<IActionResult> IsPrivilegeExists(string privilegeAction)
        {
            var privilege = await _db.applicationPrivilege.Where(e => e.Action == privilegeAction).ToListAsync();
            if (privilege.Count() == 0)
            {
                return Json(true);
            }
            else
            {
                return Json($"Privilege already exists.");
            }

        }

    }
    
}