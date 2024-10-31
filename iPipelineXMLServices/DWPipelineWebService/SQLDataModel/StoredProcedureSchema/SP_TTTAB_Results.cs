using System.ComponentModel.DataAnnotations.Schema;

namespace DWPipelineWebService.SQLDataModel.StoredProcedureSchema
{
   // [NotMapped]
    public class SP_TTTAB_Results
    {
        public string CompanyUD { get; set; } = "";
        public string TranslationType { get; set; } = "";
        public string TranslationFrom { get; set; } = "";
        public string TranslationTo { get; set; } = "";
    }
}
