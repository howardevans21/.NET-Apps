using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace DWPipelineWebService
{
    public class TBENE
    {
        private string pol_id = string.Empty;
        private string modified_by = string.Empty;
        private DateTime modified_Date = DateTime.Now;
        private DateTime created_Date = DateTime.Now;

        public TBENE() { }

        [SetsRequiredMembers]
        public TBENE(string pol_id, string modified_by, DateTime modified_Date, DateTime created_Date)
        {
            this.pol_id = pol_id;
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

        [Required, NotNull]
        [StringLength(54)]
        public string BNFY_NM { get; set; }

        [Required, NotNull]
        [StringLength(2)]
        public string CLI_TYP_CD { get; set; }

        [AllowNull]
        [StringLength(1)]
        public string BNFY_TYP_CD { get; set; }

        [AllowNull]
        [StringLength(5)]
        public string BNFY_REL_INSRD_CD { get; set; }

        [AllowNull]
        [Column(TypeName = "decimal(18,3)")]
        public decimal BNFY_PRCDS_PCT { get; set; }

        [AllowNull]
        [StringLength(54)]
        public string? BNFY_TRUSTEE_NM { get; set; }

        [AllowNull]
        [StringLength(5)]
        public string? BNFY_TRUSTEE_REL_CD { get; set; }

        [Required]
        [StringLength(50)]
        public string ModifiedBy { get { return modified_by; } set { } }

        [Required, NotNull]
        public DateTime? ModifiedDate { get { return modified_Date; } set { } }

        [Required, NotNull]
        public DateTime? CreatedDate { get { return created_Date; } set { } }

    }
}
