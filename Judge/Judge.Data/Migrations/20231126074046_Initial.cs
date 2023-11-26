﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Judge.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Contests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: false),
                    FinishTime = table.Column<DateTime>(nullable: false),
                    FreezeTime = table.Column<DateTime>(nullable: true),
                    IsOpened = table.Column<bool>(nullable: false),
                    Rules = table.Column<int>(nullable: false),
                    CheckPointTime = table.Column<DateTime>(nullable: true),
                    OneLanguagePerTask = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsCompilable = table.Column<bool>(nullable: false),
                    CompilerPath = table.Column<string>(nullable: true),
                    CompilerOptionsTemplate = table.Column<string>(nullable: true),
                    OutputFileTemplate = table.Column<string>(nullable: true),
                    RunStringFormat = table.Column<string>(nullable: true),
                    IsHidden = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestsFolder = table.Column<string>(nullable: true),
                    TimeLimitMilliseconds = table.Column<int>(nullable: false),
                    MemoryLimitBytes = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CreationDateUtc = table.Column<DateTime>(nullable: false),
                    Statement = table.Column<string>(nullable: true),
                    IsOpened = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Submits",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    LanguageId = table.Column<int>(nullable: false),
                    SourceCode = table.Column<string>(nullable: true),
                    SubmitDateUtc = table.Column<DateTime>(nullable: false),
                    ProblemId = table.Column<long>(nullable: false),
                    UserHost = table.Column<string>(nullable: true),
                    SessionId = table.Column<string>(nullable: true),
                    SubmitType = table.Column<byte>(nullable: false),
                    ContestId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContestTasks",
                columns: table => new
                {
                    ContestId = table.Column<int>(nullable: false),
                    TaskName = table.Column<string>(nullable: false),
                    TaskId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContestTasks", x => new { x.ContestId, x.TaskName });
                    table.ForeignKey(
                        name: "FK_ContestTasks_Contests_ContestId",
                        column: x => x.ContestId,
                        principalTable: "Contests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContestTasks_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubmitResults",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(nullable: false),
                    SubmitId = table.Column<long>(nullable: true),
                    PassedTests = table.Column<int>(nullable: true),
                    TotalBytes = table.Column<int>(nullable: true),
                    TotalMilliseconds = table.Column<int>(nullable: true),
                    CompileOutput = table.Column<string>(nullable: true),
                    RunDescription = table.Column<string>(nullable: true),
                    RunOutput = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubmitResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubmitResults_Submits_SubmitId",
                        column: x => x.SubmitId,
                        principalSchema: "dbo",
                        principalTable: "Submits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CheckQueue",
                schema: "dbo",
                columns: table => new
                {
                    SubmitResultId = table.Column<long>(nullable: false),
                    CreationDateUtc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckQueue", x => x.SubmitResultId);
                    table.ForeignKey(
                        name: "FK_CheckQueue_SubmitResults_SubmitResultId",
                        column: x => x.SubmitResultId,
                        principalSchema: "dbo",
                        principalTable: "SubmitResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContestTasks_TaskId",
                table: "ContestTasks",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubmitResults_SubmitId",
                schema: "dbo",
                table: "SubmitResults",
                column: "SubmitId");

            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[DequeueSubmitCheck]
AS
BEGIN

	;WITH CTE(SubmitResultId, CreationDateUtc)
	AS
	(
		SELECT TOP 1 *
		FROM dbo.CheckQueue cq WITH(UPDLOCK, READPAST)
		ORDER BY cq.CreationDateUtc
	)
	DELETE CTE
	OUTPUT
		DELETED.*
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContestTasks");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "CheckQueue",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Contests");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "SubmitResults",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Submits",
                schema: "dbo");

            migrationBuilder.Sql("DROP PROCEDURE [dbo].[DequeueSubmitCheck]");
        }
    }
}
