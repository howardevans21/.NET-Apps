
using DWiPipeline.DataModel;
using DWPipelineWebService.DataModel;
using System.Xml.Linq;

namespace DWPipelineWebService.Util
{
    partial class XMLMainReader
    {
        internal partial class XMLHoldingReader
        {
            internal partial class XMLPolicyReader
            {
                private string id_Policy = string.Empty;
                private string id_LOB = string.Empty;
                private string id_ProductType = string.Empty;
                private string id_ProductCode = string.Empty;
                private string id_PlanName = string.Empty;
                private string id_PolicyStatus = string.Empty;
                private string id_Jurisdiction = string.Empty;
                private string id_PaymentMethod = string.Empty;
                private string id_PolNumber = string.Empty;

                public XMLPolicyReader(string id_Policy, string id_LOB, string id_ProductType, string id_ProductCode, string id_PlanName, string id_PolicyStatus, string id_Jurisdiction, string id_PaymentMethod, string id_PolNumber)
                {
                    this.id_Policy = id_Policy;
                    this.id_LOB = id_LOB;
                    this.id_ProductType = id_ProductType;
                    this.id_ProductCode = id_ProductCode;
                    this.id_PlanName = id_PlanName;
                    this.id_PolicyStatus = id_PolicyStatus;
                    this.id_Jurisdiction = id_Jurisdiction;
                    this.id_PaymentMethod = id_PaymentMethod;
                    this.id_PolNumber = id_PolNumber;
                }

                public List<Holding.Policy> GetPolicyData(XElement xdn)
                {
                    /********************************************************************************************************
                     POLICY DATA
                   ********************************************************************************************************/
                    IEnumerable<XElement> policyElements = xdn.Descendants().Where(x => x.Name.LocalName.ToLower() == id_Policy);
                    IEnumerable<XNode> policyNodes = from holdingNode in policyElements.DescendantNodesAndSelf() select holdingNode;
                    List<Holding.Policy> policyModels = new List<Holding.Policy>();

                    // TO DO: Find Fields in this section Holding/Policy/ApplicationInfo/OLifEExtension/eConsentAcknowledged

                    foreach (XNode policyNode in policyNodes)
                    {
                        if (policyNode is XElement)
                        {
                            XElement xdnPolicy = (XElement)policyNode;
                           

                            Utils util = new Utils();
                            bool exists_LOB = util.doesXMLElementExists(xdnPolicy, id_LOB);
                            bool exists_ProductType = util.doesXMLElementExists(xdnPolicy, id_ProductType);
                            bool exists_ProductCode = util.doesXMLElementExists(xdnPolicy, id_ProductCode);
                            bool exists_PlanName = util.doesXMLElementExists(xdnPolicy, id_PlanName);
                            bool exists_PolicyStatus = util.doesXMLElementExists(xdnPolicy, id_PolicyStatus);
                            bool exists_Jurisdiction = util.doesXMLElementExists(xdnPolicy, id_Jurisdiction);
                            bool exists_PaymentMethod = util.doesXMLElementExists(xdnPolicy, id_PaymentMethod);
                            bool exists_PolicyNumber = util.doesXMLElementExists(xdnPolicy, id_PolNumber);

                            if (exists_LOB)
                            {
                                string lob = exists_LOB ? xdnPolicy.Elements().Where(x => x.Name.LocalName.ToLower() == id_LOB.ToLower()).First().Value : string.Empty;

                                string productType = exists_ProductType ? xdnPolicy.Elements().Where(x => x.Name.LocalName.ToLower() == id_ProductType.ToLower()).First().Value : string.Empty;

                                string productCode = exists_ProductCode ? xdnPolicy.Elements().Where(x => x.Name.LocalName.ToLower() == id_ProductCode.ToLower()).First().Value : string.Empty;

                                string planName = exists_PlanName ? xdnPolicy.Elements().Where(x => x.Name.LocalName.ToLower() == id_PlanName.ToLower()).First().Value : string.Empty;

                                string policyStatus = exists_PolicyStatus ? xdnPolicy.Elements().Where(x => x.Name.LocalName.ToLower() == id_PolicyStatus.ToLower()).First().Value : string.Empty;

                                string jurisdiction = exists_Jurisdiction ? xdnPolicy.Elements().Where(x => x.Name.LocalName.ToLower() == id_Jurisdiction.ToLower()).First().Value : string.Empty;

                                string paymentMethod = exists_PaymentMethod ? xdnPolicy.Elements().Where(x => x.Name.LocalName.ToLower() == id_PaymentMethod.ToLower()).First().Value : string.Empty;

                                string policyNumber = exists_PolicyNumber ? xdnPolicy.Elements().Where(x => x.Name.LocalName.ToLower() == id_PolNumber.ToLower()).First().Value : string.Empty;

                                /******************************************************************
                               * Populate Data in Policy Object Model 
                               **********************************************************/
                                Holding.Policy policyModel = new Holding.Policy(policyNumber, lob, productType, planName, policyStatus, jurisdiction, productCode, paymentMethod);

                                /******************************************************************
                                 * Set Life Models in Policy Model
                                **********************************************************/
                                XMLLifeReader xMLLifeReader = new XMLLifeReader("life", "faceamt", "divtype");
                                List<LifeModel> lifeModels = xMLLifeReader.GetLifeData(xdnPolicy);
                                policyModel.LifeModels = lifeModels;

                                /*************************************************
                                 * Set ApplicationInfo/OLifeExtension 
                                 ********************************************/
                                XML_Policy_ApplicationInfoReader.XML_Policy_OLifeExtensionReader xml_olife_Reader = new XML_Policy_ApplicationInfoReader.XML_Policy_OLifeExtensionReader("ApplicationInfo", "OLifeExtension", "eConsentAcknowledged", "", "PolicyDateRequested", "SpecifiedDay");
                                Policy_ApplicationInfoModel policyAppInfo = xml_olife_Reader.GetPolicyOLifeExtensionData(xdnPolicy);

                                
                                policyModel.Policy_ApplicationInfoModel = policyAppInfo;

                                policyModels.Add(policyModel);
                                /*********************************************
                                  * Break the loop for faster processing
                                  * Already obtained Policy data; no need to look for its descendants
                                  * since that will be in another process
                                  ************************************************/
                                break;
                           }
                        }
                    }

                    return policyModels;
                }
            }
        }
    }
}