using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DWPipelineWebService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SP_TTTAB_Results",
                columns: table => new
                {
                    CompanyUD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TranslationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TranslationFrom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TranslationTo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "TBENE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    POL_ID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BNFY_NM = table.Column<string>(type: "nvarchar(54)", maxLength: 54, nullable: false),
                    CLI_TYP_CD = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    BNFY_TYP_CD = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    BNFY_REL_INSRD_CD = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    BNFY_PRCDS_PCT = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    BNFY_TRUSTEE_NM = table.Column<string>(type: "nvarchar(54)", maxLength: 54, nullable: true),
                    BNFY_TRUSTEE_REL_CD = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBENE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TCDOC",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    POL_ID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CLI_TYP_CD = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    CLI_DOC_ID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CLI_DOC_CNTRY_CD = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    CLI_DOC_XPRY_DT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TCDOC", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TCLI",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    POL_ID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CLI_TYP_CD = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    CLI_INDV_GIV_NM = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    CLI_INDV_MID_NM = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    CLI_INDV_SUR_NM = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    CLI_INDV_TITL_TXT = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    CLI_INDV_SFX_NM = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    CLI_COMPANY_NM = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CLI_BTH_DT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CLI_SEX_CD = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CLI_SMKR_CD = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CLI_TAX_ID = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: true),
                    CLI_BTH_LOC_CD = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    CLI_EI_IND = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CLI_SHR_IND = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CLI_EMPL_YR_DUR = table.Column<decimal>(type: "decimal(3,0)", nullable: true),
                    CLI_EMPL_NM = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CLI_SSN_ID = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    CLI_TIN_ID = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    CLI_NATLTY_CNTRY_CD = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    CLI_PRM_RES_CNTRY = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    CLI_COMNT_TXT = table.Column<string>(type: "nvarchar(257)", maxLength: 257, nullable: true),
                    CLI_INCM_EARN_AMT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TCLI", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TCLIA",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    POL_ID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CLI_TYP_CD = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CLI_ADDR_TYP_CD = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CLI_ADDR_LN_1_TXT = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CLI_ADDR_LN_2_TXT = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CLI_ADDR_LN_3_TXT = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CLI_ADDR_ADDL_TXT = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    CLI_ADDR_MUN_CD = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    CLI_CITY_NM_TXT = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CLI_CTRY_CD = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    CLI_PSTL_CD = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: true),
                    CLI_ADDR_YR_DUR = table.Column<decimal>(type: "decimal(3,0)", nullable: true),
                    CLI_CNCT_WORK_PH = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CLI_CNCT_EMAIL = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CLI_CNCT_CELL_PH = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CLI_CNCT_HOME_PH = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CLI_CRNT_LOC_CD = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TCLIA", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TCVG",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    POL_ID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ClI_TYP_CD = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    CVG_FACE_AMT = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    CVG_PLAN_ID = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TCVG", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TERROR",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    POL_ID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    APP_ID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ERROR_MESSAGE = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TERROR", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TPOL",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    POL_ID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    POL_CREATE_STAT_CD = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    POL_INS_PURP_CD = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    POL_APP_RECV_DT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    POL_ISS_EFF_DT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    POL_CTRY_CD = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    POL_ISS_LOC_CD = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    POL_BILL_MODE_CD = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    POL_BILL_TYP_CD = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    POL_DIV_OPT_CD = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    SERV_AGT_ID = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    AGT_SHRT_PCT_1 = table.Column<decimal>(type: "decimal(3,2)", nullable: true),
                    AGT_ID_2 = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    AGT_SHRT_PCT_2 = table.Column<decimal>(type: "decimal(3,2)", nullable: true),
                    POL_CONTINGENT_GIV_NM = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    POL_CONTINGENT_SUR_NM = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    POL_CONTINGENT_BTH_DT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    POL_CONTINGENT_CLI_INSRD_CD = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    POL_CLI_INSRD_CD = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TPOL", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TBENE_POL_ID_BNFY_NM_BNFY_TYP_CD_BNFY_REL_INSRD_CD",
                table: "TBENE",
                columns: new[] { "POL_ID", "BNFY_NM", "BNFY_TYP_CD", "BNFY_REL_INSRD_CD" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TCDOC_POL_ID_CLI_DOC_ID",
                table: "TCDOC",
                columns: new[] { "POL_ID", "CLI_DOC_ID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TCLI_POL_ID_CLI_TYP_CD",
                table: "TCLI",
                columns: new[] { "POL_ID", "CLI_TYP_CD" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TCLIA_POL_ID_CLI_TYP_CD_CLI_ADDR_TYP_CD",
                table: "TCLIA",
                columns: new[] { "POL_ID", "CLI_TYP_CD", "CLI_ADDR_TYP_CD" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TCVG_POL_ID_ClI_TYP_CD_CVG_PLAN_ID",
                table: "TCVG",
                columns: new[] { "POL_ID", "ClI_TYP_CD", "CVG_PLAN_ID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TERROR_POL_ID_CreateDate_APP_ID",
                table: "TERROR",
                columns: new[] { "POL_ID", "CreateDate", "APP_ID" });

            migrationBuilder.CreateIndex(
                name: "IX_TPOL_POL_ID_POL_INS_PURP_CD",
                table: "TPOL",
                columns: new[] { "POL_ID", "POL_INS_PURP_CD" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SP_TTTAB_Results");

            migrationBuilder.DropTable(
                name: "TBENE");

            migrationBuilder.DropTable(
                name: "TCDOC");

            migrationBuilder.DropTable(
                name: "TCLI");

            migrationBuilder.DropTable(
                name: "TCLIA");

            migrationBuilder.DropTable(
                name: "TCVG");

            migrationBuilder.DropTable(
                name: "TERROR");

            migrationBuilder.DropTable(
                name: "TPOL");
        }
    }
}
