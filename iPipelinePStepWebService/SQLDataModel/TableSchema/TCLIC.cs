using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace DWPipelineWebService
{
    public class TCLIC
    {
        public TCLIC() { }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(10)]
        public string POL_ID { get; set; }
        [Required]
        [StringLength(2)]
        public string Cli_Typ_Cd { get; set; }
    

        [Required]
        [StringLength(2)]
        public string CLI_CNTCT_ID_CD { get; set; }

        [Required]
        [StringLength(50)]
        public string CLI_CNTCT_ID_TXT { get; set; }

        [Required]
        [StringLength(6)]
        public string CLI_CNTCT_PH_EXT { get; set; }
    }
}
