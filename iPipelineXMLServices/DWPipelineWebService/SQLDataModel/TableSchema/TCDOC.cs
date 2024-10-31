using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace DWPipelineWebService
{
    public class TCDOC
    {
        private string pol_id = string.Empty;
        private string cli_typ_cd = string.Empty;
        private string cli_doc_id = string.Empty;
        private string modified_by = string.Empty;
        private DateTime modified_Date = DateTime.Now;
        private DateTime created_Date = DateTime.Now;

        public TCDOC() { }

        [SetsRequiredMembers]
        public TCDOC(string pol_id, string cli_typ_cd, string cli_doc_id, string modified_by, DateTime modified_Date, DateTime created_Date)
        {
            this.pol_id = pol_id;
            this.cli_typ_cd = cli_typ_cd;
            this.cli_doc_id = cli_doc_id;
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

        [Required]
        [StringLength(50)]
        public string CLI_DOC_ID { get { return cli_doc_id; }  set { } }

        [AllowNull]
        [StringLength(2)]
        public string? CLI_DOC_CNTRY_CD { get; set; }

        [AllowNull]
        public DateTime? CLI_DOC_XPRY_DT { get;  set; }

        [Required]
        [StringLength(50)]
        public string ModifiedBy { get { return modified_by; } set { } }

        [Required, NotNull]
        public DateTime? ModifiedDate { get { return modified_Date; } set { } }

        [Required, NotNull]
        public DateTime? CreatedDate { get { return created_Date; } set { } }

    }
}
