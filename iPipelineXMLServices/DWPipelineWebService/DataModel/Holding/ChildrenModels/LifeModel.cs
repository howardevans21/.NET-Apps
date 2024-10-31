using DWPipelineWebService.DataModel.Holding;

namespace DWPipelineWebService.DataModel
{
    internal class LifeModel
    {
        private decimal faceAmt = 0;
        private string divType = string.Empty;

        public LifeModel(decimal faceAmt, string divType, List<CoverageModel> coverageModels)
        {
            this.faceAmt = faceAmt; 
            this.divType = divType;
            this.coverageModels = coverageModels;
        }

        public decimal FaceAmt { get { return this.faceAmt; } }
        public string DivType { get { return this.divType; } }

        private List<CoverageModel> coverageModels = new List<CoverageModel>();
        public List<CoverageModel> CoverageModels { get {  return coverageModels; } }
       
    }
}
