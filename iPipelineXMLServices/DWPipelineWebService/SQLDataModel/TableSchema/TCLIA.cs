using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;


namespace DWPipelineWebService
{
    public class TCLIA
    {
        private string pol_id = string.Empty;
        private string cli_typ_cd = string.Empty;
        private string cli_addr_typ_cd = string.Empty;
        private string modified_by = string.Empty;
        private DateTime modified_Date = DateTime.Now;
        private DateTime created_Date = DateTime.Now;


        public TCLIA() { }

        [SetsRequiredMembers]

        public TCLIA(string pol_ID, string cli_typ_cd, string cli_addr_typ_cd, string modified_by, DateTime modified_Date, DateTime created_Date)
        {
            this.pol_id = pol_ID;
            this.cli_typ_cd = cli_typ_cd;
            this.cli_addr_typ_cd = cli_addr_typ_cd;
            this.modified_by = modified_by;
            this.modified_Date = modified_Date;
            this.created_Date = created_Date;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(10)]
        public string POL_ID { get { return pol_id; } set { } }

        [Required]
        [AllowNull]
        public string? CLI_TYP_CD { get { return cli_typ_cd; } set { } }

        [Required]
        [AllowNull]
        public string? CLI_ADDR_TYP_CD { get { return cli_addr_typ_cd; } set { } }

        [AllowNull]
        [StringLength(30)]
        public string? CLI_ADDR_LN_1_TXT { get; set; }

        [AllowNull]
        [StringLength(30)]
        public string? CLI_ADDR_LN_2_TXT { get; set; }

        [AllowNull]
        [StringLength(30)]
        public string? CLI_ADDR_LN_3_TXT { get; set; }

        [AllowNull]
        [StringLength(60)]
        public string? CLI_ADDR_ADDL_TXT { get; set; }

        [AllowNull]
        [StringLength(4)]
        public string? CLI_ADDR_MUN_CD { get; set; }

        [AllowNull]
        [StringLength(30)]
        public string? CLI_CITY_NM_TXT { get; set; }

        [AllowNull]
        [StringLength(2)]
        public string? CLI_CTRY_CD { get; set; }

        [AllowNull]
        [StringLength(9)]
        public string? CLI_PSTL_CD { get; set; }

        [AllowNull]
        [Column(TypeName = "decimal(3,0)")]
        public decimal? CLI_ADDR_YR_DUR { get; set; }

        [AllowNull]
        [StringLength(50)]
        public string? CLI_CNCT_WORK_PH { get; set; }

        [AllowNull]
        [StringLength(50)]
        public string? CLI_CNCT_EMAIL { get; set; }

        [AllowNull]
        [StringLength(50)]
        public string? CLI_CNCT_CELL_PH { get; set; }

        [AllowNull]
        [StringLength(50)]
        public string? CLI_CNCT_HOME_PH { get; set; }

        [AllowNull]
        [StringLength(2)]
        public string? CLI_CRNT_LOC_CD { get; set; }

        [Required]
        [StringLength(50)]
        public string ModifiedBy { get { return modified_by; } set { } }

        [Required, NotNull]
        public DateTime? ModifiedDate { get { return modified_Date; } set { } }

        [Required, NotNull]
        public DateTime? CreatedDate { get { return created_Date; } set { } }
  
    }
}

        
      

     
    

