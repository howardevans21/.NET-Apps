using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace DWPipelineWebService
{
    public class TBENE
    {
      
        public TBENE() { }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(10)]
        public string POL_ID { get; set; }

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
    }
}

