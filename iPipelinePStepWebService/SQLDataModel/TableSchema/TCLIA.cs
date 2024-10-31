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
    public class TCLIA
    {
        public TCLIA() { }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

  
        [Required]
        [StringLength(10)]
        public string POL_ID { get; set; }

        [Required]
        [AllowNull]
        public string? CLI_TYP_CD { get; set; }

        [Required]
        [AllowNull]
        public string? CLI_ADDR_TYP_CD { get; set; }

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

      

    }
}
