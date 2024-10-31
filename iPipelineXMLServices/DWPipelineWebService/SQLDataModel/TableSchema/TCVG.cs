using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace DWPipelineWebService
{
    public class TCVG
    {
        private string pol_id = string.Empty;
        private string cli_typ_cd = string.Empty;
        private string cvg_plan_id = string.Empty;
        private string modified_by = string.Empty;
        private DateTime modified_Date = DateTime.Now;
        private DateTime created_Date = DateTime.Now;

        public TCVG() { }

        [SetsRequiredMembers]
        public TCVG(string pol_id, string cli_typ_cd, string cvg_plan_id, string modified_by, DateTime modified_Date, DateTime created_Date)
        {
            this.pol_id = pol_id;
            this.cli_typ_cd = cli_typ_cd;
            this.cvg_plan_id = cvg_plan_id;
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
        public string ClI_TYP_CD { get { return cli_typ_cd; } set { } }

        [Required]
        [Column(TypeName = "decimal(15,2)")]
        public decimal CVG_FACE_AMT { get; set; }

        [Required]
        [StringLength(06)]
        public string CVG_PLAN_ID { get { return cvg_plan_id; } set { } }

        [Required]
        [StringLength(50)]
        public string ModifiedBy { get { return modified_by; } set { } }

        [Required, NotNull]
        public DateTime? ModifiedDate { get { return modified_Date; } set { } }

        [Required, NotNull]
        public DateTime? CreatedDate { get { return created_Date; } set { } }



    }
}
