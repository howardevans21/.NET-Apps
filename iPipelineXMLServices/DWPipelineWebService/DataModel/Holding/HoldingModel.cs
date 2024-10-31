using DWPipelineWebService.DataModel;

namespace DWiPipeline.DataModel
{
    internal partial class Holding
    {
        public Holding()
        {

        }
        public Holding(string holdingKey, string holdingSysKey, string holdingTypeCode, string holdingStatus)
        {

            this.holdingKey = holdingKey;
            this.holdingSysKey = holdingSysKey; 
            this.holdingTypeCode = holdingTypeCode;
            this.holdingStatus = holdingStatus; 
        }

        private string holdingSysKey = string.Empty; 
        public string HoldingSysKey { get {  return this.holdingSysKey; } set { holdingSysKey = value; } } 

        private string holdingKey = string.Empty; 
        public string HoldingKey { get { return this.holdingKey; } set { holdingKey = value; } }    

        private string holdingTypeCode = string.Empty;
        public string HoldingTypeCode { get { return holdingTypeCode; } set { holdingTypeCode = value; } }

        private string holdingStatus = string.Empty;
        public string HoldingTypeStatus { get { return holdingStatus; } set { holdingStatus = value; } }

        private List<Holding.Policy> policyData = new List<Policy>(); 
        public List<Holding.Policy> PolicyData
        {
            get { return policyData; }
            set { policyData = value; }
        }

        private IntentModel intentModel = new IntentModel();
        public IntentModel IntentData
        {
            get { return intentModel; }
            set { intentModel = value; }
        }

        private ArrangementModel arrangementModel = new ArrangementModel();
        public ArrangementModel ArrangementData
        {
            get { return arrangementModel; }
            set { arrangementModel = value; }
        }

        /// <summary>
        /// Coverage Holidngs 
        /// </summary>
        private List<Holding> coverageHoldings = new List<Holding>();
        public List<Holding> CoverageHolidngs {  get { return coverageHoldings; } set { coverageHoldings = value; } }
    }
}
