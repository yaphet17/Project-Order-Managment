using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Project_Management_System.Models;
using Project_Management_System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;


namespace Project_Management_System.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _iwebhost;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment iwebhost)
        {
            _db = db;
            _iwebhost = iwebhost;
        }

        [HttpGet]
        [Route("project/product/create")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Create(int pid)
        {

            var project = await _db.project.FindAsync(pid);
            if (project == null)
            {
                return NotFound();
            }
            var model = new ProjectProduct
            {
                PId = pid
            };
            return View(model);
        }

        [HttpPost]
        [Route("project/product/create")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Create(ProjectProduct model)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Some error occured.");
                return View(model);
            }
            await _db.projectProduct.AddAsync(model);
            await _db.SaveChangesAsync();
            TempData["pdMessage"] = "Product successfully added.";
            return RedirectToAction("Detail", "Product", new { id = model.Id });
        }

        [HttpGet]
        [Route("project/product/edit")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Edit(int id)
        {

            var model = await _db.projectProduct.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        [Route("project/product/edit")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Edit(ProjectProduct model)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Some error occured.");
                return View(model);
            }
            var newModel = await _db.projectProduct.FindAsync(model.Id);
            newModel.ProductName = model.ProductName;
            newModel.PrescPlan = model.PrescPlan;
            newModel.PrescActual = model.PrescActual;
            newModel.DraftRPlan = model.DraftRPlan;
            newModel.DraftAPlan = model.DraftAPlan;
            newModel.Fqcca = model.Fqcca;
            newModel.ApprovedPlan = model.ApprovedPlan;
            newModel.ApprovedActual = model.ApprovedActual;
            newModel.HandleOPlan = model.HandleOPlan;
            newModel.HandleAPlan = model.HandleAPlan;
            await _db.SaveChangesAsync();
            ViewBag.message = "Product successfully updated.";
            return View(newModel);
        }

        [Route("project/product/delete")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Delete(int id)
        {

            var projectProduct = await _db.projectProduct.FindAsync(id);
            if (projectProduct == null)
            {
                return NotFound();
            }
            _db.projectProduct.Remove(projectProduct);
            await _db.SaveChangesAsync();
            TempData["pdMessage"] = "Product successfully deleted.";
            return RedirectToAction("Detail", "Product", new { pid = projectProduct.PId });
        }

        [Route("project/product/detail")]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> Detail(int id)
        {

            var model = await _db.projectProduct.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }
    }
}