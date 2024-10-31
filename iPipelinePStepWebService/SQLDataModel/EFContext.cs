using iPipelinePStepWebService;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Immutable;
using System.Reflection;
using System.Reflection.Metadata;
using System.Xml;


namespace DWPipelineWebService
{
    public partial class EFContext:DbContext
    {
        public virtual DbSet<TBENE> Beneficiaries { get; set; }
        public virtual DbSet<TPOL> Policies { get; set; }
        public virtual DbSet<TCLI> Clients { get; set; }
        public virtual DbSet<TCVG> Coverages { get; set; }
        public virtual DbSet<TCLIA> Addresses { get; set; }
        public virtual DbSet<TCLIC> Contacts { get; set; }
        public virtual DbSet<TCDOC> Documents { get; set; }
        public virtual DbSet<TERROR> Errors { get; set; }
        public virtual DbSet<SP_POLICY_RESULTS> PENDING_POLICIES { get; set; }
        public virtual DbSet<SP_POLICY_RESULTS> VERIFY_POLICIES { get; set; }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public EFContext() : base()
        {

        }

        public EFContext(DbContextOptions<EFContext> options)
         : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            // Step3: Get the Section to Read from the Configuration File
            var configSection = configBuilder.GetSection("ConnectionStrings");
            // Step4: Get the Configuration Values based on the Config key.
            var connectionString = configSection["CDR"] ?? null;

            //Configuring the Connection String
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<TPOL>().ToTable("TPOL").HasIndex(u => new { u.POL_ID, u.POL_INS_PURP_CD }).IsUnique();
            modelBuilder.Entity<TBENE>().ToTable("TBENE").HasIndex(u => new { u.POL_ID, u.BNFY_NM, u.BNFY_TYP_CD, u.BNFY_REL_INSRD_CD }).IsUnique();
            modelBuilder.Entity<TCVG>().ToTable("TCVG").HasIndex(u => new { u.POL_ID, u.ClI_TYP_CD, u.CVG_PLAN_ID }).IsUnique();
            modelBuilder.Entity<TCLI>().ToTable("TCLI").HasIndex(u => new { u.POL_ID, u.CLI_TYP_CD }).IsUnique();
            modelBuilder.Entity<TCLIA>().ToTable("TCLIA").HasIndex(u => new { u.POL_ID, u.CLI_TYP_CD, u.CLI_ADDR_TYP_CD }).IsUnique();
            modelBuilder.Entity<TCDOC>().ToTable("TCDOC").HasIndex(u => new { u.POL_ID, u.CLI_DOC_ID }).IsUnique();
            modelBuilder.Entity<TERROR>().ToTable("TERROR").HasIndex(u => new { u.POL_ID, u.CreateDate, u.APP_ID });
            modelBuilder.Entity<SP_POLICY_RESULTS>(entity => entity.HasNoKey());
        }

        /***************************************
        * Get Pending Policies
        *********************************************/
        public IEnumerable<SP_POLICY_RESULTS> Get_Pending_Policies(int days)
        {

            var results = PENDING_POLICIES.FromSqlInterpolated
                         ($"EXECUTE dbo.colina_usp_ipipeline_PendingPolicies {days} ")
                         .ToList();

            return results;
        }

        /**********************************************************
         * Verify Policies
         ********************************************************/

        public IEnumerable<SP_POLICY_RESULTS> VerifyPolicy(string pol_id)
        {

            var results = PENDING_POLICIES.FromSqlRaw
                         ($"EXECUTE dbo.[colina_usp_ipipeline_VerifyStatus]  {pol_id} ")
                         .ToList();

            return results;
        }
    }
}

