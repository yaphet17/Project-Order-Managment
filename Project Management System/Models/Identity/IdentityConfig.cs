using Project_Management_System.Data;
using Project_Management_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Management_System.Models
{
    /// <summary>
    /// System user to be authorized
    /// </summary>
    public class ProjectOrderManagementUser
    {
        ApplicationDbContext applicationDbContext;
        private readonly IServiceProvider serviceProvider;
        public string Username { get; set; }

        private List<ApplicationUserRole> Roles = new List<ApplicationUserRole>();

        public ProjectOrderManagementUser(IServiceProvider _serviceProvider)
        {
            serviceProvider = _serviceProvider;
        }

        public ProjectOrderManagementUser(string _username, IServiceProvider serviceProvider)
        {
            var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();

            applicationDbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
            applicationDbContext.Database.EnsureCreated();

            Username = _username;
            GetUserRolesPrivileges();
        }

        /// <summary>
        /// Gets user privileges using its role
        /// </summary>
        private void GetUserRolesPrivileges()
        {
            //get user
            ApplicationUser user = applicationDbContext.Users.Include(ur => ur.UserRoles).Where(u => u.UserName == this.Username).FirstOrDefault();

            if (user != null)
            {
                var userRoles = applicationDbContext.UserRoles.Where(ur => ur.UserId == user.Id).ToList();

                foreach (var role in userRoles)
                {
                    this.Roles.Add(new ApplicationUserRole { RoleId = role.RoleId });
                }
            }
        }

        /// <summary>
        /// Checks whether a user has a given privilege
        /// </summary>
        /// <param name="requiredPrivilege">Privilege to be checked</param>
        /// <returns></returns>

        public bool HasPrivilege(string requiredPrivilege)
        {
            bool found = false;
            List<ApplicationRolePrivilege> rolePrivilegelist = applicationDbContext.applicationRolePrivilege.ToList();
            var privileges = applicationDbContext.applicationPrivilege.ToList();

            foreach (ApplicationUserRole userRole in this.Roles)
            {
                List<ApplicationRolePrivilege> rolePrivilege = rolePrivilegelist.Where(r => r.RoleId == userRole.RoleId).ToList();
                foreach (var privilege in rolePrivilege)
                {
                    found = privileges.Where(p => p.Action == requiredPrivilege && privilege.PrivilegeId == p.Id).ToList().Count > 0;
                    if (found)
                        break;
                }
                if (found)
                    break;
            }
            return found;
        }
    }

    public class ProjectOrderManagementAuthorizationFilter : Attribute, IAsyncAuthorizationFilter
    {

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            await Task.Delay(1);

            if (context != null && context?.ActionDescriptor is ControllerActionDescriptor descriptor)
            {
                //var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                string requiredPrivilege = String.Format("{0}-{1}", descriptor.ControllerName, descriptor.ActionName);

                ProjectOrderManagementUser requestingUser = new ProjectOrderManagementUser(context.HttpContext.User.Identity.Name, context.HttpContext.RequestServices);

                if (!requestingUser.HasPrivilege(requiredPrivilege))
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary {
                                                { "action", "Unauthorized" },
                                                { "controller", "Account" } });
                }

            }


        }
    }



}
