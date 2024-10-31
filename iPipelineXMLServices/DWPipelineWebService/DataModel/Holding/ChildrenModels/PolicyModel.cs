using DWPipelineWebService.DataModel;
using System.Configuration;

namespace DWiPipeline.DataModel
{
    internal partial class Holding
    {
        internal class Policy
        {

            internal Policy(string policyNumber, string lob, string productType, string planName, string policyStatus, string jurisdiction, string productCode, string paymentMethod)
            {
                this.policyNumber = policyNumber;
                this.lob = lob;
                this.planName = planName;
                this.policyStatus = policyStatus;
                this.jurisdiction = jurisdiction;
                this.productCode = productCode;
                this.productType = productType;
                this.paymentMethod = paymentMethod;
                
            }

            private string policyNumber = string.Empty; 
            public string PolicyNumber { get { return policyNumber; } }

            private string lob = string.Empty;
            public string LOB { get { return lob; } }

            private string productType = string.Empty;
            public string ProductType { get { return productType; } }

            private string planName = string.Empty;
            public string PlanName { get { return planName; } }

            private string policyStatus = string.Empty;
            public string PolicyStatus { get { return policyStatus; } }

            private string jurisdiction = string.Empty;
            public string Jurisdiction { get { return jurisdiction; } }
        
            private string productCode = string.Empty;
            public string ProductCode { get { return  productCode; } }

            private string paymentMethod = string.Empty;
            public string PaymentMethod { get { return paymentMethod; } }   

            // List of Life Models 
           private List<LifeModel> lifeModels = new List<LifeModel>();
           public List<LifeModel> LifeModels { get { return lifeModels; } set { lifeModels = value; } }

          // Life Participant Models 
        //  private List<LifeParticipantModel> lifeParticipantModels = new List<LifeParticipantModel>();  
        //  public List<LifeParticipantModel> LifeParticipantModels { get {  return lifeParticipantModels; } set { lifeParticipantModels = value; } }

           // Policy Application Info Model 
          private Policy_ApplicationInfoModel policyApplicationInfoModel = new Policy_ApplicationInfoModel();
          public Policy_ApplicationInfoModel Policy_ApplicationInfoModel { get { return policyApplicationInfoModel; } set { policyApplicationInfoModel = value; } }
        }
    }
}
