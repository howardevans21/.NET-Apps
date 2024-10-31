using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPipelinePStepWebService
{
    public class TCDOC
    {
     
        public TCDOC() { }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(10)]
        public string POL_ID { get; set; }

        [Required]
        [StringLength(2)]
        public string CLI_TYP_CD { get; set; }

        [Required]
        [StringLength(50)]
        public string CLI_DOC_ID { get; set; }

        [AllowNull]
        [StringLength(2)]
        public string? CLI_DOC_CNTRY_CD { get; set; }

        [AllowNull]
        public DateTime? CLI_DOC_XPRY_DT { get; set; }
    }
}
