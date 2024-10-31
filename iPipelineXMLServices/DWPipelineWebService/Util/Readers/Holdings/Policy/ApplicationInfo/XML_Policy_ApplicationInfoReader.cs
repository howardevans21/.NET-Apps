using DWPipelineWebService.DataModel;
using System.Xml.Linq;

namespace DWPipelineWebService.Util
{
    internal partial class XML_Policy_ApplicationInfoReader
    {
        internal partial class XML_Policy_OLifeExtensionReader
        {
            private string id_Main_ApplicationInfo = string.Empty;
            private string id_Main_OLifeExtension = string.Empty;
            private string id_EConsentAcknowledged = string.Empty;
            private string id_OptOutContact = string.Empty;
            private string id_PolicyDateRequested = string.Empty;
            private string id_SpecifiedDay = string.Empty;
            
            public XML_Policy_OLifeExtensionReader() { }

            public XML_Policy_OLifeExtensionReader(string id_Main, string id_Main_OLifeExtension, string id_EConsentAcknowledged, string id_OptOutContact, string id_PolicyDateRequested, string id_SpecifiedDay)
            {
                this.id_Main_ApplicationInfo = id_Main;
                this.id_Main_OLifeExtension= id_Main_OLifeExtension;
                this.id_EConsentAcknowledged = id_EConsentAcknowledged;
                this.id_OptOutContact = id_OptOutContact;
                this.id_PolicyDateRequested = id_PolicyDateRequested;
                this.id_SpecifiedDay = id_SpecifiedDay;
            }

            public Policy_ApplicationInfoModel GetPolicyOLifeExtensionData(XElement xdn)
            {
                Utils util = new Utils();

                IEnumerable<XElement> applicationInfoReaderElements = xdn.Elements().Where(x => x.Name.LocalName.ToLower() == id_Main_ApplicationInfo.ToLower());
                IEnumerable<XElement> oLifeExtensionElements = applicationInfoReaderElements.Elements().Where(x => x.Name.LocalName.ToLower() == id_Main_OLifeExtension.ToLower());

                Policy_ApplicationInfoModel policyAppInfoModel = new Policy_ApplicationInfoModel();

                foreach (XElement x in oLifeExtensionElements)
                {
                    bool doesEConsentAcknowledged = util.doesXMLElementExists(x, id_EConsentAcknowledged);
                    string eConsentAcknowledged = doesEConsentAcknowledged ? x.Elements().Where(x => x.Name.LocalName.ToLower() == id_EConsentAcknowledged.ToLower()).First().Value : string.Empty;
                    bool eConsentAcknowledgedBool = false;
                    bool.TryParse(eConsentAcknowledged.ToLower(), out eConsentAcknowledgedBool);

                    bool doesOptOutContact = util.doesXMLElementExists(x, id_OptOutContact);
                    string optOutContact = doesOptOutContact ? x.Elements().Where(x => x.Name.LocalName.ToLower() == id_OptOutContact.ToLower()).First().Value : string.Empty;
                    bool optOutContactBool = false;
                    bool.TryParse(optOutContact.ToLower(), out optOutContactBool);

                    // Issue Date Settings
                    bool does_Issue_Policy_Date_RequestedExists = util.doesXMLElementExists(x, id_PolicyDateRequested);
                    string policy_Date_Requested = does_Issue_Policy_Date_RequestedExists ? x.Elements().Where(x => x.Name.LocalName.ToLower() == id_PolicyDateRequested.ToLower()).First().Value : string.Empty;

                    bool does_Issue_SpecifiedDay_Exists = util.doesXMLElementExists(x, id_SpecifiedDay);
                    string specifiedDayS = does_Issue_SpecifiedDay_Exists ? x.Elements().Where(x => x.Name.LocalName.ToLower() == id_SpecifiedDay.ToLower()).First().Value : string.Empty;
                    int specifiedDayInt = -1;
                    int.TryParse(specifiedDayS, out specifiedDayInt);

                    /*******************************************************
                     * Smoker Codes are obtained from UW Class Insured fields
                     **********************************************************/
                    string id_UWClassInsured1 = "UWClassInsured1";
                    bool does_UWClassInsured1Exists = util.doesXMLElementExists(x, id_UWClassInsured1);
                    string uwClassInsured1 = does_UWClassInsured1Exists ? x.Elements().Where(x => x.Name.LocalName.ToLower() == id_UWClassInsured1.ToLower()).First().Value : string.Empty;

                    string id_UWClassInsured2 = "UWClassInsured2";
                    bool does_UWClassInsured2Exists = util.doesXMLElementExists(x, id_UWClassInsured2);
                    string uwClassInsured2 = does_UWClassInsured2Exists ? x.Elements().Where(x => x.Name.LocalName.ToLower() == id_UWClassInsured2.ToLower()).First().Value : string.Empty;

                    // Policy Application Info Model 
                    policyAppInfoModel = new Policy_ApplicationInfoModel(eConsentAcknowledgedBool, optOutContactBool, policy_Date_Requested, specifiedDayInt, uwClassInsured1, uwClassInsured2);

                    break;

                }

                string id_BackDateType = "BackDateType";
                string lookupValue = "backdatetosaveage";
                foreach (XElement x in applicationInfoReaderElements)
                {
                    bool does_BackDateTypeExists = util.doesXMLElementExists(x, id_BackDateType);
                    string backDateType = does_BackDateTypeExists ? x.Elements().Where(x => x.Name.LocalName.ToLower() == id_BackDateType.ToLower()).First().Value : string.Empty;
                    string conditionalBackDateTypeS = backDateType.ToLower().Replace(" ", "");
                    if (conditionalBackDateTypeS == lookupValue) policyAppInfoModel.IssueDate_Type = IssueDateType.BackDateToSaveAge;
                }


                return policyAppInfoModel;
            }
        }
    }
}
