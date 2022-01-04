using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Project_Management_System.Models;
using Project_Management_System.ViewModels.Identity;

namespace Project_Management_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("")]
        [Route("home")]
        [Route("home/index")]
        public IActionResult Index()
        {
            LogInViewModel model = new LogInViewModel();
            return View(model);
        }

        [HttpPost]
        [Route("")]
        [Route("home")]
        [Route("home/index/{returnUrl?}")]
        public async Task<IActionResult> Index(LogInViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (await _userManager.CheckPasswordAsync(user, model.Password) == false)
                {
                    ModelState.AddModelError("", "Invalid credentials");
                    return View(model);
                }
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Account");

                }
                ModelState.AddModelError("", "Invalid Login Attempt");
            }
            return View(model);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
