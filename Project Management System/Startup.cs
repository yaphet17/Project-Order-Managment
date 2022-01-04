using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Authorization;
using Project_Management_System.Data;
using Project_Management_System.Models;
using Project_Management_System.Abstractions;
using Project_Management_System.Utility;


namespace Project_Management_System
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();  
            services.AddControllersWithViews();
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
           {
               options.User.RequireUniqueEmail = true;
               options.Password.RequiredLength = 8;
               options.Password.RequireNonAlphanumeric = true;
               options.Password.RequireDigit = true;
               options.Password.RequireLowercase = true;
               options.Password.RequireUppercase = true;

               options.Lockout.AllowedForNewUsers = true;
               options.Lockout.MaxFailedAccessAttempts = 3;

           }).AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbInitializer dbIntializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            // SeedData.Initial
            dbIntializer.Initialize();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
