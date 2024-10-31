
using DWiPipeline.DataModel;
using DWPipelineWebService.DataModel;
using DWPipelineWebService.DataModel.Holding;
using System.Xml.Linq;

namespace DWPipelineWebService.Util
{
    partial class XMLMainReader
    {
        internal partial class XMLHoldingReader
        {
            internal partial class XMLPolicyReader
            {
                internal partial class XMLLifeReader
                {
                    internal partial class XMLCoverageReader
                    {
                        private string id_ParentElement = string.Empty;
                        private string id_Base = string.Empty;
                        private string id_CurrentAmt = string.Empty;
                        private string id_InitCovtAmt = string.Empty;
                        private string id_ProductCode = string.Empty;
                        private string id_IndicatorCode = string.Empty;

                        public XMLCoverageReader(string id_ParentElement, string id_Base, string id_CurrentAmt, string id_ProductCode, string id_InitCovtAmt, string id_IndicatorCode)
                        {
                            this.id_ParentElement = id_ParentElement;
                            this.id_Base = id_Base;
                            this.id_CurrentAmt = id_CurrentAmt;
                            this.id_InitCovtAmt = id_InitCovtAmt;
                            this.id_ProductCode = id_ProductCode;
                            this.id_IndicatorCode = id_IndicatorCode;
                        }

                        public List<CoverageModel> GetCoverageData(IEnumerable<XElement> lifeRequestElements)
                        {
                            Utils util = new Utils();
                            List<CoverageModel> coverageModels = new List<CoverageModel>();

                            /********************************************************************************************************
                             COVERAGE DATA
                            ********************************************************************************************************/
                            IEnumerable<XElement> coverageElements = lifeRequestElements.Descendants().Where(x => x.Name.LocalName.ToLower() == id_ParentElement);
                            IEnumerable<XNode> coverageNodes = from coverageNode in coverageElements.DescendantNodesAndSelf() select coverageNode;

                            // Add Base Coverage
                            foreach (XNode coverageNode in coverageNodes)
                            {
                                if (coverageNode is XElement)
                                {
                                    XElement xdnCoverage = (XElement)coverageNode;

                                    // Coverage Base ID 
                                    bool existsCoverageBaseID = util.doesAttributeExists(xdnCoverage, id_Base);
                                    string baseID = existsCoverageBaseID ? xdnCoverage.Attributes().Where(x => x.Name.LocalName.ToLower() == id_Base).First().Value : string.Empty;

                                    if (existsCoverageBaseID)
                                    {
                                        // TC Code 
                                        string id_TC = "tc";

                                        // Current Amount 
                                        bool exists_currentAmt = util.doesXMLElementExists(xdnCoverage, id_CurrentAmt);
                                        string currentAmt = exists_currentAmt ? xdnCoverage.Elements().Where(x => x.Name.LocalName.ToLower() == id_CurrentAmt).First().Value : string.Empty;
                                        decimal currentAmtDecimal = 0;
                                        decimal.TryParse(currentAmt, out currentAmtDecimal);

                                        // Indicator Code 
                                        bool existsIndicatorCode = util.doesXMLElementExists(xdnCoverage, id_IndicatorCode);
                                        string indicatorCode = existsIndicatorCode ? xdnCoverage.Elements().Where(x => x.Name.LocalName.ToLower() == id_IndicatorCode.ToLower()).First().Value : string.Empty;
                                        string tcCode = string.Empty;
                                        if(existsIndicatorCode)
                                        {
                                           XElement indicatorXML =  xdnCoverage.Elements().Where(x => x.Name.LocalName.ToLower() == id_IndicatorCode.ToLower()).First();

                                            if (indicatorXML.FirstAttribute != null)
                                            {
                                                 tcCode = indicatorXML.FirstAttribute.Value;
                                            }
                                        }

                                        // Product Code 
                                        bool exists_productCode = util.doesXMLElementExists(xdnCoverage, id_ProductCode);
                                        string productCode = exists_productCode ? xdnCoverage.Elements().Where(x => x.Name.LocalName.ToLower() == id_ProductCode).First().Value : string.Empty;

                                        // Initial Covt Amount 
                                        bool exists_initCovtAmt = util.doesXMLElementExists(xdnCoverage, id_InitCovtAmt);
                                        string initialCovtAmt = exists_initCovtAmt ? xdnCoverage.Elements().Where(x => x.Name.LocalName.ToLower() == id_InitCovtAmt).First().Value : string.Empty;
                                        decimal currentCovtAmtDecimal = 0;
                                        decimal.TryParse(initialCovtAmt, out currentCovtAmtDecimal);

                                        /********************************************************
                                          Life Participant Readers
                                         ************************************************************/
                                        XMLLifeParticipantReader lifeParticipantReader = new XMLLifeParticipantReader("lifeparticipant", "PartyID", "lifeparticipantrolecode", "issueage", "issuegender", "tobaccopremiumbasis", "underwritingclass", "smokerStat");
                                        List<LifeParticipantModel> lifeParticipantModels = lifeParticipantReader.GetLifeParticipantData(xdnCoverage);

                                        /***************************************************************
                                         * OLifeExtension
                                         **************************************/
                                        string id_OLifeExtension = "OLifEExtension";
                                        IEnumerable<XElement> OLifeExtensionElements = xdnCoverage.Descendants().Where(x => x.Name.LocalName.ToLower() == id_OLifeExtension.ToLower());

                                        string purpose_Insurance_Family = string.Empty;
                                        string purpose_Insurance_Loan = string.Empty;
                                        string purpose_Insurance_Other = string.Empty;

                                        foreach (XNode lifeNode in OLifeExtensionElements)
                                        {
                                            if (lifeNode is XElement)
                                            {
                                                XElement xdnLife = (XElement)lifeNode;

                                                string id_PurposeInsuranceFamily = "PurposeInsuranceFamily";
                                                bool does_PurposeInsurance_Family_Exists = util.doesXMLElementExists(xdnLife, id_PurposeInsuranceFamily);
                                                purpose_Insurance_Family = does_PurposeInsurance_Family_Exists ? xdnLife.Elements().Where(x => x.Name.LocalName.ToLower() == id_PurposeInsuranceFamily.ToLower()).First().Value : string.Empty;

                                                string id_PurposeInsuranceLoan = "PurposeInsuranceLoan";
                                                bool does_id_PurposeInsuranceLoan_Exists = util.doesXMLElementExists(xdnLife, id_PurposeInsuranceLoan);
                                                purpose_Insurance_Loan = does_id_PurposeInsuranceLoan_Exists ? xdnLife.Elements().Where(x => x.Name.LocalName.ToLower() == id_PurposeInsuranceLoan.ToLower()).First().Value : string.Empty;

                                                string id_PurposeInsuranceOther = "PurposeInsuranceOther";
                                                bool does_id_PurposeInsuranceOther_Exists = util.doesXMLElementExists(xdnLife, id_PurposeInsuranceOther);
                                                purpose_Insurance_Other = does_id_PurposeInsuranceOther_Exists ? xdnLife.Elements().Where(x => x.Name.LocalName.ToLower() == id_PurposeInsuranceOther.ToLower()).First().Value : string.Empty;

                                            }
                                        }

                                        bool purpose_Insurance_Family_Bool = false;
                                        bool purpose_Insurance_Loan_Bool = false;
                                        bool purpose_Insurance_Other_Bool = false;

                                        bool.TryParse(purpose_Insurance_Other, out purpose_Insurance_Other_Bool);
                                        bool.TryParse(purpose_Insurance_Loan, out purpose_Insurance_Loan_Bool);
                                        bool.TryParse(purpose_Insurance_Family, out purpose_Insurance_Family_Bool);

                                        PurposeCodes purpose = new PurposeCodes(purpose_Insurance_Family_Bool, purpose_Insurance_Loan_Bool, purpose_Insurance_Other_Bool);

                                        /*****************************************
                                          Coverage Model
                                         *****************************************/
                                       CoverageModel coverageModel = new CoverageModel(baseID, currentAmtDecimal, currentCovtAmtDecimal, productCode, indicatorCode, lifeParticipantModels, purpose, tcCode);
                                        coverageModels.Add(coverageModel);

                                        /**************************************************
                                         * Additional Coverages for the Base Coverage
                                         ***************************************************/
                                        foreach (XElement coverageElement in coverageElements)
                                        {
                                            // Coverage Base ID 
                                            existsCoverageBaseID = util.doesAttributeExists(coverageElement, id_Base);
                                            string child_baseID = existsCoverageBaseID ? coverageElement.Attributes().Where(x => x.Name.LocalName.ToLower() == id_Base).First().Value : string.Empty;
                                            if (child_baseID.ToLower() == baseID.ToLower()) continue; // Only looking for additional coverages that is an extension of the base coverage

                                            // Current Amount 
                                            exists_currentAmt = util.doesXMLElementExists(coverageElement, id_CurrentAmt);
                                            currentAmt = exists_currentAmt ? coverageElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_CurrentAmt).First().Value : string.Empty;
                                            currentAmtDecimal = 0;
                                            decimal.TryParse(currentAmt, out currentAmtDecimal);

                                            // Indicator Code 
                                             existsIndicatorCode = util.doesXMLElementExists(coverageElement, id_IndicatorCode);
                                             indicatorCode = existsIndicatorCode ? coverageElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_IndicatorCode.ToLower()).First().Value : string.Empty;

                                            // Product Code 
                                            exists_productCode = util.doesXMLElementExists(coverageElement, id_ProductCode);
                                            productCode = exists_productCode ? coverageElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_ProductCode).First().Value : string.Empty;

                                            // Addtional Coverages for the main Coverage
                                            AdditionalCoverages addtionalCoverage = new AdditionalCoverages(currentAmtDecimal, productCode, indicatorCode);
                                            coverageModel.AddtionalCoverages.Add(addtionalCoverage);                                        
                                        }
                                    }
                                }
                            }

                            return coverageModels;
                        }
                    }
                }
            }     
        }
    }
}