using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternetSafetyPlan.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Device",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Nickname = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Device", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Device_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UltimateGoal",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UltimateGoal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UltimateGoal_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DeviceTag",
                columns: table => new
                {
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceTag", x => new { x.DeviceId, x.TagId });
                    table.ForeignKey(
                        name: "FK_DeviceTag_Device_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Device",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DeviceTag_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tag",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Goal",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UltimateGoalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Goal_UltimateGoal_UltimateGoalId",
                        column: x => x.UltimateGoalId,
                        principalTable: "UltimateGoal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teammate",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teammate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teammate_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Teammate_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ActionItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GoalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionItem_Goal_GoalId",
                        column: x => x.GoalId,
                        principalTable: "Goal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoalDevice",
                columns: table => new
                {
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GoalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoalDevice", x => new { x.DeviceId, x.GoalId });
                    table.ForeignKey(
                        name: "FK_GoalDevice_Device_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Device",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GoalDevice_Goal_GoalId",
                        column: x => x.GoalId,
                        principalTable: "Goal",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GoalTag",
                columns: table => new
                {
                    GoalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoalTag", x => new { x.GoalId, x.TagId });
                    table.ForeignKey(
                        name: "FK_GoalTag_Goal_GoalId",
                        column: x => x.GoalId,
                        principalTable: "Goal",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GoalTag_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tag",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DeviceTeammate",
                columns: table => new
                {
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeammateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceTeammate", x => new { x.DeviceId, x.TeammateId });
                    table.ForeignKey(
                        name: "FK_DeviceTeammate_Device_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Device",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DeviceTeammate_Teammate_TeammateId",
                        column: x => x.TeammateId,
                        principalTable: "Teammate",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GoalTeammate",
                columns: table => new
                {
                    GoalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeammateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoalTeammate", x => new { x.GoalId, x.TeammateId });
                    table.ForeignKey(
                        name: "FK_GoalTeammate_Goal_GoalId",
                        column: x => x.GoalId,
                        principalTable: "Goal",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GoalTeammate_Teammates_TeammateId",
                        column: x => x.TeammateId,
                        principalTable: "Teammate",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionItem_GoalId",
                table: "ActionItem",
                column: "GoalId");

            migrationBuilder.CreateIndex(
                name: "IX_Device_TeamId",
                table: "Device",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceTag_TagId",
                table: "DeviceTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceTeammate_TeammateId",
                table: "DeviceTeammate",
                column: "TeammateId");

            migrationBuilder.CreateIndex(
                name: "IX_Goal_UltimateGoalId",
                table: "Goal",
                column: "UltimateGoalId");

            migrationBuilder.CreateIndex(
                name: "IX_GoalDevice_GoalId",
                table: "GoalDevice",
                column: "GoalId");

            migrationBuilder.CreateIndex(
                name: "IX_GoalTag_TagId",
                table: "GoalTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_GoalTeammate_TeammateId",
                table: "GoalTeammate",
                column: "TeammateId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_Name_Type",
                table: "Tag",
                columns: new[] { "Name", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_Teammate_TeamId",
                table: "Teammate",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Teammate_UserId",
                table: "Teammate",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UltimateGoal_TeamId",
                table: "UltimateGoal",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_User_TeamId",
                table: "User",
                column: "TeamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionItem");

            migrationBuilder.DropTable(
                name: "DeviceTag");

            migrationBuilder.DropTable(
                name: "DeviceTeammate");

            migrationBuilder.DropTable(
                name: "GoalDevice");

            migrationBuilder.DropTable(
                name: "GoalTag");

            migrationBuilder.DropTable(
                name: "GoalTeammate");

            migrationBuilder.DropTable(
                name: "Device");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "Goal");

            migrationBuilder.DropTable(
                name: "Teammate");

            migrationBuilder.DropTable(
                name: "UltimateGoal");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Team");
        }
    }
}
