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
using Project_Management_System.Data;
using Project_Management_System.Models;
using Project_Management_System.ViewModels.Identity;

namespace Project_Management_System.Controllers
{

    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AccountController(ApplicationDbContext db, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
        {
            _db = db;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [Route("account")]
        [Route("account/index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("account/users-list")]
        [Authorize]
        [ProjectOrderManagementAuthorizationFilter]
        public async Task<IActionResult> UsersList()
        {
            var users = await _userManager.Users.OrderBy(e => e.FirstName).ThenBy(e => e.LastName).ToListAsync();
            var usersList = new List<UsersListViewModel>();
            foreach (ApplicationUser user in users)
            {
                var tempList = new UsersListViewModel();
                tempList.Id = user.Id;
                tempList.FirstName = user.FirstName;
                tempList.LastName = user.LastName;
                tempList.Email = user.Email;
                tempList.PhoneNumber = user.PhoneNumber;
                tempList.Roles = await _userManager.GetRolesAsync(user);
                usersList.Add(tempList);
            }
            return View(usersList);
        }


        [HttpGet]
        [Route("account/register-user")]
        [Authorize]
        [ProjectOrderManagementAuthorizationFilter]
        public IActionResult Register()
        {
            RegisterViewModel model = new RegisterViewModel();
            return View(model);
        }


        [HttpPost]
        [Authorize]
        [Route("account/register-user")]
        public async Task<IActionResult> Register(RegisterViewModel request)
        {
            if (ModelState.IsValid)
            {
                var userCheck = await _userManager.FindByEmailAsync(request.Email);

                if (userCheck == null)
                {
                    var user = new ApplicationUser
                    {
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        UserName = request.Email,
                        NormalizedUserName = request.Email,
                        Email = request.Email,
                        PhoneNumber = request.PhoneNumber,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                    };
                    var result = await _userManager.CreateAsync(user, request.Password);
                    if (result.Succeeded)
                    {
                        //Set the default role to User when user os registerd
                        await _userManager.AddToRoleAsync(user, "User");
                        ViewBag.message = "Sucessfully registered";
                        return RedirectToAction("UsersList");
                    }
                    else
                    {
                        if (result.Errors.Count() > 0)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                        return View(request);
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Email already exists.");
                    return View(request);
                }
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        [Authorize]
        [Route("account/edit-user-info/{id}")]
        public async Task<IActionResult> EditUserInfo(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();

            }
            var roles = await _db.applicationRole.Select(e => new ApplicationRole { Id = e.Id, Name = e.Name }).ToListAsync();
            var role = await _userManager.GetRolesAsync(user);
            var model = new EditUserInfoViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = role,
                Roles = roles
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [Route("account/edit-user-info/{id}")]
        public async Task<IActionResult> EditUserInfo(EditUserInfoViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            var role = await _db.applicationRole.FindAsync(model.Role.ElementAt(0));
            if (user == null || role == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.UserName = model.Email;
                user.NormalizedUserName = model.Email;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;

                var result = await _userManager.UpdateAsync(user);
                var userRoles = await _userManager.GetRolesAsync(user);
                if (result.Succeeded)
                {
                    if (!userRoles.SequenceEqual(model.Role))
                    {
                        if (userRoles.Count() > 0)
                        {
                            await _userManager.RemoveFromRoleAsync(user, userRoles[0]);
                        }
                        result = await _userManager.AddToRoleAsync(user, role.Name);
                        if (!result.Succeeded)
                        {
                            if (result.Errors.Count() > 0)
                            {
                                foreach (var error in result.Errors)
                                {
                                    ModelState.AddModelError("", error.Description);
                                }
                            }
                            return View(model);
                        }

                    }
                    TempData["rMessage"] = "Sucessfully updated";
                    return RedirectToAction("UsersList");
                }
                else
                {
                    if (result.Errors.Count() > 0)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                    return View(model);
                }

            }
            return RedirectToAction("Index");
        }

        [Route("account/delete-user/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                ViewBag.message = $"User {user.FirstName + " " + user.LastName} successfuly deleted.";
                return RedirectToAction("UsersList");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View("UsersList");

        }

        [HttpGet]
        [Route("account/reset-password")]
        [Authorize]
        public async Task<IActionResult> ResetPassword(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var model = new ResetPasswordViewModel
            {
                Id = user.Id
            };
            return View(model);
        }

        [HttpPost]
        [Route("account/reset-password")]
        [Authorize]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
            if (result.Succeeded)
            {
                ViewBag.message = "Password successfuly resetted.";
                return RedirectToAction("EditUserInfo", new { id = model.Id });
            }
            if (result.Errors.Count() > 0)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }


        [HttpGet]
        [Route("account/profile")]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            //Force the use to login again if info is missing
            if (user == null)
            {
                return Challenge();
            }
            var model = new EditProfileViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
            return View(model);
        }


        [HttpGet]
        [Route("account/edit-profile")]
        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new EditProfileViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
            return View(model);
        }

        [HttpPost]
        [Route("account/edit-profile")]
        [Authorize]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "");
                return View(model);
            }
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }
            user.Id = model.Id;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                ViewBag.message = "Successfuly Updated.";
                return RedirectToAction("Profile");
            }
            if (result.Errors.Count() > 0)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);


        }


        [HttpGet]
        [Route("account/change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new ChangePasswordViewModel
            {
                Id = user.Id
            };
            return View(model);
        }

        [HttpPost]
        [Route("account/change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "");
                return View(model);
            }
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }
            var result = await _userManager.CheckPasswordAsync(user, model.OldPassword);
            if (result == false)
            {
                ModelState.AddModelError("", "Invalid Password.");
                return View(model);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetResult = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
            if (resetResult.Succeeded)
            {
                ViewBag.message = "Password successfuly changed.";
                return RedirectToAction("EditProfile", new { id = model.Id });
            }
            if (resetResult.Errors.Count() > 0)
            {
                foreach (var error in resetResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);

        }
        [HttpGet]
        [HttpPost]
        [Route("account/is-email-in-use")]
        [Authorize]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in use.");
            }
        }

        [Route("account/logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


    }
}
