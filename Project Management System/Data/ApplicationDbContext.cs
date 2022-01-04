using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Project_Management_System.Models;
using Microsoft.AspNetCore.Identity;

namespace Project_Management_System.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            //Rename the default table names
            modelBuilder.Entity<IdentityUser<string>>().ToTable("Users");
            modelBuilder.Entity<IdentityRole<string>>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            //Name the new tables; define keys and relations
            modelBuilder.Entity<ApplicationPrivilege>().ToTable("Privileges").HasKey(p => p.Id);
            modelBuilder.Entity<ApplicationRole>().HasMany((ApplicationRole e) => e.RolePrivileges);
            modelBuilder.Entity<ApplicationRolePrivilege>().ToTable("RolePrivileges").HasKey(e => new { e.RoleId, e.PrivilegeId });
            //Changing delete behavior
            modelBuilder.Entity<StageTasks>()
            .HasOne(e => e.ProjectTask)
            .WithOne(e => e.StageTasks)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApplicationRolePrivilege>()
            .HasOne(e => e.Privilege)
            .WithMany(e => e.RolePrivileges)
            .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<ApplicationRolePrivilege>()
            .HasOne(e => e.Role)
            .WithMany(e => e.RolePrivileges)
            .OnDelete(DeleteBehavior.ClientCascade);
        }

        public DbSet<ApplicationUser> applicationUser { get; set; }

        public DbSet<ApplicationPrivilege> applicationPrivilege { get; set; }

        public DbSet<ApplicationRole> applicationRole { get; set; }

        public DbSet<ApplicationUserRole> applicationUserRole { get; set; }

        public DbSet<ApplicationRolePrivilege> applicationRolePrivilege { get; set; }

        public DbSet<Project> project { get; set; }

        public DbSet<ProjectRole> projectRole { get; set; }

        public DbSet<ProjectMembersRole> projectMembersRoles { get; set; }

        public DbSet<ProjectProduct> projectProduct { get; set; }

        public DbSet<ProjectTeam> projectTeam { get; set; }

        public DbSet<TeamMembers> teamMember { get; set; }

        public DbSet<ProjectStage> projectStage { get; set; }

        public DbSet<StageTasks> stageTasks { get; set; }

        public DbSet<ProjectTask> projectTask { get; set; }

        public DbSet<TaskAttachment> taskAttachment { get; set; }

        public DbSet<TaskComment> taskComment { get; set; }
    }

}