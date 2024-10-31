namespace DWPipelineWebService.DataModel.Holding.ChildrenModels
{
    public class RiskModel
    {
        public RiskModel() { }

        public RiskModel(DateTime dateLastUpdated, bool existingInsurance, 
             string replacementInd, bool rejectionInd)
        {
            this.dateLastUpdated = dateLastUpdated;
            this.existingInsurance = existingInsurance;
            this.replacementInd = replacementInd;
            this.rejectionInd = rejectionInd;
        }

        private DateTime dateLastUpdated;
        public DateTime DateLastUpdated
        {
            get { return dateLastUpdated; }
        }

       private bool existingInsurance = false;
       public bool ExistingInsurance
        {
            get { return existingInsurance; }
        }

        private string replacementInd = string.Empty;
        public string ReplacementInd
        {
            get { return replacementInd; }
        }
        
        private bool rejectionInd = false;
        public bool RejectionInd
        {
            get { return rejectionInd; }
        }

    }
}
