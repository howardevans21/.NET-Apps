using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace IDWORKS_STUDENT.Migrations
{
    [DbContext(typeof(IDWORKS_STUDENT.Models.StudentContext))]
    [Migration("MigrationV4")]
    public class DataMigration : Migration
    {
        protected override void Up([NotNullAttribute] MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "colina_Student_Card_Accident",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDWCoverage = table.Column<string>(type: "varchar(50)", nullable: false),
                    IDWFirstName = table.Column<string>(type: "varchar(50)", nullable: false),
                    IDWLastName = table.Column<string>(type: "varchar(50)", nullable: false),
                    IDWPolicyNumber = table.Column<string>(type: "varchar(20)", nullable: false),
                    IDWEffectiveDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IDWEffectiveDateText = table.Column<string>(type: "varchar(10)", nullable: false),
                    IDWCoverageProvider = table.Column<string>(type: "varchar(50)", nullable: false),
                    IDWSchool = table.Column<string>(type: "varchar(50)", nullable: false)
                },

                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);

                    table.UniqueConstraint(
                      name: "Unique_Students",
                      columns: x => new { x.IDWFirstName, x.IDWLastName, x.IDWPolicyNumber, x.IDWSchool, x.IDWCoverageProvider, x.IDWCoverage, x.IDWEffectiveDate, x.IDWEffectiveDateText});
                });
        }
    }
}
