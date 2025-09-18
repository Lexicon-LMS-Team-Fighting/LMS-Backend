using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Companies.Infractructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEntityRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_LMSActivity_ActivityId",
                table: "Document");

            migrationBuilder.DropForeignKey(
                name: "FK_Document_User_UserId1",
                table: "Document");

            migrationBuilder.DropForeignKey(
                name: "FK_LMSActivity_ActivityType_ActivityTypeId",
                table: "LMSActivity");

            migrationBuilder.DropForeignKey(
                name: "FK_LMSActivity_Module_ModuleId",
                table: "LMSActivity");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCourse_User_UserId1",
                table: "UserCourse");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropIndex(
                name: "IX_UserCourse_UserId1",
                table: "UserCourse");

            migrationBuilder.DropIndex(
                name: "IX_Document_UserId1",
                table: "Document");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LMSActivity",
                table: "LMSActivity");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "UserCourse");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Document");

            migrationBuilder.RenameTable(
                name: "LMSActivity",
                newName: "Activity");

            migrationBuilder.RenameIndex(
                name: "IX_LMSActivity_ModuleId",
                table: "Activity",
                newName: "IX_Activity_ModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_LMSActivity_ActivityTypeId",
                table: "Activity",
                newName: "IX_Activity_ActivityTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activity",
                table: "Activity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_ActivityType_ActivityTypeId",
                table: "Activity",
                column: "ActivityTypeId",
                principalTable: "ActivityType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_Module_ModuleId",
                table: "Activity",
                column: "ModuleId",
                principalTable: "Module",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Activity_ActivityId",
                table: "Document",
                column: "ActivityId",
                principalTable: "Activity",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activity_ActivityType_ActivityTypeId",
                table: "Activity");

            migrationBuilder.DropForeignKey(
                name: "FK_Activity_Module_ModuleId",
                table: "Activity");

            migrationBuilder.DropForeignKey(
                name: "FK_Document_Activity_ActivityId",
                table: "Document");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Activity",
                table: "Activity");

            migrationBuilder.RenameTable(
                name: "Activity",
                newName: "LMSActivity");

            migrationBuilder.RenameIndex(
                name: "IX_Activity_ModuleId",
                table: "LMSActivity",
                newName: "IX_LMSActivity_ModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_Activity_ActivityTypeId",
                table: "LMSActivity",
                newName: "IX_LMSActivity_ActivityTypeId");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "UserCourse",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "Document",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LMSActivity",
                table: "LMSActivity",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCourse_UserId1",
                table: "UserCourse",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Document_UserId1",
                table: "Document",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_UserName",
                table: "User",
                column: "UserName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Document_LMSActivity_ActivityId",
                table: "Document",
                column: "ActivityId",
                principalTable: "LMSActivity",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Document_User_UserId1",
                table: "Document",
                column: "UserId1",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LMSActivity_ActivityType_ActivityTypeId",
                table: "LMSActivity",
                column: "ActivityTypeId",
                principalTable: "ActivityType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LMSActivity_Module_ModuleId",
                table: "LMSActivity",
                column: "ModuleId",
                principalTable: "Module",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCourse_User_UserId1",
                table: "UserCourse",
                column: "UserId1",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
