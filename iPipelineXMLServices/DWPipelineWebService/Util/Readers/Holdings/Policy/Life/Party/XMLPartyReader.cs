
using DWiPipeline.DataModel;
using DWPipelineWebService.DataModel;
using DWPipelineWebService.DataModel.Holding.ChildrenModels;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Collections.Frozen;
using System.Globalization;
using System.Security.Cryptography.Xml;
using System.Xml.Linq;

namespace DWPipelineWebService.Util
{
    partial class XMLMainReader
    {
        internal partial class XMLLifeReader
        {
            internal partial class XMLPartyReader
            {
                private string id_PartyTypeCode = string.Empty;
                private string id_FullName = string.Empty;
                private string id_Govtid = string.Empty;
                private string id_Govtidc = string.Empty;
                private string id_residenceState = string.Empty;
                private string id_ResidenceCountry = string.Empty;

                public XMLPartyReader(string id_PartyTypeCode, string id_FullName, string id_Govtid, string id_Govtidc, string id_residenceState, string id_ResidenceCountry)
                {
                    this.id_PartyTypeCode = id_PartyTypeCode; ;
                    this.id_FullName = id_FullName;
                    this.id_Govtid = id_Govtid;
                    this.id_Govtidc = id_Govtidc;
                    this.id_residenceState = id_residenceState;
                    this.id_ResidenceCountry = id_ResidenceCountry;
                }

                public List<PartyModel> GetPartyData(IEnumerable<XNode> partyNodes)
                {
                    Utils util = new Utils();
                    List<PartyModel> partyModels = new List<PartyModel>();

                    /********************************************************************************************************
                     PARTY DATA
                    ********************************************************************************************************/
                    foreach (XNode partyNode in partyNodes)
                    {
                        if (partyNode is XElement)
                        {
                            XElement xdn = (XElement)partyNode;

                            // Get ID for Party --> Used to set a relationship
                            bool doesPartyIDExists = util.doesAttributeExists(xdn, "id");
                            string partyID = doesPartyIDExists ? xdn.Attributes().Where(x => x.Name.ToString().ToLower() == "id").First().Value : string.Empty;

                            bool doesPartyTypeCode = util.doesXMLElementExists(xdn, id_PartyTypeCode);
                            bool doesPartyFullName = util.doesXMLElementExists(xdn, id_FullName);

                            if (partyID.Length == 0 && !doesPartyFullName) continue;

                            bool doesPartyGovtID = util.doesXMLElementExists(xdn, id_Govtid);
                            bool doesPartyGovtIDC = util.doesXMLElementExists(xdn, id_Govtidc);
                            bool doesResidenceState = util.doesXMLElementExists(xdn, id_residenceState);
                            bool doesResidenceCountry = util.doesXMLElementExists(xdn, id_ResidenceCountry);

                            string partyTypeCode = doesPartyTypeCode ? xdn.Elements().Where(x => x.Name.LocalName.ToLower() == id_PartyTypeCode.ToLower()).First().Value : string.Empty;
                            string partyFullName = doesPartyFullName ? xdn.Elements().Where(x => x.Name.LocalName.ToLower() == id_FullName.ToLower()).First().Value : string.Empty;
                            string partyGovtID = doesPartyGovtID ? xdn.Elements().Where(x => x.Name.LocalName.ToLower() == id_Govtid.ToLower()).First().Value : string.Empty;
                            string partyGovtIDC = doesPartyGovtIDC ? xdn.Elements().Where(x => x.Name.LocalName.ToLower() == id_Govtidc.ToLower()).First().Value : string.Empty;
                            string partyResidenceState = doesResidenceState ? xdn.Elements().Where(x => x.Name.LocalName.ToLower() == id_residenceState.ToLower()).First().Value : string.Empty;
                            string partyResidenceCountry = doesResidenceCountry ? xdn.Elements().Where(x => x.Name.LocalName.ToLower() == id_ResidenceCountry.ToLower()).First().Value : string.Empty;

                            /*************************************************
                             * Party Model 
                             ***************************************************/
                            PartyModel partyModel = new PartyModel(partyID, partyTypeCode, partyFullName, partyGovtID, partyGovtIDC, partyResidenceState, partyResidenceCountry);

                            /***************************************************
                             * Persons in Party Model
                             ***********************************************/
                            XMLPersonReader personReader = new XMLPersonReader("person", "firstname", "lastname", "martstat", "gender", "birthdate", "age", "citizenship", "birthcountry", "birthjurisdictiontc", "middleName", "prefix", "suffix", "title", "smokerStat");
                            List<PersonModel> personModels = personReader.GetPersonData(xdn);
                            partyModel.Persons = personModels;

                            partyModel.ISAPerson = partyModel.Persons.Count() > 0 ? true : false;

                            /************************************************************************
                             *  Organization\OLifeExtension 
                             ************************************************************************/
                            string id_Main_Organization = "organization";
                            string id_Main_OLifeExtension = "olifeExtension";
                            string id_OrganizationType = "organizationType";

                            IEnumerable<XElement> organizationElements = xdn.Elements().Where(x => x.Name.LocalName.ToLower() == id_Main_Organization.ToLower());
                            IEnumerable<XElement> oLifExtensionOrganizationElements = organizationElements.Elements().Where(x => x.Name.LocalName.ToLower() == id_Main_OLifeExtension.ToLower());

                            OrganizationModel organizationModel = new OrganizationModel();
                            foreach (XElement element in oLifExtensionOrganizationElements)
                            {
                                // Organization Type
                                bool exists_OrganizationType = util.doesXMLElementExists(element, id_OrganizationType);
                                string organizationType = exists_OrganizationType ? oLifExtensionOrganizationElements.Elements().Where(x => x.Name.LocalName.ToLower() == id_OrganizationType.ToLower()).First().Value : string.Empty;
                                organizationModel.OrgannizationType = organizationType;

                                break;
                            }

                            foreach(XElement element in organizationElements)
                            {
                                string id_OrgForm = "OrgForm";
                                bool exists_OrgForm = util.doesXMLElementExists(element, id_OrgForm);
                                string orgForm = exists_OrgForm ? element.Elements().Where(x => x.Name.LocalName.ToLower() == id_OrgForm.ToLower()).First().Value : string.Empty;
                                organizationModel.OrgForm = orgForm;
                            }

                            /******************************************************************************
                             * Party/OLifeExtension Elements 
                             ************************************************************************************/
                            IEnumerable<XElement> oLifeExtensionElements = xdn.Elements().Where(x => x.Name.LocalName.ToLower() == id_Main_OLifeExtension.ToLower());
                            foreach (XElement element in oLifeExtensionElements)
                            {
                                // Social Security Number
                                string id_SSN = "SocialSecurityNumber";
                                bool exists_SSN = util.doesXMLElementExists(element, id_SSN);
                                string ssn = exists_SSN ? element.Elements().Where(x => x.Name.LocalName.ToLower() == id_SSN.ToLower()).First().Value : string.Empty;
                                partyModel.SocialSecurityNumber = ssn;

                                // Tax ID Number 
                                string id_TaxIDNumber = "TaxIDNumber";
                                bool exists_TaxIDNumber = util.doesXMLElementExists(element, id_TaxIDNumber);
                                string tax_IDNumber = exists_TaxIDNumber ? element.Elements().Where(x => x.Name.LocalName.ToLower() == id_TaxIDNumber.ToLower()).First().Value : string.Empty;
                                partyModel.TaxIDNumber = tax_IDNumber;

                                // NIB Number 
                                string id_NIBNumber = "NIBNumber";
                                bool exists_NIBNumber = util.doesXMLElementExists(element, id_NIBNumber);
                                string nibNumber = exists_NIBNumber ? element.Elements().Where(x => x.Name.LocalName.ToLower() == id_NIBNumber.ToLower()).First().Value : string.Empty;
                                partyModel.NIBNumber = nibNumber;

                                // Nationality 
                                string id_Nationality = "Nationality";
                                bool exists_Nationality = util.doesXMLElementExists(element, id_Nationality);
                                string nationality = exists_Nationality ? element.Elements().Where(x => x.Name.LocalName.ToLower() == id_Nationality.ToLower()).First().Value : string.Empty;
                                partyModel.Nationality = nationality;
                            }

                            /**********************************************
                             * Set Organization to Party
                             ************************************************/
                            partyModel.Organization = organizationModel;

                            /*****************************************************************
                             * Address Information
                             *****************************************************************/
                            string id_Address = "address";
                            IEnumerable<XElement> addressElements = xdn.Elements().Where(x => x.Name.LocalName.ToLower() == id_Address.ToLower());

                            foreach (XElement addressElement in addressElements)
                            {
                                string id_AddressTypeCode = "addresstypecode";
                                bool doesAddressTypeCode = util.doesXMLElementExists(addressElement, id_AddressTypeCode);
                                string addressTypeCode = doesAddressTypeCode ? addressElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_AddressTypeCode.ToLower()).First().Value : string.Empty;

                                string id_Line1 = "line1";
                                bool doesLine1 = util.doesXMLElementExists(addressElement, id_Line1);
                                string line1 = doesLine1 ? addressElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_Line1.ToLower()).First().Value : string.Empty;

                                string id_Line2 = "line2";
                                bool doesLine2 = util.doesXMLElementExists(addressElement, id_Line2);
                                string line2 = doesLine2 ? addressElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_Line2.ToLower()).First().Value : string.Empty;

                                string id_Line3 = "line3";
                                bool doesLine3 = util.doesXMLElementExists(addressElement, id_Line3);
                                string line3 = doesLine3 ? addressElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_Line3.ToLower()).First().Value : string.Empty;

                                string id_City = "city";
                                bool doesCity = util.doesXMLElementExists(addressElement, id_City);
                                string city = doesCity ? addressElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_City.ToLower()).First().Value : string.Empty;

                                string id_AddressStateTC = "addressstatetc";
                                bool doesAddressStateTC = util.doesXMLElementExists(addressElement, id_AddressStateTC);
                                string addressStateTC = doesAddressStateTC ? addressElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_AddressStateTC.ToLower()).First().Value : string.Empty;

                                string id_AddressState = "addressState";
                                bool doesAddressState = util.doesXMLElementExists(addressElement, id_AddressState);
                                string addressState = doesAddressState ? addressElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_AddressState.ToLower()).First().Value : string.Empty;

                                string id_Zip = "zip";
                                bool doesZip = util.doesXMLElementExists(addressElement, id_Zip);
                                string zip = doesZip ? addressElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_Zip.ToLower()).First().Value : string.Empty;

                                string id_AddressCountryTC = "addresscountrytc";
                                bool doesAddressCountryTC = util.doesXMLElementExists(addressElement, id_AddressCountryTC);
                                string addressCountryTC = doesAddressCountryTC ? addressElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_AddressCountryTC.ToLower()).First().Value : string.Empty;

                                string id_PostalDropCode = "postalDropCode";
                                bool doesPostalDropCode = util.doesXMLElementExists(addressElement, id_PostalDropCode);
                                string postalDropCode = doesPostalDropCode ? addressElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_PostalDropCode.ToLower()).First().Value : string.Empty;

                                string id_YearsAtAddress = "YearsAtAddress";
                                bool doesYearsAtAddress = util.doesXMLElementExists(addressElement, id_YearsAtAddress);
                                string yearsAtAddress = doesYearsAtAddress ? addressElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_YearsAtAddress.ToLower()).First().Value : string.Empty;
                                decimal yearsAtAddressDecimal = 0;
                                decimal.TryParse(yearsAtAddress, out yearsAtAddressDecimal);

                                /*********************************************************
                                 * Address Model set to Party
                                 *************************************************************/
                                AddressModel addressModel = new AddressModel(addressTypeCode, line1, line2, line3, city, addressStateTC, addressState, zip, addressCountryTC, postalDropCode, yearsAtAddressDecimal);
                                partyModel.Address = addressModel;

                                partyModel.Addresses.Add(addressModel);
                            }

                            /*****************************************************************
                            * Phone Information
                            *****************************************************************/

                            IEnumerable<XElement> phoneElements = xdn.Elements().Where(x => x.Name.LocalName.ToLower() == "phone");
                            foreach (XElement phoneElement in phoneElements)
                            {
                                bool doesPhoneTypeCode = util.doesXMLElementExists(phoneElement, "phoneTypeCode");
                                string phoneTypeCode = doesPhoneTypeCode ? phoneElement.Elements().Where(x => x.Name.LocalName.ToLower() == "phonetypecode").First().Value : string.Empty;

                                bool doesAreaCode = util.doesXMLElementExists(phoneElement, "areacode");
                                string areaCode = doesAreaCode ? phoneElement.Elements().Where(x => x.Name.LocalName.ToLower() == "areacode").First().Value : string.Empty;

                                bool doesDialNumber = util.doesXMLElementExists(phoneElement, "dialnumber");
                                string dialNumberS = doesDialNumber ? phoneElement.Elements().Where(x => x.Name.LocalName.ToLower() == "dialnumber").First().Value : string.Empty;
                                int dialNumber = -1;
                                int.TryParse(dialNumberS, out dialNumber);

                                /*******************************************
                                 *  Phone Type Description 
                                 *********************************************************/
                                string id_PhoneTypeDescription = "phoneTypeDescription";
                                string phoneDescription = string.Empty;

                                IEnumerable<XElement> phoneOLifeExtension = phoneElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_Main_OLifeExtension.ToLower());

                                foreach (XElement xElement in phoneOLifeExtension)
                                {
                                    bool doesPhoneTypeDescription = util.doesXMLElementExists(xElement, id_PhoneTypeDescription);
                                    string phoneTypeDescription = doesPhoneTypeDescription ? xElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_PhoneTypeDescription.ToLower()).First().Value : string.Empty;

                                    if (phoneTypeDescription != string.Empty)
                                        phoneDescription = phoneTypeDescription;
                                }

                                /***************************************************
                                 * Set Phone Model to Party
                                 *******************************************************/
                                PhoneNumberType phoneNumberType = PhoneNumberType.Permanent;
                                string[] phoneDescWords = phoneDescription.Split(' ', ';', ',');
                                foreach (string phoneDescWord in phoneDescWords)
                                {
                                    if (phoneDescWord.ToLower() == "foreign")
                                    {
                                        phoneNumberType = PhoneNumberType.Foreign;
                                        break;
                                    }
                                }

                                PhoneModel phoneModel = new PhoneModel(phoneTypeCode, areaCode, dialNumber.ToString(), phoneNumberType);
                                phoneModel.PhoneTypeDescription = phoneDescription;

                                // Add party phone model in the list
                                partyModel.PhoneModels.Add(phoneModel);
                            }

                            /*****************************************************************
                              * Producer Information
                            *****************************************************************/
                            ProducerModel producerModel = new ProducerModel();
                            string id_producer = "producer";
                            string id_companyProducerID = "CompanyProducerID";
                            string id_CarrierAppointment = "CarrierAppointment";
                            IEnumerable<XElement> producerElements = xdn.Elements().Where(x => x.Name.LocalName.ToLower() == id_producer.ToLower());
                            IEnumerable<XElement> carrierAppointmentElements = producerElements.Elements().Where(x => x.Name.LocalName.ToLower() == id_CarrierAppointment.ToLower());
                            foreach (XElement producerElement in carrierAppointmentElements)
                            {
                                string companyProducerID = util.getXMLElementValue(producerElement, id_companyProducerID);
                                producerModel.CompanyProducerID = companyProducerID;
                                partyModel.ProducerModel = producerModel;
                                break;

                            }
                        

                                /*********************************************
                                 * Producer License Information 
                                 ***********************************************/
                                string id_License = "license";
                                IEnumerable<XElement> licenseElements = producerElements.Where(x => x.Name.LocalName.ToLower() == id_License.ToLower());
                            foreach (XElement licenseElement in licenseElements)
                            {
                                string id_LicenseNumber = "licenseNumber";
                                bool doesLicenseNumber = util.doesXMLElementExists(licenseElement, id_LicenseNumber);
                                string licenseNumber = doesLicenseNumber ? licenseElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_LicenseNumber).First().Value : string.Empty;
                                producerModel.LicenseNumber = licenseNumber;
                                break;
                            }
                            

                            /*****************************************************************
                            * Email Information
                            *****************************************************************/
                            //      IEnumerable<XElement> phoneElements = xdn.Elements().Where(x => x.Name.LocalName.ToLower() == "phone");
                            IEnumerable<XElement> emailElements = xdn.Elements().Where(x => x.Name.LocalName.ToLower() == "emailaddress");
                            foreach (XElement emailElement in emailElements)
                            {
                                string addrline = "addrline";
                                bool doesAddrLine = util.doesXMLElementExists(emailElement, addrline.ToLower());
                                string addrLine = doesAddrLine ? emailElement.Elements().Where(x => x.Name.LocalName.ToLower() == addrline.ToLower()).First().Value : string.Empty;

                                if(addrLine.Length > 0)
                                {
                                    EmailModel emailModel = new EmailModel(addrLine);
                                    partyModel.EmailModel = emailModel;
                                    break;
                                }
                            }

                            /*****************************************************************
                                * Risk Information
                            *****************************************************************/
                            IEnumerable<XElement> riskElements = xdn.Elements().Where(x => x.Name.LocalName.ToLower() == "risk");
                            foreach (XElement riskElement in riskElements)
                            {
                                bool doesDateLastUpdated = util.doesXMLElementExists(riskElement, "datelastupdated");
                                string dateLastUpdatedS = doesDateLastUpdated ? riskElement.Elements().Where(x => x.Name.LocalName.ToLower() == "datelastupdated").First().Value : string.Empty;
                                DateTime dateLastUpdated = new DateTime();
                                DateTime.TryParse(dateLastUpdatedS, out dateLastUpdated);

                                bool doesExistinginsuranceind = util.doesXMLElementExists(riskElement, "existinginsuranceind");
                                string existinginsuranceindS = doesExistinginsuranceind ? riskElement.Elements().Where(x => x.Name.LocalName.ToLower() == "existinginsuranceind").First().Value : string.Empty;
                                bool existinginsurnace = false;
                                bool.TryParse(existinginsuranceindS, out existinginsurnace);

                                bool doesReplacementind = util.doesXMLElementExists(riskElement, "replacementind");
                                string replacementind = doesReplacementind ? riskElement.Elements().Where(x => x.Name.LocalName.ToLower() == "replacementind").First().Value : string.Empty;

                                bool doesRejectionind = util.doesXMLElementExists(riskElement, "rejectionind");
                                string rejectionindS = doesRejectionind ? riskElement.Elements().Where(x => x.Name.LocalName.ToLower() == "rejectionind").First().Value : string.Empty;

                                bool rejectionInd = false;
                                bool.TryParse(rejectionindS, out rejectionInd);

                                /************************************************************
                                 * Set Risk Model to Party
                                 *******************************************************/
                                RiskModel riskModel = new RiskModel(dateLastUpdated, existinginsurnace, replacementind, rejectionInd);
                                partyModel.RiskModel = riskModel;
                            }

                            /***********************************************************
                             * Prior Name = Maiden Name if Applicable 
                             ***************************************************/
                            string id_PriorName = "priorName";
                            IEnumerable<XElement> priorNameElements = xdn.Elements().Where(x => x.Name.LocalName.ToLower() == id_PriorName.ToLower());
                            foreach (XElement element in priorNameElements)
                            {
                                string id_LastName = "lastName";
                                bool doesPriorName = util.doesXMLElementExists(element, id_LastName);
                                string lastName = doesPriorName ? element.Elements().Where(x => x.Name.LocalName.ToLower() == id_LastName.ToLower()).First().Value : string.Empty;

                                if (lastName.Length > 0)
                                {
                                    PriorNameModel priorNameModel = new PriorNameModel(lastName);
                                    partyModel.PriorNameModel = priorNameModel;
                                }
                            }

                            /************************************************************
                            * Government Information
                            ***********************************************************/
                            string id_GovtIDInfo = "govtidinfo";
                            IEnumerable<XElement> governmentElements = xdn.Elements().Where(x => x.Name.LocalName.ToLower() == id_GovtIDInfo);
                            foreach (XElement governmentElement in governmentElements)
                            {
                                bool doesPartyGovtID_Exists = util.doesXMLElementExists(governmentElement, id_Govtid);
                                string partyGovtID2 = doesPartyGovtID_Exists ? governmentElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_Govtid.ToLower()).First().Value : string.Empty;

                                string id_GovtExpDate = "GovtIDExpDate";
                                bool doesPartyGovtExpDate_Exists = util.doesXMLElementExists(governmentElement, id_GovtExpDate);
                                string govtExpDate = doesPartyGovtExpDate_Exists ? governmentElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_GovtExpDate.ToLower()).First().Value : string.Empty;
                                DateTime govtExpDateDate = new DateTime();
                                DateTime.TryParse(govtExpDate, out govtExpDateDate);

                                string id_Nation = "Nation";
                                bool doesPartyGovtNation_Exists = util.doesXMLElementExists(governmentElement, id_Nation);
                                string nation = doesPartyGovtNation_Exists ? governmentElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_Nation.ToLower()).First().Value : string.Empty;

                                GovernmentInfoModel governmentInfo = new GovernmentInfoModel(partyGovtID2, govtExpDateDate, nation);
                               // partyModel.GovernmentInfo = governmentInfo;
                               partyModel.GovernmentInfoModels.Add(governmentInfo);
           
                            }


                            /*****************************************************************
                                * Employment Information
                            *****************************************************************/
                            XMLEmploymentReader employmentReader = new XMLEmploymentReader("employment", "annualSalary", "employername", "occupation", "yearsAtEmployment");
                                EmploymentModel employmentModel = employmentReader.GetEmploymentData(xdn);
                                partyModel.EmploymentModel = employmentModel;

                            /******************************************************
                             * Add List of Party Models 
                             **********************************************************/
                            if (partyID.Length > 0)
                            {
                                partyModels.Add(partyModel);
                            }
                           
                        }
                    }

                    return partyModels;
                }
            }
        }
    }
}
