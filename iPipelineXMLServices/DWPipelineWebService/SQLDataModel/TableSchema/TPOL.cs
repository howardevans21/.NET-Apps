using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace DWPipelineWebService
{
    public class TPOL
    {
        private string pol_id = string.Empty;
        private string pol_ins_purp_cd = string.Empty;
        private string modified_by = string.Empty;
        private DateTime modified_Date = DateTime.Now;
        private DateTime created_Date = DateTime.Now;

        public TPOL() { }

        [SetsRequiredMembers]
        public TPOL(string pol_id, string pol_ins_purp_cd, string modified_by, DateTime modified_Date, DateTime created_Date)
        {
            this.pol_id = pol_id;
            this.pol_ins_purp_cd = pol_ins_purp_cd;
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

        [AllowNull]
        [StringLength(1)]
        public string? POL_CREATE_STAT_CD { get; set; }

        [Required, NotNull]
        [StringLength(1)]
        public string POL_INS_PURP_CD { get { return pol_ins_purp_cd; } set { } }
        
        [AllowNull]
        public DateTime? POL_APP_RECV_DT { get; set; }

        [AllowNull]
        public DateTime? POL_ISS_EFF_DT { get; set; }

        [AllowNull]
        [StringLength(2)]
        public string? POL_CTRY_CD { get; set; }

        [AllowNull]
        [StringLength(2)]
        public string? POL_ISS_LOC_CD { get; set; }

        [AllowNull]
        [StringLength(2)]
        public string? POL_BILL_MODE_CD { get; set; }

        [AllowNull]
        [StringLength(2)]
        public string? POL_BILL_TYP_CD { get; set; }

        [AllowNull]
        [StringLength(1)]
        public string? POL_DIV_OPT_CD { get; set; }

        [AllowNull]
        [StringLength(6)]
        public string? SERV_AGT_ID { get; set; }

        [AllowNull]
        [Column(TypeName = "decimal(3,2)")]
        public decimal? AGT_SHRT_PCT_1 { get; set; }

        [AllowNull]
        [StringLength(6)]
        public string? AGT_ID_2 { get; set; }

        [AllowNull]
        [Column(TypeName = "decimal(3,2)")]
        public decimal? AGT_SHRT_PCT_2 { get; set; }

        [AllowNull]
        [StringLength(25)]
        public string? POL_CONTINGENT_GIV_NM { get; set; }

        [AllowNull]
        [StringLength(25)]
        public string? POL_CONTINGENT_SUR_NM { get; set; }

        [AllowNull]
        public DateTime? POL_CONTINGENT_BTH_DT { get; set; }

        [AllowNull]
        [StringLength(5)]
        public string? POL_CONTINGENT_CLI_INSRD_CD { get; set; }

        [AllowNull]
        [StringLength(5)]
        public string? POL_CLI_INSRD_CD { get; set; }

        [Required]
        [StringLength(50)]
        public string ModifiedBy { get { return modified_by; } set { } }

        [Required, NotNull]
        public DateTime? ModifiedDate { get { return modified_Date; } set { } }

        [Required, NotNull]
        public DateTime? CreatedDate { get { return created_Date; } set { } }

    }
}
