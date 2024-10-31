using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace DWPipelineWebService
{
    public class TCLI
    {
        private string pol_id = string.Empty;
        private string cli_typ_cd = string.Empty;
        private string modified_by = string.Empty;
        private DateTime modified_Date = DateTime.Now;
        private DateTime created_Date = DateTime.Now;

        public TCLI() { }

        [SetsRequiredMembers]
        public TCLI(string pol_id, string cli_typ_cd, string modified_by, DateTime modified_Date, DateTime created_Date)
        {
            this.pol_id = pol_id;
            this.cli_typ_cd = cli_typ_cd;
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
        [StringLength(2)]
        public string CLI_TYP_CD { get { return cli_typ_cd; } set { } }

        [Required, NotNull]
        [StringLength(25)]
        public string CLI_INDV_GIV_NM { get; set; }

        [Required, AllowNull]
        [StringLength(25)]
        public string? CLI_INDV_MID_NM { get; set; }

        [Required, NotNull]
        [StringLength(25)]
        public string CLI_INDV_SUR_NM { get; set; }

        [AllowNull]
        [StringLength(15)]
        public string? CLI_INDV_TITL_TXT { get; set; }

        [AllowNull]
        [StringLength(15)]
        public string? CLI_INDV_SFX_NM { get; set; }

        [AllowNull]
        [StringLength(50)]
        public string? CLI_COMPANY_NM { get; set; }

        [AllowNull]
        public DateTime? CLI_BTH_DT { get; set; }

        [AllowNull]
        [StringLength(1)]
        public string? CLI_SEX_CD { get; set; }

        [AllowNull]
        [StringLength(1)]
        public string? CLI_SMKR_CD { get; set; }

        [AllowNull]
        [StringLength(9)]
        public string? CLI_TAX_ID { get; set; }

        [AllowNull]
        [StringLength(6)]  
        public string? CLI_BTH_LOC_CD { get; set; }

        [AllowNull]
        [StringLength(1)]
        public string? CLI_EI_IND { get; set; }

        [AllowNull]
        [StringLength(1)]
        public string? CLI_SHR_IND { get; set; }

        [AllowNull]
        [Column(TypeName = "decimal(3,0)")]
        public decimal? CLI_EMPL_YR_DUR { get; set; }

        [AllowNull]
        [StringLength(50)]
        public string? CLI_EMPL_NM { get; set; }

        [AllowNull]
        [StringLength(15)]
        public string? CLI_SSN_ID { get; set; }

        [AllowNull]
        [StringLength(15)]
        public string? CLI_TIN_ID { get; set; }

        [AllowNull]
        [StringLength(2)]
        public string? CLI_NATLTY_CNTRY_CD { get; set; }

        [AllowNull]
        [StringLength(2)]
        public string? CLI_PRM_RES_CNTRY { get; set; }

        [AllowNull]
        [StringLength(257)]
        public string? CLI_COMNT_TXT { get; set; }

        [AllowNull]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CLI_INCM_EARN_AMT { get; set; }

        [Required]
        [StringLength(50)]
        public string ModifiedBy { get { return modified_by; } set { } }

        [Required, NotNull]
        public DateTime? ModifiedDate { get { return modified_Date; } set { } }

        [Required, NotNull]
        public DateTime? CreatedDate { get { return created_Date; } set { } }
    }
}
