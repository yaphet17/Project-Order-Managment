using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Project_Management_System.Migrations
{
    public partial class IntialSetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Privileges",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Privileges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "project",
                columns: table => new
                {
                    PId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndDate = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project", x => x.PId);
                });

            migrationBuilder.CreateTable(
                name: "projectRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projectRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_AspNetRoles_RoleId1",
                        column: x => x.RoleId1,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePrivileges",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PrivilegeId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePrivileges", x => new { x.RoleId, x.PrivilegeId });
                    table.ForeignKey(
                        name: "FK_RolePrivileges_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePrivileges_Privileges_PrivilegeId",
                        column: x => x.PrivilegeId,
                        principalTable: "Privileges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "projectProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PId = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrescPlan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrescActual = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DraftRPlan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DraftAPlan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fqccp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fqcca = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovedPlan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovedActual = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HandleOPlan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HandleAPlan = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projectProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_projectProduct_project_PId",
                        column: x => x.PId,
                        principalTable: "project",
                        principalColumn: "PId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "projectStage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PId = table.Column<int>(type: "int", nullable: false),
                    StageName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StageWeight = table.Column<double>(type: "float", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projectStage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_projectStage_project_PId",
                        column: x => x.PId,
                        principalTable: "project",
                        principalColumn: "PId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "projectTask",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StartDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projectTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_projectTask_AspNetUsers_UId",
                        column: x => x.UId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_projectTask_project_PId",
                        column: x => x.PId,
                        principalTable: "project",
                        principalColumn: "PId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "projectTeam",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PId = table.Column<int>(type: "int", nullable: false),
                    TeamName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projectTeam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_projectTeam_project_PId",
                        column: x => x.PId,
                        principalTable: "project",
                        principalColumn: "PId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "projectMembersRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PId = table.Column<int>(type: "int", nullable: false),
                    RId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projectMembersRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_projectMembersRoles_AspNetUsers_UId",
                        column: x => x.UId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_projectMembersRoles_project_PId",
                        column: x => x.PId,
                        principalTable: "project",
                        principalColumn: "PId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_projectMembersRoles_projectRole_RId",
                        column: x => x.RId,
                        principalTable: "projectRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "stageTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SId = table.Column<int>(type: "int", nullable: false),
                    TId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stageTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_stageTasks_projectStage_SId",
                        column: x => x.SId,
                        principalTable: "projectStage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_stageTasks_projectTask_TId",
                        column: x => x.TId,
                        principalTable: "projectTask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "taskAttachment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TId = table.Column<int>(type: "int", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_taskAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_taskAttachment_projectTask_TId",
                        column: x => x.TId,
                        principalTable: "projectTask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "taskComment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommentDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_taskComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_taskComment_AspNetUsers_UId",
                        column: x => x.UId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_taskComment_projectTask_TId",
                        column: x => x.TId,
                        principalTable: "projectTask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "teamMember",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TmId = table.Column<int>(type: "int", nullable: false),
                    UId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TeamRole = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teamMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_teamMember_AspNetUsers_UId",
                        column: x => x.UId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_teamMember_projectTeam_TmId",
                        column: x => x.TmId,
                        principalTable: "projectTeam",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_projectMembersRoles_PId",
                table: "projectMembersRoles",
                column: "PId");

            migrationBuilder.CreateIndex(
                name: "IX_projectMembersRoles_RId",
                table: "projectMembersRoles",
                column: "RId");

            migrationBuilder.CreateIndex(
                name: "IX_projectMembersRoles_UId",
                table: "projectMembersRoles",
                column: "UId");

            migrationBuilder.CreateIndex(
                name: "IX_projectProduct_PId",
                table: "projectProduct",
                column: "PId");

            migrationBuilder.CreateIndex(
                name: "IX_projectStage_PId",
                table: "projectStage",
                column: "PId");

            migrationBuilder.CreateIndex(
                name: "IX_projectTask_PId",
                table: "projectTask",
                column: "PId");

            migrationBuilder.CreateIndex(
                name: "IX_projectTask_UId",
                table: "projectTask",
                column: "UId");

            migrationBuilder.CreateIndex(
                name: "IX_projectTeam_PId",
                table: "projectTeam",
                column: "PId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePrivileges_PrivilegeId",
                table: "RolePrivileges",
                column: "PrivilegeId");

            migrationBuilder.CreateIndex(
                name: "IX_stageTasks_SId",
                table: "stageTasks",
                column: "SId");

            migrationBuilder.CreateIndex(
                name: "IX_stageTasks_TId",
                table: "stageTasks",
                column: "TId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_taskAttachment_TId",
                table: "taskAttachment",
                column: "TId");

            migrationBuilder.CreateIndex(
                name: "IX_taskComment_TId",
                table: "taskComment",
                column: "TId");

            migrationBuilder.CreateIndex(
                name: "IX_taskComment_UId",
                table: "taskComment",
                column: "UId");

            migrationBuilder.CreateIndex(
                name: "IX_teamMember_TmId",
                table: "teamMember",
                column: "TmId");

            migrationBuilder.CreateIndex(
                name: "IX_teamMember_UId",
                table: "teamMember",
                column: "UId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_ApplicationUserId",
                table: "UserRoles",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId1",
                table: "UserRoles",
                column: "RoleId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "projectMembersRoles");

            migrationBuilder.DropTable(
                name: "projectProduct");

            migrationBuilder.DropTable(
                name: "RolePrivileges");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "stageTasks");

            migrationBuilder.DropTable(
                name: "taskAttachment");

            migrationBuilder.DropTable(
                name: "taskComment");

            migrationBuilder.DropTable(
                name: "teamMember");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "projectRole");

            migrationBuilder.DropTable(
                name: "Privileges");

            migrationBuilder.DropTable(
                name: "projectStage");

            migrationBuilder.DropTable(
                name: "projectTask");

            migrationBuilder.DropTable(
                name: "projectTeam");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "project");
        }
    }
}
