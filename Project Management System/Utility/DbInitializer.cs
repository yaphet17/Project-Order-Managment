using Project_Management_System.Abstractions;
using Project_Management_System.Models;
using Project_Management_System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project_Management_System.Utility
{
    public class DbInitializer : IDbInitializer
    {
        private readonly IServiceProvider serviceProvider;
        public IConfiguration Configuration { get; }

        public DbInitializer(IServiceProvider _serviceProvider, IConfiguration _configuration)
        {
            serviceProvider = _serviceProvider;
            Configuration = _configuration;
        }

        //Seed, Creat Admin role and one Admin users, and assign privileges
        public async void Initialize()
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                //create database schema if none exists
                var applicationDbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                applicationDbContext.Database.EnsureCreated();

                if (!applicationDbContext.Users.Any())
                {
                    //Newly added
                    List<ApplicationPrivilege> privileges = new List<ApplicationPrivilege>()
                    {
                        //Account privileges
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Account-UsersList", Description="List users"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Account-Register", Description="Create users"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Account-EditUserInfo", Description="Edit user Information"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Account-Delete", Description="Delete user"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Account-ResetPassword", Description="Reset user Password"},
                       //Role privileges
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Role-Create", Description="Create role"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Role-Detail", Description="Role detail"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Role-Index", Description="List roles"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Role-Edit", Description="Edit role"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Role-Delete", Description="Delete role"},
                        //Privilege privileges
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Privilege-Create", Description="Create privilege"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Privilege-Edit", Description="Edit privilege"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Privilege-Delete", Description="Delete privilege"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Privilege-Index", Description="List rivileges"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Project-Index", Description="List projects"},
                        //Project privileges
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Project-Create", Description="Create project"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Project-Detail", Description="Project detail"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Project-Edit", Description="Edit project"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Project-Delete", Description="Delete project"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Project-ChangeStatus", Description="Change project status"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Project-Delay", Description="Delay project"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Project-AssignRole", Description="Assign project role"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Project-RemoveRole", Description="Remove project"},
                        //Task privileges
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Task-Create", Description="Create task"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Task-Detail", Description="Task detail"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Task-Edit", Description="Edit task"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Task-Delete", Description="Delete task"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Task-ChangeStatus", Description="Change task status"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Task-Delay", Description="Delay task"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Task-AddAttachment", Description="Add task attachment"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Task-AssignOwner", Description="Assign task owner"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Task-DeleteOwner", Description="Delete task owner"},
                        //Stage privileges
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Stage-Create", Description="Create stage"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Stage-Detail", Description="Stage detail"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Stage-Edit", Description="Edit stage"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Stage-Delete", Description="Delete stage"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Stage-AddTask", Description="Add task to stage"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Stage-RemoveTask", Description="Remove task from stage"},
                        //Team privileges
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Team-Create", Description="Create team"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Team-Detail", Description="Team detail"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Team-Edit", Description="Edit team"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Team-Delete", Description="Delete team"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Team-AddMember", Description="Add team member"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Team-RemoveMember", Description="Remove team member"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Team-ChangeMemberRole", Description="Change team member role"},
                        //Product  privileges
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Product-Create", Description="Create product"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Product-Detail", Description="Product detail"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Product-Edit", Description="Edit product"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Product-Delete", Description="Delete product"},
                     };

                    applicationDbContext.applicationPrivilege.AddRange(privileges);

                    //end Newly added

                    //If there is already an Administrator role, abort
                    var _roleManager = serviceScope.ServiceProvider.GetService<RoleManager<ApplicationRole>>();
                    var temp = Configuration.GetSection("UserSettings")["UserRole"];
                    var roleExists = await _roleManager.RoleExistsAsync(temp);
                    if (!roleExists)
                    {
                        //Create the Administartor Role
                        var model = new ApplicationRole
                        {
                            Name = Configuration.GetSection("UserSettings")["UserRole"],
                            Description = Configuration.GetSection("UserSettings")["Description"]
                        };
                        var result = await _roleManager.CreateAsync(model);

                        if (!result.Succeeded)
                            return;

                        var context = serviceProvider.GetService<ApplicationDbContext>();

                        string[] roles = new string[] { "Admin", "ProjectManager", "Executive", "SeniorSupplier", "SeniorUser", "User" };

                        foreach (string role in roles)
                        {
                            model = new ApplicationRole
                            {
                                Name = role,
                                Description = ""
                            };
                            result = await _roleManager.CreateAsync(model);
                            if (!result.Succeeded)
                                return;
                        }
                    }
                    foreach (ApplicationPrivilege ap in privileges)
                    {
                        applicationDbContext.applicationRolePrivilege.Add(
                            new ApplicationRolePrivilege
                            {
                                PrivilegeId = ap.Id,
                                RoleId = applicationDbContext.Roles.FirstOrDefault(r => r.Name == Configuration.GetSection("UserSettings")["UserRole"])?.Id
                            });
                    }

                    applicationDbContext.SaveChanges();

                    //Create the default Admin account and apply the Administrator role
                    var _userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

                    ApplicationUser user = new ApplicationUser
                    {
                        FirstName = Configuration.GetSection("UserSettings")["FirstName"],
                        LastName = Configuration.GetSection("UserSettings")["LastName"],
                        UserName = Configuration.GetSection("UserSettings")["UserEmail"],
                        Email = Configuration.GetSection("UserSettings")["UserEmail"]
                    };

                    var success = await _userManager.CreateAsync(user, Configuration.GetSection("UserSettings")["UserPassword"]);

                    if (success.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, Configuration.GetSection("UserSettings")["UserRole"]);
                    }

                }
            }
        }
    }

}
