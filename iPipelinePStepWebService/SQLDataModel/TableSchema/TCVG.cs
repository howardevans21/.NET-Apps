using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace DWPipelineWebService
{
    public class TCVG
    {
        public TCVG() { }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(10)]
        public string POL_ID { get; set; }

        [Required]
        [StringLength(2)]
        public string ClI_TYP_CD { get; set; }

        [Required]
        [Column(TypeName = "decimal(15,2)")]
        public decimal CVG_FACE_AMT { get; set; }

        [Required]
        [StringLength(05)]
        public string CVG_PLAN_ID { get; set; }
    }
}
