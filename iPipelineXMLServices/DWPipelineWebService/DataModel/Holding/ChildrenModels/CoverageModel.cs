namespace DWPipelineWebService.DataModel.Holding
{
    internal class CoverageModel
    {
        public CoverageModel() { }

        public CoverageModel(string id, decimal currentAmt, decimal initCovtAmt, string productCode, string indicatorCode, List<LifeParticipantModel> lifeParticipantModels, PurposeCodes purposeCodes, string indicator_TC_Code)
        {
            this.id = id;
            this.currentAmt = currentAmt;
            this.productCode = productCode;
            this.indicatorCode = indicatorCode;
            this.lifeParticipantModels = lifeParticipantModels;
            this.purposeCodes = purposeCodes;
            this.indicatorTCCode = indicator_TC_Code;
        }

        private string id = string.Empty;
        public string ID {  get { return id; } }
        
        private decimal currentAmt = 0;
        public decimal CurrentAmt { get { return currentAmt; } }

        private decimal initialCovtAmt = 0;
        public decimal InitialCovtAmt { get { return initialCovtAmt; } }

        private string productCode = string.Empty;
        public string ProductCode { get { return productCode; } }

        private string indicatorCode = string.Empty;
        public string IndicatorCode { get { return indicatorCode; } }

        private string indicatorTCCode = string.Empty;
        public string Indicator_TC_Code { get { return indicatorTCCode; } }
        
        private List<LifeParticipantModel> lifeParticipantModels = new List<LifeParticipantModel>();  
        public List<LifeParticipantModel> LifeParticipants { get { return lifeParticipantModels; } }

        private PurposeCodes purposeCodes = null;
        public PurposeCodes MainPurposeCodes {  get { return purposeCodes; } }
    
       private List<AdditionalCoverages> additionalCoverages = new List<AdditionalCoverages>();
       public List<AdditionalCoverages> AddtionalCoverages {  get { return additionalCoverages; } }
    }
}

internal class AdditionalCoverages
{
    public AdditionalCoverages(decimal currentAmt, string productCode, string indicatorCode)
    {
        this.currentAmt = currentAmt;
        this.productCode = productCode;
        this.indicatorCode = indicatorCode;
    }

    private decimal currentAmt = 0;
    public decimal CurrentAmt { get { return currentAmt; } }
    
    private string productCode = string.Empty;
    public string ProductCode { get { return productCode; } }

    private string indicatorCode = string.Empty;
    public string IndicatorCode { get { return indicatorCode; } }
}

internal class PurposeCodes
{
    private bool purpose_Insurance_Family = false;
    private bool purpose_Insurance_Loan = false;
    private bool purpose_Insurance_Other = false;

    public PurposeCodes(bool purpose_Insurance_Family, bool purpose_Insurance_Loan, bool purpose_Insurance_Other)
    {
        this.purpose_Insurance_Family = purpose_Insurance_Family;
        this.purpose_Insurance_Loan = purpose_Insurance_Loan;
        this.purpose_Insurance_Other = purpose_Insurance_Other;
    }

    public bool Purpose_Insurance_Family { get { return purpose_Insurance_Family; } }

    public bool Purpose_Insurance_Loan { get { return purpose_Insurance_Loan; } }
    
    public bool Purpose_Insurance_Other { get { return purpose_Insurance_Other;  } }

    private string purposeCode = string.Empty;
    public string PurposeCode
    {
        get
        {
            if (purpose_Insurance_Family) purposeCode = "P";
            else if (purpose_Insurance_Loan) purposeCode = "M";
            else purposeCode = "P";

            return purposeCode;
        }
    }
}
