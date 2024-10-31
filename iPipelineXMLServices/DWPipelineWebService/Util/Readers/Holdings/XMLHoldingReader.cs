using System.Xml.Linq;
using DWiPipeline.DataModel;
using DWPipelineWebService.DataModel;

namespace DWPipelineWebService.Util
{
    partial class XMLMainReader
    {
        internal partial class XMLHoldingReader
        {
            private string holdingtypeCode = string.Empty;
            private string holdingStatus = string.Empty;

            public XMLHoldingReader(string holdingtypeCode, string holdingStatus)
            {
                this.holdingtypeCode = holdingtypeCode;
                this.holdingStatus = holdingStatus;
            }

            public Holding GetHoldingData(XElement xdn)
            {
                Utils util = new Utils();
                bool doesTypeCodeExists = util.doesXMLElementExists(xdn, holdingtypeCode);
                bool doesHoldingStatusCodeExists = util.doesXMLElementExists(xdn, holdingStatus);

                Holding holding = new Holding();

                if (xdn.HasElements && doesTypeCodeExists && doesHoldingStatusCodeExists)
                {
                    string v_HoldingTypeCode = string.Empty;
                    XElement holdingTypeCodeElement = xdn.Elements().Where(x => x.Name.LocalName.ToLower() == holdingtypeCode).First();
                    v_HoldingTypeCode = holdingTypeCodeElement.Value;
                    holding.HoldingTypeCode = v_HoldingTypeCode;


                    string v_HoldingStatus = string.Empty;
                    XElement holdingStatusElement = xdn.Elements().Where(x => x.Name.LocalName.ToLower() == holdingStatus).First();
                    v_HoldingStatus = holdingStatusElement.Value;
                    holding.HoldingTypeStatus = v_HoldingStatus;

                    /********************************************************
                     * Populate Policy Data
                     ***********************************************/
                    XMLPolicyReader policyReader = new XMLPolicyReader("policy", "lineofbusiness", "producttype", "productcode", "planname", "policystatus", "jurisdiction", "paymentmethod", "polNumber");
                    List<Holding.Policy> policyData = policyReader.GetPolicyData(xdn);
                    holding.PolicyData = policyData;

                    /********************************************************
                    * Intent Data
                  ***********************************************/
                    XMLIntentReader intentReader = new XMLIntentReader("intent", "ExpenseNeedKey", "expenseNeedTypeCode");
                    IntentModel intentData = intentReader.GetIntentData(xdn);
                    holding.IntentData = intentData;

                    XMLArrangementReader arrangementReader = new XMLArrangementReader("arrangement", "paymentmethod", "arrmode");
                    ArrangementModel arrangementModel = arrangementReader.GetArrangementData(xdn);
                    holding.ArrangementData = arrangementModel;
                   
                }

                return holding;
            }
        }
    }
}
