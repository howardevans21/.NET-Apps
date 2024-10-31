using DocuWare.Platform.ServerClient;
using DWiPipeline;
using DWiPipeline.DataModel;
using DWPipelineWebService.DataModel;
using DWPipelineWebService.DataModel.Holding;
using DWPipelineWebService.DataModel.Holding.ChildrenModels;
using DWPipelineWebService.SQLDataModel.StoredProcedureSchema;
using DWPipelineWebService.Test;
using DWPipelineWebService.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics.Eventing.Reader;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Xml;
using System.Xml.Linq;
using static DWiPipeline.DataModel.Holding;

namespace DWPipelineWebService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ColinaController : ControllerBase
    {
        public ColinaController()
        {


        }

        // GET all action

        // GET by Id action

        // POST action

        // PUT action
        //[HttpPost("{xml}")]

        //  [Route("DataUpload")]
        [EnableCors("AllowAll")]
        [HttpPost]
        [Consumes("application/xml")]
        // [Consumes("text/xml")]
        public IActionResult Update([FromBody] XElement xml)
        {
            string policyNumber = string.Empty;
            try
            {
                Utils util = new Utils();
                ReadConfig readConfig = new ReadConfig();
                readConfig.GetSettings();
                using (var eFContext = new EFContext())
                {
                    eFContext.Database.EnsureCreated();

                    /****************************************************************
                     * Save XML in Archive Folder for Recovery in case of system failue
                     *******************************************************************/
                    try
                    {
                        saveXMLArchive(readConfig, xml);
                    } catch { }

                    /***********************************************************/

                    // DocuWare
                    FileCabinet fileCabinet = connectToDocuWare();

                    if (getCurrentUserName().ToLower() == "COLINA\\iis_cdr")
                    {
                        TestModel.Policy_NO = TestModel.GenerateRandomString("123456789", 9);
                    }
                    else
                    {
                        TestModel.Policy_NO = TestModel.GenerateRandomString("ABCDEFGHI", 9);
                    }

                    if (xml.Document == null) return NoContent();
                    XDocument xmlDoc = xml.Document;

                    if (xmlDoc.Root == null) return NoContent();

                    foreach (XNode n in xmlDoc.Root.Nodes())
                    {
                        if (n.Document == null) continue;
                        foreach (XElement xe in n.Document.Elements())
                        {

                            IEnumerable<XElement> lifeRequestElements = xe.Descendants().Where(x => x.Name.LocalName.ToLower() == "olife");

                            IEnumerable<XElement> holdingElements = lifeRequestElements.Descendants().Where(x => x.Name.LocalName.ToLower() == "holding");

                            IEnumerable<XElement> partyElements = lifeRequestElements.Descendants().Where(x => x.Name.LocalName.ToLower() == "party");

                            IEnumerable<XElement> relationElements = lifeRequestElements.Descendants().Where(x => x.Name.LocalName.ToLower() == "relation");

                            IEnumerable<XElement> policyElements = holdingElements.Descendants().Where(x => x.Name.LocalName.ToLower() == "policy");

                            IEnumerable<XNode> dnas = from node in policyElements.DescendantNodesAndSelf() select node;

                            // Get Holdings
                            /***********************************************************************************************************/
                            List<Holding> holdings = new List<Holding>();
                            IEnumerable<XNode> holdingNodes = from holdingNode in holdingElements.DescendantNodesAndSelf() select holdingNode;
                            Holding mainHoldingRef = null;
                            foreach (XElement holdingElement in holdingElements)
                            {
                                XMLMainReader.XMLHoldingReader holdingReader = new XMLMainReader.XMLHoldingReader("holdingtypecode", "holdingstatus");
                                Holding holding = holdingReader.GetHoldingData(holdingElement);
                                if (holding.PolicyData.Count > 0 && holding.PolicyData.First().PolicyNumber.Length > 0)
                                {
                                    holdings.Add(holding);
                                    mainHoldingRef = holding;
                                }
                                else
                                {
                                    if (mainHoldingRef != null)
                                    {
                                        mainHoldingRef.CoverageHolidngs.Add(holding);
                                    }
                                }
                            }

                            // Get Parties
                            /***********************************************************************************************************/
                            IEnumerable<XNode> partyNodes = from partyNode in partyElements.DescendantNodesAndSelf() select partyNode;
                            XMLMainReader.XMLLifeReader.XMLPartyReader partyReader = new XMLMainReader.XMLLifeReader.XMLPartyReader("partytypecode", "fullname", "govtid", "govtidc", "residencestate", "residenceCountry");
                            List<PartyModel> partyModels = partyReader.GetPartyData(partyNodes);

                            // Get Relations 
                            /***********************************************************************************************************/
                            IEnumerable<XNode> relationNodes = from relationNode in relationElements.DescendantNodesAndSelf() select relationNode;
                            XMLMainReader.XMLLifeReader.XMLRelationReader relationReader = new XMLMainReader.XMLLifeReader.XMLRelationReader("Relation", "id", "OriginatingObjectID", "OriginatingObjectType", "RelationRoleCode", "InterestPercent", "IrrevokableInd", "RelationDescription", "RelatedObjectID", "RelatedObjectType", "DistributionOption");
                            List<RelationModel> relationModels = relationReader.GetRelationData(relationNodes);

                            // Save Data
                            /***********************************************************************************************************/
                            saveData(holdings, partyModels, relationModels);

                            // Get Documents 
                            /***********************************************************************************************************/
                            IEnumerable<XNode> lifeNodes = from lifeNode in lifeRequestElements.DescendantNodesAndSelf() select lifeNode;
                            foreach (XNode lifeNode in lifeNodes)
                            {
                                if (lifeNode is XElement)
                                {
                                    XElement xdn = (XElement)lifeNode;

                                    if (xdn.HasElements)
                                    {
                                        IEnumerable<XElement> formElements = xdn.Elements().Where(x => x.Name.LocalName.ToLower() == "forminstance");
                                        if (!formElements.Any()) { continue; } // Skip if there aren't any form elements 
                                        XMLMainReader.XMLAttachmentReader attachmentReader = new XMLMainReader.XMLAttachmentReader("attachment", "attachmentbasictype", "attachmentdata", "imagetype", "description");
                                        List<AttachmentModel> attachments = attachmentReader.GetDocumentData(formElements).Distinct().ToList();

                                        if (holdings.Count > 0)
                                        {
                                            Holding holding = holdings.First();
                                            Holding.Policy policy = holding.PolicyData.First();
                                            policyNumber = policy.PolicyNumber;

                                            /*******************************************************
                                             * Save XML in Policy Folder for recovery in
                                             * case of system failure
                                             *********************************************************/
                                            try
                                            {
                                                saveXMLArchivePolicy(readConfig, xml, policyNumber);
                                            }
                                            catch { }

                                            /*********************************************
                                               * Create Companion file to index fields in DocuWare
                                            ***********************************************/

                                            string relationRoleCode = getRelationRoleCode(InsuredType.Insured);
                                            List<ClientModel> clientModels = getClients(partyModels, relationModels);
                                            if (clientModels.Count > 0)
                                            {

                                                // Get the Primary Owner if it exists
                                                ClientModel clientModel = clientModels.Where(x => x.ClientType == ClientType.PrimaryInsured).FirstOrDefault();

                                                /**********************************************
                                                 * Process Documents for DocuWare Integration
                                                 *******************************************/
                                                foreach (AttachmentModel attachment in attachments)
                                                {
                                                    if (attachment.WhichAttachmentName == WhichAttachmentName.FormName)
                                                        attachment.ConvertedFormInstanceName = translateText(eFContext, attachment.FormInstanceName, TranslationType.IP_DOCUWARETYPE, false);
                                                    else attachment.ConvertedFormInstanceName = attachment.Document_Type;

                                                    // Check for blank Doc Types
                                                    if (attachment.ConvertedFormInstanceName.Length <= 0) attachment.ConvertedFormInstanceName = "Other";

                                                    attachment.CreateDataFiles(policy, clientModel, fileCabinet);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string innerException = ex.InnerException != null ? ex.InnerException.Message : string.Empty;
                string stackTrace = ex.StackTrace != null ? ex.StackTrace : string.Empty;
                string errorMessage = string.Format("{0}\r\n\r\n{1}\\r\n\r\n{2}", ex.Message, ex.InnerException, stackTrace);

                saveErrorData(errorMessage, policyNumber, "WS");
                return StatusCode(StatusCodes.Status500InternalServerError, errorMessage);
            }
            // Response for successful completion
            return Ok("XML received successfully");
            // return NoContent();
        }

        private void saveErrorData(string errorMessage, string policyID, string appID)
        {
            using (var eFContext = new EFContext())
            {
                eFContext.Database.EnsureCreated();
                TERROR errorRecord = new TERROR(appID, policyID, errorMessage, DateTime.Now);
                eFContext.Add(errorRecord);
                eFContext.SaveChanges();
            }
        }


        private void saveData(List<Holding> holdings, List<PartyModel> partyModels, List<RelationModel> relationModels)
        {
            using (var eFContext = new EFContext())
            {
                eFContext.Database.EnsureCreated();
                savePolicyData(eFContext, holdings, partyModels, relationModels);
                saveClientData(eFContext, holdings, partyModels, relationModels);
                saveBeneficiaryData(eFContext, holdings, partyModels, relationModels, InsuredType.Beneficiary);
            }
        }

        private void saveClientData(EFContext context, List<Holding> holdings, List<PartyModel> partyModels, List<RelationModel> relationModels)
        {

            if (holdings.Count == 0) { return; }
            Holding holding = holdings.First();
            Holding.Policy policy = holding.PolicyData.First();
            saveCoverageDataHelper(context, holding, policy, relationModels);

            // Save Primary Insured information 
            saveInsuredTypes(context, holdings, partyModels, relationModels, InsuredType.Insured);

            // Save Joint insured information 
            saveInsuredTypes(context, holdings, partyModels, relationModels, InsuredType.JointInsured);

            // Save owner information
            saveInsuredTypes(context, holdings, partyModels, relationModels, InsuredType.Owner);
        }

        private void saveBeneficiaryData(EFContext context, List<Holding> holdings, List<PartyModel> partyModels, List<RelationModel> relationModels, InsuredType insuredType)
        {

            Util.Utils util = new Utils();

            if (holdings.Count == 0) { return; }
            Holding holding = holdings.First();
            Holding.Policy policy = holding.PolicyData.First();

            string relationRoleCode = getRelationRoleCode(insuredType);

            List<TBENE> equallyDistributedBeneficiaryRecords = saveDataBasedOnParticipantRoleCode(InsuredType.Beneficiary, context, holding, relationModels, partyModels, policy);


            /**************************************************************
             * Calculate equally distributed beneficiaries if applicable
             *************************************************************/
            if (equallyDistributedBeneficiaryRecords.Count > 0)
            {
                int numberOfEQDistrRecords = equallyDistributedBeneficiaryRecords.Count;

                // Check if total count is divisable by 100
                // If 0 it fits perfectly where each percentage will be the same
                decimal divisiableNumber = 100 % numberOfEQDistrRecords;
                if (divisiableNumber == 0)
                {
                    decimal equalDivider = 100M / numberOfEQDistrRecords;
                    equalDivider = util.TruncateDecimal(equalDivider, 2) / 100;

                    // Update beneficiary records beneficiary percentage in the TBENE table
                    // Percentages are equal
                    foreach (TBENE tBene in equallyDistributedBeneficiaryRecords)
                    {
                        tBene.BNFY_PRCDS_PCT = equalDivider;
                        context.SaveChanges();
                    }
                }
                else
                {
                    decimal equalDivider = 100M / numberOfEQDistrRecords;
                    equalDivider = util.TruncateDecimal(equalDivider, 2);
                    decimal totalPercentageCalculation = Decimal.Multiply(equalDivider, 3);
                    decimal remainder = 100M - totalPercentageCalculation;
                    decimal unevenPercentage = equalDivider + remainder;
                    int counter = 0;
                    foreach (TBENE tBENE in equallyDistributedBeneficiaryRecords)
                    {
                        if (counter > 0) tBENE.BNFY_PRCDS_PCT = equalDivider;
                        else tBENE.BNFY_PRCDS_PCT = unevenPercentage;

                        context.SaveChanges();
                        counter++;
                    }
                }
            }

        }

        private void savePolicyData(EFContext context, List<Holding> holdings, List<PartyModel> partyModels, List<RelationModel> relationModels)
        {
            if (holdings.Count == 0) { return; }
            Holding holding = holdings.First();
            Holding.Policy policy = holding.PolicyData.First();

            // Get Contingent Owner 
            ContingentOwnerModel contingentOwner = getContingentOwner(relationModels, partyModels);

            // Get Owner 
            OwnerModel ownerModel = getPrimaryOwner(relationModels, partyModels);
            PartyModel primaryInsuredModel = getPrimaryInsured(partyModels);

            // Agent Relation Models 
            List<AgentModel> agents = getAgentPartyModels(relationModels, partyModels);
            savePolicyDataHelper(context, holding, policy, contingentOwner, agents, ownerModel, primaryInsuredModel);
        }

        private bool checkIfPartyModelWasAlreadyAdded(List<RelationModel> relationModels, PartyModel partyModel, List<PartyModel> partyModels)
        {
            bool istheOwnerPartyExists = false;

            List<PartyModel> insuredPartyModels = new List<PartyModel>();
            string relationRoleCode = getRelationRoleCode(InsuredType.Insured).ToLower().Trim().Replace(" ", "");
            string joinInsuredRoleCode = getRelationRoleCode(InsuredType.JointInsured).ToLower().Trim().Replace(" ", "");

            // Check if Primary Insured and Joint Insured Has the Owner Party Model
            List<RelationModel> insuredList = relationModels.Where(x => x.RelationRoleCode.ToLower().Trim().Replace(" ", "") == relationRoleCode
                        ||  x.RelationRoleCode.ToLower().Trim().Replace(" ", "") == joinInsuredRoleCode ).ToList();

            foreach (RelationModel relationModel in insuredList)
            {
                 insuredPartyModels = partyModels.Where(x => x.PartyID.ToLower() == relationModel.RelatedObjectID.ToLower()).ToList();

                 foreach(PartyModel model in insuredPartyModels)
                {
                    if(model.PartyID.ToLower() == partyModel.PartyID.ToLower())
                    {
                        istheOwnerPartyExists = true;
                        break;
                    }
                }
            }

            return istheOwnerPartyExists;
        }

        private void saveInsuredTypes(EFContext context, List<Holding> holdings, List<PartyModel> partyModels, List<RelationModel> relationModels, InsuredType insuredType)
        {
            if (holdings.Count == 0) { return; }
            Holding holding = holdings.First();
            Holding.Policy policy = holding.PolicyData.First();

            string relationRoleCode = getRelationRoleCode(insuredType);
            List<RelationModel> insuredList = relationModels.Where(x => x.RelationRoleCode.ToLower().Trim().Replace(" ", "") == relationRoleCode.ToLower().Trim().Replace(" ", "")).ToList();

            
            foreach (RelationModel relationModel in insuredList)
            {
                // Get Party information based on relation model 
                List<PartyModel> insuredPartyModels = partyModels.Where(x => x.PartyID.ToLower() == relationModel.RelatedObjectID.ToLower()).ToList();

                foreach (PartyModel partyModel in insuredPartyModels)
                {
                    // If the owner == primary insured or joint insured
                    // Skip party model to prevent data being the same if the insured type is different in the lookup process
                    if (insuredType == InsuredType.Owner)
                    {
                        bool skipParty = checkIfPartyModelWasAlreadyAdded(relationModels, partyModel, partyModels);
                        if (skipParty) continue;
                    }

                    if (partyModel.Persons.Count > 0)
                    {
                        foreach (PersonModel personModel in partyModel.Persons)
                        {
                            saveClientDataHelper(context, holding, policy, personModel, insuredType, partyModel, relationModel);
                            saveClientAddressDataHelper(context, holding, policy, personModel, partyModel, insuredType);
                            saveDocumentDataHelper(context, holding, policy, personModel, partyModel, insuredType);
                        }
                    }
                    else
                    {
                        if (insuredType == InsuredType.Owner)
                        {
                            OwnerModel ownerModel = getPrimaryOwner(relationModels, partyModels);
                            PersonModel personOwnerModel = new PersonModel(ownerModel.PartyFulllName, string.Empty, string.Empty, -1, DateTime.Now.Date, string.Empty, ownerModel.Country, string.Empty, string.Empty, string.Empty, string.Empty, "C", string.Empty, string.Empty, false);

                            ContingentOwnerModel contingentOwnerModel = getContingentOwner(relationModels, partyModels);
                            saveClientDataHelper(context, holding, policy, personOwnerModel, insuredType, partyModel, relationModel);
                            saveClientAddressDataHelper(context, holding, policy, personOwnerModel, ownerModel.PartyData, insuredType);
                            saveDocumentDataHelper(context, holding, policy, personOwnerModel, ownerModel.PartyData, insuredType);
                        }
                    }
                }
            }
        }

        private void getDocuWareDocInfo(Holding.Policy policy, List<PartyModel> partyModels, List<RelationModel> relationModels)
        {
            string relationRoleCode = getRelationRoleCode(InsuredType.Insured);
            // Agent Relation Models 
            List<AgentModel> agents = getAgentPartyModels(relationModels, partyModels);
            List<RelationModel> insuredList = relationModels.Where(x => x.RelationRoleCode.ToLower().Trim().Replace(" ", "") == relationRoleCode.ToLower().Trim().Replace(" ", "")).ToList();
        }

        private List<TBENE> saveDataBasedOnParticipantRoleCode(InsuredType insuredType, EFContext context, Holding holding, List<RelationModel> relationModels, List<PartyModel> partyModels, Policy policy)
        {
            string specificRoleCodeToFind = string.Empty;

            List<TBENE> beneficiaryEquallyDistributedRecords = new List<TBENE>();

            switch (insuredType)
            {
                case InsuredType.Beneficiary:
                    specificRoleCodeToFind = "Beneficiary";
                    break;
            }

            if (policy.LifeModels != null && policy.LifeModels.Count > 0)
            {
                LifeModel lifeModel = policy.LifeModels.First();
                if (lifeModel.CoverageModels != null && lifeModel.CoverageModels.Count > 0)
                {
                    CoverageModel coverageModel = lifeModel.CoverageModels.First();

                    foreach (LifeParticipantModel lifeParticipantModel in coverageModel.LifeParticipants)
                    {
                        if (lifeParticipantModel.LifeParticipantRoleCode.Contains(specificRoleCodeToFind))
                        {
                            List<RelationModel> lifeParticipantRelationModels = relationModels.Where(x => x.RelatedObjectID.ToLower().Trim().Replace(" ", "") == lifeParticipantModel.PartyID.ToLower().Trim().Replace(" ", "") && x.RelationRoleCode.Contains(specificRoleCodeToFind)).ToList();

                            foreach (RelationModel lifeParticipantRelationModel in lifeParticipantRelationModels)
                            {
                                List<PartyModel> lifeParticipantPartyModels = partyModels.Where(x => x.PartyID.ToLower().Trim().Replace(" ", "") == lifeParticipantRelationModel.RelatedObjectID.ToLower().Trim().Replace(" ", "")).ToList();

                                foreach (PartyModel lifeParticipantPartyModel in lifeParticipantPartyModels)
                                {
                                    switch (insuredType)
                                    {
                                        case InsuredType.Beneficiary:

                                            bool doesPrimaryBeneficiaryRelationExists = relationModels.Where(x => x.RelatedObjectID.ToLower().Trim() == lifeParticipantModel.PartyID.ToLower().Trim() && !x.RelationRoleCode.Contains(specificRoleCodeToFind)).ToList().Count > 0 ? true : false;
                                            bool doesBeneficiaryRelationExists = relationModels.Where(x => x.RelatedObjectID.ToLower().Trim() == lifeParticipantModel.PartyID.ToLower().Trim() && x.RelationRoleCode.Contains(specificRoleCodeToFind)).ToList().Count > 0 ? true : false;

                                            if (doesPrimaryBeneficiaryRelationExists && doesBeneficiaryRelationExists)
                                            {
                                                RelationModel primaryBeneficiaryRelation = relationModels.Where(x => x.RelatedObjectID.ToLower().Trim() == lifeParticipantModel.PartyID.ToLower().Trim().Replace(" ", "") && !x.RelationRoleCode.Contains(specificRoleCodeToFind)).First();
                                                RelationModel beneficiaryRelation = relationModels.Where(x => x.RelatedObjectID.ToLower().Trim() == lifeParticipantModel.PartyID.ToLower().Trim().Replace(" ", "") && x.RelationRoleCode.Contains(specificRoleCodeToFind)).First();
                                                TBENE beneficiaryRecord = saveBeneficiaryDataHelper(context, holding, policy, lifeParticipantPartyModel, insuredType, primaryBeneficiaryRelation, beneficiaryRelation, relationModels, partyModels);
                                                if (beneficiaryRecord != null) beneficiaryEquallyDistributedRecords.Add(beneficiaryRecord);
                                            }
                                            else if (doesBeneficiaryRelationExists)
                                            {
                                                RelationModel beneficiaryRelation = relationModels.Where(x => x.RelatedObjectID.ToLower().Trim() == lifeParticipantModel.PartyID.ToLower().Trim().Replace(" ", "") && x.RelationRoleCode.Contains(specificRoleCodeToFind)).First();

                                                TBENE beneficiaryRecord = saveBeneficiaryDataHelper(context, holding, policy, lifeParticipantPartyModel, insuredType, beneficiaryRelation, beneficiaryRelation, relationModels, partyModels);
                                                if (beneficiaryRecord != null) beneficiaryEquallyDistributedRecords.Add(beneficiaryRecord);
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return beneficiaryEquallyDistributedRecords;
        }

        private void saveAdditionalCoveragesHelper(EFContext context, List<AdditionalCoverages> additionalCoverages, string planID, Holding.Policy policy, int issueAge, string coverage_cli_typ_cd)
        {

            /**********************************************************************
                       * Save Child Base Coverages
              *********************************************************/
            foreach (AdditionalCoverages addtionalCoverage in additionalCoverages)
            {
                string addtionalCoveragePlanID = string.Empty;
                string planAgeRange = string.Empty;

                if (addtionalCoverage.ProductCode.ToUpper() == "ADD")
                {
                    if (issueAge >= 41) planAgeRange = "65";
                    else if (issueAge > 18 && issueAge <= 40) planAgeRange = "40";
                    else planAgeRange = "17";

                    addtionalCoveragePlanID = string.Format("{0}{1}{2}", planID, addtionalCoverage.ProductCode, planAgeRange);
                }
                else
                {
                    addtionalCoveragePlanID = addtionalCoverage.ProductCode;
                }

                addtionalCoveragePlanID = translateText(context, addtionalCoveragePlanID, TranslationType.IP_PLAN_CD, false);
                addtionalCoveragePlanID = addtionalCoveragePlanID.Trim();

                TCVG additionalCoverageRecord = new TCVG(policy.PolicyNumber, coverage_cli_typ_cd, addtionalCoveragePlanID, getCurrentUserName(), DateTime.Now, DateTime.Now);
                additionalCoverageRecord.CVG_FACE_AMT = addtionalCoverage.CurrentAmt > 0 ? addtionalCoverage.CurrentAmt : 0;

                bool isDuplicateAddtionalCoverage = checkForDuplicate(context, additionalCoverageRecord);

                if (!isDuplicateAddtionalCoverage)
                {
                    // Save Coverage Record 
                    context.Add(additionalCoverageRecord);
                    context.SaveChanges();
                }
            }

        }


        #region Coverage Data 

        private void saveCoverageDataHelper(EFContext context, Holding holding, Holding.Policy policy, List<RelationModel> allRelationModels)
        {
            /*************************************************************
             * Save Main Holding Coverage and Additional Coverages
             *************************************************************/
            if (holding.PolicyData != null && holding.PolicyData.Count > 0 && holding.PolicyData.First().LifeModels != null

                && holding.PolicyData.First().LifeModels.Count > 0)
            {
                List<CoverageModel> parentCoverageModels = holding.PolicyData.First().LifeModels.First().CoverageModels.Distinct().ToList();
                int issueAge = -1;

                foreach (CoverageModel parentCoverageModel in parentCoverageModels)
                {
                    string planID = string.Empty;
                    string coverage_cli_typ_cd = string.Empty;
                    planID = policy.ProductCode.Length > 0 ? policy.ProductCode : parentCoverageModel.ProductCode;
                    LifeModel lifeModel = policy.LifeModels.First();

                    foreach (LifeParticipantModel lifeParticipant in parentCoverageModel.LifeParticipants)
                    {
                        if (lifeParticipant.IssueAge > -1) issueAge = lifeParticipant.IssueAge;

                        List<RelationModel> coverageRelationModels = allRelationModels.Where(x => x.RelatedObjectID.ToLower() == lifeParticipant.PartyID.ToLower()).ToList();
                        foreach (RelationModel coverageRelationModel in coverageRelationModels)
                        {
                            InsuredType coverageInsuredType = getInsuredType(coverageRelationModel.RelationRoleCode);
                            if (coverageInsuredType != InsuredType.None)
                            {
                                coverage_cli_typ_cd = getRelationRoleCodeConverter(coverageInsuredType);
                                break;
                            }
                        }

                        // Translate Plan ID 
                        string coverage_PlanID = translateText(context, planID, TranslationType.IP_PLAN_CD, false);
                        coverage_PlanID = coverage_PlanID.Trim();

                        TCVG coverageRecord = new TCVG(policy.PolicyNumber, coverage_cli_typ_cd, coverage_PlanID, getCurrentUserName(), DateTime.Now, DateTime.Now);
                        coverageRecord.CVG_FACE_AMT = parentCoverageModel.Indicator_TC_Code == "1" || parentCoverageModel.Indicator_TC_Code== "4" && lifeModel.FaceAmt > 0 ? lifeModel.FaceAmt: parentCoverageModel.CurrentAmt;
                        bool isDuplicate = checkForDuplicate(context, coverageRecord);

                        if (!isDuplicate)
                        {
                            // Save Coverage Record 
                            context.Add(coverageRecord);
                            context.SaveChanges();

                            /**********************************************************************
                             * Save Child Base Coverages
                            *********************************************************/
                            saveAdditionalCoveragesHelper(context, parentCoverageModel.AddtionalCoverages, planID, policy, issueAge, coverage_cli_typ_cd);
                        }
                    }
                }
            }


            /*******************************************************************
             * Save Child Coverage Models from the main Parent Holding
             *****************************************************************/

            foreach (Holding mainCoverageHolding in holding.CoverageHolidngs)
            {
                if (mainCoverageHolding.PolicyData != null && mainCoverageHolding.PolicyData.Count > 0
                    && mainCoverageHolding.PolicyData.First().LifeModels != null && mainCoverageHolding.PolicyData.First().LifeModels.Count > 0)
                {
                    LifeModel lifeModel = mainCoverageHolding.PolicyData.First().LifeModels.First();
                    Policy coveragePolicyModel = mainCoverageHolding.PolicyData.First();

                    if (lifeModel.CoverageModels != null)
                    {
                        int issueAge = -1;

                        foreach (CoverageModel coverageModel in lifeModel.CoverageModels)
                        {
                            string planID = coveragePolicyModel.ProductCode;
                            string coverage_cli_typ_cd = string.Empty;

                            foreach (LifeParticipantModel lifeParticipant in coverageModel.LifeParticipants)
                            {
                                if (lifeParticipant.IssueAge > -1) issueAge = lifeParticipant.IssueAge;

                                List<RelationModel> coverageRelationModels = allRelationModels.Where(x => x.RelatedObjectID.ToLower() == lifeParticipant.PartyID.ToLower()).ToList();
                                foreach (RelationModel coverageRelationModel in coverageRelationModels)
                                {
                                    InsuredType coverageInsuredType = getInsuredType(coverageRelationModel.RelationRoleCode);
                                    if (coverageInsuredType != InsuredType.None)
                                    {
                                        coverage_cli_typ_cd = getRelationRoleCodeConverter(coverageInsuredType);
                                        break;
                                    }
                                }
                            }

                            // Translate Plan ID 
                           string coveragePlanID = translateText(context, planID, TranslationType.IP_PLAN_CD, false);

                            TCVG coverageRecord = new TCVG(policy.PolicyNumber, coverage_cli_typ_cd, coveragePlanID, getCurrentUserName(), DateTime.Now, DateTime.Now);
                            coverageRecord.CVG_FACE_AMT = coverageModel.Indicator_TC_Code == "1" || coverageModel.Indicator_TC_Code == "4" && lifeModel.FaceAmt > 0 ? lifeModel.FaceAmt : coverageModel.CurrentAmt;
                            bool isDuplicate = checkForDuplicate(context, coverageRecord);

                            if (!isDuplicate)
                            {
                                // Save Coverage Record 
                                context.Add(coverageRecord);
                                context.SaveChanges();
                            }

                            saveAdditionalCoveragesHelper(context, coverageModel.AddtionalCoverages, planID, policy, issueAge, coverage_cli_typ_cd);

                        }
                    }
                }
            }

        }

        #endregion

        #region Policy Data
        private void savePolicyDataHelper(EFContext context, Holding holding, Holding.Policy policy, ContingentOwnerModel contingentOwner, List<AgentModel> agentModels, OwnerModel ownerModel, PartyModel primaryInsuredModel)
        {
            string purposeCode = string.Empty;
            string bahamasCountryCode = "BS";

            bool hasPurposeCode = policy.LifeModels.Count() > 0 && policy.LifeModels.First().CoverageModels.Count > 0 && policy.LifeModels.First().CoverageModels.First().MainPurposeCodes != null ? true : false;
            if (hasPurposeCode)
                purposeCode = policy.LifeModels.First().CoverageModels.First().MainPurposeCodes.PurposeCode;

            string insured_purpose_cd = translateText(context, holding.IntentData.ExpenseNeedTypeCode, TranslationType.IP_PURP, true);
            TPOL policyRecord = new TPOL(policy.PolicyNumber, purposeCode, getCurrentUserName(), DateTime.Now, DateTime.Now);

            string countryCode = getCountryCode(context, ownerModel.Country);
            policyRecord.POL_CTRY_CD = countryCode.Length > 0 ? countryCode : bahamasCountryCode;

            // Policy Division Type 
            string policyDivisionType_CD = string.Empty;
            if (policy.LifeModels.Count > 0)
                policyDivisionType_CD = translateText(context, policy.LifeModels.First().DivType, TranslationType.IP_POL_DIV_OPT_CD, true);

            policyRecord.POL_DIV_OPT_CD = policyDivisionType_CD;

            string paymentMethod = holding.ArrangementData.PaymentMethod.Length > 0 ? holding.ArrangementData.PaymentMethod : policy.PaymentMethod;
            string arrMethod = holding.ArrangementData.ArrMode;

            policyRecord.POL_BILL_TYP_CD = translateText(context, paymentMethod, TranslationType.IP_PAYT, true);
            policyRecord.POL_BILL_MODE_CD = translateText(context, arrMethod, TranslationType.IP_POL_BILL_MODE_CD, true);

            if (contingentOwner.WasContingentOwnerFound)
            {
                policyRecord.POL_CONTINGENT_BTH_DT = contingentOwner.BirthDate;
                policyRecord.POL_CONTINGENT_GIV_NM = contingentOwner.FirstName;
                policyRecord.POL_CONTINGENT_SUR_NM = contingentOwner.LastName;
                policyRecord.POL_CONTINGENT_CLI_INSRD_CD = translateText(context, contingentOwner.RelationDescription, TranslationType.IP_OTOI, true);
            }

            RelationModel ownerRelation = null;
            string id_Owner = "owner";
            bool doesRelationExists = doesObjectExists(ownerModel, id_Owner, false);
            if (doesRelationExists) ownerRelation = ownerModel.AllRelations.Where(x => x.RelationRoleCode.ToLower() != "owner") != null ? ownerModel.AllRelations.Where(x => x.RelationRoleCode.ToLower() != "owner").First() : null;

           // RelationModel ownerRelation2 = null;
           // doesRelationExists = doesObjectExists(ownerModel, id_Owner, true);
           // if (doesRelationExists) ownerRelation2 = ownerModel.AllRelations.Where(x => x.RelationRoleCode.ToLower() == "owner") != null ? ownerModel.AllRelations.Where(x => x.RelationRoleCode.ToLower() == "owner").First() : null;

            if (ownerRelation != null) policyRecord.POL_CLI_INSRD_CD = translateText(context, ownerRelation.RelationRoleCode.Length > 0 ? ownerRelation.RelationRoleCode : ownerRelation.RelationDescription, TranslationType.IP_OTOI, true);
           // else if (ownerRelation2 != null) policyRecord.POL_CLI_INSRD_CD = translateText(context, ownerRelation2.RelationRoleCode.Length > 0 ? ownerRelation2.RelationRoleCode : ownerRelation2.RelationDescription, TranslationType.IP_OTOI, true);


            // Agent ID 
            int counter = 0;
            foreach (AgentModel agentModel in agentModels)
            {
                int length = agentModel.ProducerID.Length;
                if (counter > 0)
                {  
                    // Set the second agent 
                    if (length > 6) policyRecord.AGT_ID_2 = agentModel.ProducerID.Substring(0, 6);
                    else policyRecord.AGT_ID_2 = agentModel.ProducerID;

                    policyRecord.AGT_SHRT_PCT_2 = (decimal)agentModel.InterestPercent / 100;
                }
                else
                {
                    // Set the first agent 
                    if (length > 6) policyRecord.SERV_AGT_ID = agentModel.ProducerID.Substring(0, 6);
                    else policyRecord.SERV_AGT_ID = agentModel.ProducerID;

                    policyRecord.AGT_SHRT_PCT_1 = (decimal)agentModel.InterestPercent / 100;
                }

                counter++;
            }

            // Policy Create Stat Date 
            // Change_CREATE_STAT_CD to R only after everthing has completed
            policyRecord.POL_CREATE_STAT_CD = "R"; // Received
            policyRecord.POL_APP_RECV_DT = DateTime.Now.Date;

            // Primary Owner Address is the Policy Issue Location
            policyRecord.POL_ISS_LOC_CD = translateText(context, ownerModel.Country, TranslationType.IP_ILOC, true);
            if (policyRecord.POL_ISS_LOC_CD.Length == 0)
                policyRecord.POL_ISS_LOC_CD = bahamasCountryCode;

            /************************************************
                * Calculate Issue Specified Day 
            ************************************************/
            if (policy.Policy_ApplicationInfoModel.IssueDate_Type == IssueDateType.CurrentDayOfTheMonth)
            {
                if (policy.Policy_ApplicationInfoModel.IssueDate_SpecifiedDay > 0)
                {
                    int currentYear = DateTime.Now.Year;
                    int currentMonth = DateTime.Now.Month;
                    DateTime currentDayOfTheMonth = new DateTime(currentYear, currentMonth, policy.Policy_ApplicationInfoModel.IssueDate_SpecifiedDay);
                    policyRecord.POL_ISS_EFF_DT = currentDayOfTheMonth;
                }
            }
            else if (policy.Policy_ApplicationInfoModel.IssueDate_Type == IssueDateType.BackDateToSaveAge)
            {
                /************************************************
                 * Calculate Back Date To Save Age 
                 ************************************************/

                int birthMonthIndex = primaryInsuredModel.Persons.First().BirthDate.Month;
                int currentMonthIndex = DateTime.Now.Month;
                DateTime today = DateTime.Now;

                if (birthMonthIndex > currentMonthIndex)
                    policyRecord.POL_ISS_EFF_DT = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                else if (birthMonthIndex < currentMonthIndex)
                {
                    DateTime sixMonthsBack = today.AddMonths(-6);
                    int issueDayIndex = primaryInsuredModel.Persons.First().BirthDate.Day;
                    int issueDay = issueDayIndex - 1;
                    if (issueDay <= 0) issueDay = 1;
                    
                    int issueMonth = birthMonthIndex;
                    DateTime issueDate = new DateTime(today.Year, issueMonth, issueDay);

                    if (issueDate < sixMonthsBack)
                        policyRecord.POL_ISS_EFF_DT = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    else
                        policyRecord.POL_ISS_EFF_DT = issueDate;
                }
                else
                {
                    // Birth Month == Current Month
                    int currentDayIndex = today.Day;
                    int issueDayIndex = primaryInsuredModel.Persons.First().BirthDate.Day;
                    int issueDay = issueDayIndex - 1;
                    if (issueDay < currentDayIndex)
                    {
                        DateTime issueDate = new DateTime(today.Year, today.Month, issueDay);
                        policyRecord.POL_ISS_EFF_DT = issueDate;
                    }
                    else
                    {
                        int currentIssueDay = today.Day - 1;
                        DateTime issueDate = new DateTime(today.Year, today.Month, currentIssueDay);
                        policyRecord.POL_ISS_EFF_DT = issueDate;
                    }
                }
            }

            // TO DO: Missing Mappings for SERV_BR_ID     

            bool isDuplicate = checkForDuplicate(context, policyRecord);

            if (!isDuplicate)
            {
                // Save Client Record 
                context.Add(policyRecord);
                context.SaveChanges();
            }
        }

        #endregion

        #region Beneficiary Data 

        private TBENE saveBeneficiaryDataHelper(EFContext context, Holding holding, Holding.Policy policy, PartyModel partyModel, InsuredType insuredType, RelationModel relationModel, RelationModel beneficiaryRelationModel, List<RelationModel> trusteeRelations, List<PartyModel> partyModels)
        {
            string relationRoleCode = getRelationRoleCode(insuredType);

            if (insuredType == InsuredType.Beneficiary)
            {
                TBENE beneficiaryRecord = new TBENE(policy.PolicyNumber, getCurrentUserName(), DateTime.Now, DateTime.Now);

                beneficiaryRecord.BNFY_NM = partyModel.PartyFullName; // Beneficiary Name
                beneficiaryRecord.BNFY_PRCDS_PCT = (decimal)relationModel.InterestPercent / 100; // Beneficiary Interest Percent 

                // Benefit Type CD
                string irrevokableInd = beneficiaryRelationModel != null && beneficiaryRelationModel.IrrevokableInd.Length > 0 ? beneficiaryRelationModel.IrrevokableInd.ToLower() : relationModel.IrrevokableInd.ToLower();
                bool isIrrevokableInd = false;
                bool.TryParse(irrevokableInd, out isIrrevokableInd);
                beneficiaryRecord.BNFY_TYP_CD = isIrrevokableInd == true ? "I" : "R";

                // Relation Description 
                if (partyModel.ISAPerson)
                {
                    string bnfy_rel_insrd_cd = relationModel.RelationDescription.Length > 0 ? relationModel.RelationDescription : relationModel.RelationRoleCode;
                    beneficiaryRecord.BNFY_REL_INSRD_CD = translateText(context, bnfy_rel_insrd_cd, TranslationType.IP_BTOI, true);
                }
                else
                {
                    string bnfy_rel_insrd_cd = partyModel.Organization.OrgForm;
                    beneficiaryRecord.BNFY_REL_INSRD_CD = translateText(context, bnfy_rel_insrd_cd, TranslationType.IP_BTOI, true);
                }

                /*************************************
                 * Get Client Type Code for Beneficiary related to
                 * Insured (Primary Insured = P1) and 
                 * (Other = P2)
                 * Check if the Relation object 
                 * has a relation role of Primary Insured
                 ***************************************/
                bool isPrimaryInsured = is_Beneficiary_Related_To_PrimaryInsured(relationModel);

                // Check if Life Participant is has a relation role code of primary insured
                // if it cannot be found in the XML relation object
                if (!isPrimaryInsured)
                    isPrimaryInsured = is_Beneficiary_Related_To_PrimaryInsured_Using_LifeParticpant(policy, partyModel);

                beneficiaryRecord.CLI_TYP_CD = isPrimaryInsured ? "P1" : "P2";

                /************************************************
                 * Find Trustee information 
                 *************************************************/
                string trusteeName = string.Empty;

                List<RelationModel> filteredTrusteeRelations = trusteeRelations.Where(x => x.OriginatingObjectID == partyModel.PartyID).ToList();

                if (filteredTrusteeRelations.Count > 0)
                {
                    RelationModel trusteeRelationModel = filteredTrusteeRelations.First();
                    List<PartyModel> trusteePartyModels = partyModels.Where(x => x.PartyID.ToLower().Trim() == trusteeRelationModel.RelatedObjectID.ToLower().Trim()).ToList();
                    if (trusteePartyModels.Count > 0)
                    {
                        PartyModel trusteePartyModel = trusteePartyModels.First();
                        if (trusteePartyModel.IsATrustee)
                        {
                            trusteeName = trusteePartyModel.PartyFullName;
                            beneficiaryRecord.BNFY_TRUSTEE_NM = trusteeName;
                            string trusteeRelationDescription = trusteeRelationModel.RelationRoleCode.Length > 0 ? trusteeRelationModel.RelationRoleCode : trusteeRelationModel.RelationDescription;
                            beneficiaryRecord.BNFY_TRUSTEE_REL_CD = translateText(context, trusteeRelationDescription, TranslationType.IP_BTOI, true);
                        }
                    }
                }

                bool isDuplicate = checkForDuplicate(context, beneficiaryRecord);
                if (!isDuplicate)
                {

                    // Save Client Record 
                    context.Add(beneficiaryRecord);
                    context.SaveChanges();

                    // Return beneficiary record if equally distributed for percentages
                    if (beneficiaryRelationModel != null)
                    {
                        string[] words = beneficiaryRelationModel.DistributionOption.Split(' ', ';', ',', ':', '.', '?', '|');
                        foreach (string word in words)
                        {
                            if (word.ToUpper().Contains("EQUAL")) return beneficiaryRecord;
                            else
                            {
                                decimal perc = 0;
                                decimal.TryParse(beneficiaryRelationModel.InterestPercent.ToString(), out perc);
                                perc = perc / 100;
                                beneficiaryRecord.BNFY_PRCDS_PCT = perc;
                                context.SaveChanges();
                            }
                        }
                    }

                }
            }

            return null;
        }


        #endregion

        #region Client Data
        private void saveClientDataHelper(EFContext context, Holding holding, Holding.Policy policy, PersonModel personModel, InsuredType insuredType, PartyModel partyModel, RelationModel relationModel)
        {
            string gender = personModel.Gender.Trim().ToLower();
            bool isARealGender = false;
            if (gender == "male" || gender == "female")
                isARealGender = true;
            else
                gender = "other";

            string relationRoleCode = getRelationRoleCodeConverter(insuredType);
            TCLI clientRecord = new TCLI(policy.PolicyNumber, relationRoleCode, getCurrentUserName(), DateTime.Now, DateTime.Now);

            clientRecord.CLI_INDV_GIV_NM = isARealGender ? personModel.FirstName : string.Empty;
            clientRecord.CLI_INDV_MID_NM = personModel.MiddleName;
            clientRecord.CLI_INDV_SUR_NM = personModel.LastName;
            clientRecord.CLI_INDV_TITL_TXT = translateText(context, personModel.Prefix, TranslationType.IP_TITL_TXT, true);
            clientRecord.CLI_INDV_SFX_NM = translateText(context, personModel.Suffix, TranslationType.IP_SFX_NM, true);
            clientRecord.CLI_COMPANY_NM = !isARealGender ? personModel.FirstName : null;

            clientRecord.CLI_BTH_DT = personModel.IsBirthDateValid ? personModel.BirthDate : null;
            clientRecord.CLI_SEX_CD = translateText(context, gender, TranslationType.IP_SEX_CD, true);

            LifeParticipantModel lifeParticipantModel = checkLifeParticipantObjectExists(policy);

            /*******************************************
             * Apply Smoker Code based on a conidtion of Primary Insured 
             ************************************************/
            bool isPrimaryInsured = false;
            if (partyModel.PartyID.ToLower() == "party_primaryinsured") isPrimaryInsured = true;

            if(isPrimaryInsured)
            {
                clientRecord.CLI_SMKR_CD = translateText(context, policy.Policy_ApplicationInfoModel.UWClassInsured1, TranslationType.IP_SMKR_CD, true);
            } else
            {
                clientRecord.CLI_SMKR_CD = translateText(context, policy.Policy_ApplicationInfoModel.UWClassInsured2, TranslationType.IP_SMKR_CD, true);
            }
       
            // Nationality Country Code 
            string countryCode = getCountryCode(context, partyModel.Address);
            clientRecord.CLI_NATLTY_CNTRY_CD = isARealGender ? translateText(context, partyModel.Nationality, TranslationType.IP_CNTRY, true) : countryCode;

            // Employer Name 
            clientRecord.CLI_EMPL_NM = partyModel.EmploymentModel.EmployerName;

            // Comment 
            string otherIncomeAndSource = partyModel.EmploymentModel.EmploymentOLifeExtension.OtherIncomeAndSourceComment;
            string unemploymentDetails = partyModel.EmploymentModel.EmploymentOLifeExtension.UnemploymentDetailsComment;
            string occupation = partyModel.EmploymentModel.Occupation;
            string unemploymentSpecify = partyModel.EmploymentModel.EmploymentOLifeExtension.UnemploymentSpecify;
            string unemploymentIncomeAndSource = partyModel.EmploymentModel.EmploymentOLifeExtension.UnemploymentIncomeAndSource;
            string providerOrSchoolText = personModel.Education.ProviderOrSchoolText;
            string personTitle = personModel.Title;
            string comment = System.String.Format("{0}\r\n\r\n{1}\r\n{2}\r\n{3}\r\n{4}\r\n{5}\r\n{6}", personTitle, otherIncomeAndSource, unemploymentIncomeAndSource, unemploymentDetails, unemploymentSpecify, occupation, providerOrSchoolText);
            clientRecord.CLI_COMNT_TXT = comment;

            // Years at Address
            //clientTable.CLI_ADDR_YR_DUR_DUR = partyModel.Address.YearsAtAddress;

            // Employment Annual Salary 
            clientRecord.CLI_INCM_EARN_AMT = partyModel.EmploymentModel.AnnualSalary;

            // Years at Employment 
            clientRecord.CLI_EMPL_YR_DUR = (decimal)partyModel.EmploymentModel.YearsAtEmployment;

            // Tax ID 
            // IF THE CLIENT AGE < 18 AND NO NIBNUMBER and Address is BS -> OVERRIDE 
         
            if (personModel.Age < 18 && partyModel.NIBNumber.Trim().Length == 0 && countryCode == "BS")
                clientRecord.CLI_TAX_ID = "OVERRIDE"; 
            else clientRecord.CLI_TAX_ID = partyModel.NIBNumber != null && partyModel.NIBNumber.Length > 0 ? partyModel.NIBNumber : null;

            // Social Security 
            clientRecord.CLI_SSN_ID = partyModel.SocialSecurityNumber != null && partyModel.SocialSecurityNumber.Length > 0 ? partyModel.SocialSecurityNumber : null;

            // TinID --> Government ID 

            clientRecord.CLI_TIN_ID = partyModel.TaxIDNumber;  // partyModel.GovernmentInfo.GovtID != null && partyModel.GovernmentInfo.GovtID.Length > 0 ? partyModel.GovernmentInfo.GovtID : null;

            // Birth Country
            if (partyModel.Persons.Count > 0 && partyModel.Persons != null)
                clientRecord.CLI_BTH_LOC_CD = translateText(context, partyModel.Persons.First().BirthCountry, TranslationType.IP_CNTRY, true);

            // Suffix
            clientRecord.CLI_INDV_SFX_NM = personModel.Suffix;

            clientRecord.CLI_TYP_CD = !isARealGender ? "OP" : string.Empty;

            // EI Indicator 
            clientRecord.CLI_EI_IND = policy.Policy_ApplicationInfoModel.EConsentAcknowledged == true ? "Y" : "N";
            clientRecord.CLI_SHR_IND = policy.Policy_ApplicationInfoModel.EConsentAcknowledged == true ? "Y" : "N";

            // Permanent Residence Country
            clientRecord.CLI_PRM_RES_CNTRY = translateText(context, partyModel.PartyResidenceCountry, TranslationType.IP_CNTRY, true);

            bool isDuplicate = checkForDuplicate(context, clientRecord);

            if (!isDuplicate)
            {
                // Save Client Record 
                context.Add(clientRecord);
                context.SaveChanges();
            }
        }

        private void saveDocumentDataHelper(EFContext context, Holding holding, Holding.Policy policy, PersonModel personModel, PartyModel partyModel, InsuredType insuredType)
        {
            string relationRoleCode = getRelationRoleCodeConverter(insuredType);

            string policyNumber = policy.PolicyNumber;
            foreach (GovernmentInfoModel govtInfo in partyModel.GovernmentInfoModels)
            {
                TCDOC clientDocumentTable = new TCDOC(policyNumber, relationRoleCode, govtInfo.GovtID, getCurrentUserName(), DateTime.Now, DateTime.Now);
                //TCDOC clientDocumentTable = new TCDOC(policy.PolicyNumber, relationRoleCode, govtInfo.GovtID, getCurrentUserName(), DateTime.Now, DateTime.Now);
                clientDocumentTable.CLI_DOC_XPRY_DT = govtInfo.GovtIDExpDate;  //partyModel.GovernmentInfo.GovtIDExpDate;
                clientDocumentTable.CLI_DOC_CNTRY_CD = translateText(context, govtInfo.Nation, TranslationType.IP_CNTRY, true);  //translateText(context, partyModel.GovernmentInfo.Nation, TranslationType.IP_CNTRY);

                bool isDuplicate = checkForDuplicate(context, clientDocumentTable);
                if (govtInfo.GovtID.Length > 0 && !isDuplicate)
                {
                    // Save Client Document Record 
                    context.Add(clientDocumentTable);
                    context.SaveChanges();
                }
            }
        }

        private void saveClientAddressDataHelper(EFContext context, Holding holding, Holding.Policy policy, PersonModel personModel, PartyModel partyModel, InsuredType insuredType)
        {
            // Cli_Typ_CD
            // PR = Primary Address
            // FM = Foreign Address
            string policyNumber = policy.PolicyNumber;
            foreach (AddressModel addressModel in partyModel.Addresses)
            {
                string cli_addr_typ_cd = getAddressTypeCode(addressModel);
                string relationRoleCode = getRelationRoleCodeConverter(insuredType);

                TCLIA clienAddressRecord = new TCLIA(policyNumber, relationRoleCode, cli_addr_typ_cd, getCurrentUserName(), DateTime.Now, DateTime.Now);
                // TCLIA clienAddressRecord = new TCLIA(policy.PolicyNumber, relationRoleCode, cli_addr_typ_cd, getCurrentUserName(), DateTime.Now, DateTime.Now);

                string countryCode = getCountryCode(context, addressModel);
                string countryCodeUpper = countryCode.ToUpper();
                string bahamasCode = "BS";
                string turksCode = "TC";

                // Country Code
                clienAddressRecord.CLI_CTRY_CD = countryCode;

                // Address Typ CD
                clienAddressRecord.CLI_ADDR_TYP_CD = cli_addr_typ_cd;

                string addressState = addressModel.AddrressState.Length > 0 ? addressModel.AddrressState : addressModel.AddressStateTC;
                if (countryCodeUpper == bahamasCode || countryCode == turksCode)
                {

                    // Bahamas
                    if (countryCode == bahamasCode)
                    {
                        clienAddressRecord.CLI_CITY_NM_TXT = addressModel.City;

                        // Municipality
                        clienAddressRecord.CLI_ADDR_MUN_CD = translateText(context, addressState, TranslationType.IP_MCPCD, true);

                        if (addressModel.Zip.Length > 0)
                        {
                            clienAddressRecord.CLI_ADDR_LN_1_TXT = addressModel.Zip;
                            clienAddressRecord.CLI_ADDR_LN_2_TXT = addressModel.Line1;
                            clienAddressRecord.CLI_ADDR_LN_3_TXT = addressModel.Line2;
                        }
                        else
                        {
                            clienAddressRecord.CLI_ADDR_LN_1_TXT = addressModel.Line1;
                            clienAddressRecord.CLI_ADDR_LN_2_TXT = addressModel.Line2;
                            clienAddressRecord.CLI_ADDR_LN_3_TXT = string.Empty;
                        }
                    }

                    // Turks and Caicos
                    if (countryCode == turksCode)
                    {
                        clienAddressRecord.CLI_CITY_NM_TXT = addressState;

                        if (addressModel.Zip.Length > 0 || addressModel.PostalDropCode.Length > 0)
                        {
                            clienAddressRecord.CLI_ADDR_LN_1_TXT = addressModel.PostalDropCode;
                            clienAddressRecord.CLI_ADDR_LN_2_TXT = addressModel.Line1;
                            clienAddressRecord.CLI_ADDR_LN_3_TXT = addressModel.City;
                            clienAddressRecord.CLI_ADDR_ADDL_TXT = addressModel.Line2;
                        }
                        else
                        {
                            clienAddressRecord.CLI_ADDR_LN_1_TXT = addressModel.Line1;
                            clienAddressRecord.CLI_ADDR_LN_2_TXT = addressModel.Line2;
                            clienAddressRecord.CLI_ADDR_LN_3_TXT = string.Empty;
                        }
                    }
                }
                else
                {
                    // NOT BAHAMAS, TURKS AND CAICOS, OR US
                    clienAddressRecord.CLI_PSTL_CD = addressModel.PostalDropCode;
                    clienAddressRecord.CLI_ADDR_LN_1_TXT = addressModel.Line1;
                    clienAddressRecord.CLI_ADDR_LN_2_TXT = addressModel.Line2;

                    // IS Country United States 
                    if (countryCode.ToUpper() == "US")
                    {
                        clienAddressRecord.CLI_CRNT_LOC_CD = addressModel.AddrressState.Length > 0 ? addressModel.AddrressState : addressModel.AddressStateTC;
                        clienAddressRecord.CLI_CITY_NM_TXT = addressModel.City;
                        clienAddressRecord.CLI_PSTL_CD = addressModel.Zip.Length > 0 ? addressModel.Zip : addressModel.PostalDropCode;
                    }
                    else
                    {
                        clienAddressRecord.CLI_CITY_NM_TXT = addressState;
                        clienAddressRecord.CLI_ADDR_LN_3_TXT = addressModel.City;
                    }
                }

                // Years At Address
                clienAddressRecord.CLI_ADDR_YR_DUR = addressModel.YearsAtAddress;

                // Phone Numbers 
                // Only store phone numbers and email addresses for clients at the age of 18 or over
                if (personModel.Age >= 18)
                {
                    PhoneNumberType phoneNumberTypeFilter = PhoneNumberType.Permanent;
                    if (cli_addr_typ_cd == "PR")
                    {

                        PhoneModel homePhoneModel = partyModel.PhoneModels.Where(x => x.PhoneTypeCode.Contains("Home") && x.PhoneNumberType == phoneNumberTypeFilter).Any() ? partyModel.PhoneModels.Where(x => x.PhoneTypeCode.Contains("Home") && x.PhoneNumberType == phoneNumberTypeFilter).First() : new PhoneModel();
                        PhoneModel mobilePhoneModel = partyModel.PhoneModels.Where(x => x.PhoneTypeCode.Contains("Mobile") && x.PhoneNumberType == phoneNumberTypeFilter).Any() ? partyModel.PhoneModels.Where(x => x.PhoneTypeCode.Contains("Mobile") && x.PhoneNumberType == phoneNumberTypeFilter).First() : new PhoneModel();
                        PhoneModel businessPhoneModel = partyModel.PhoneModels.Where(x => x.PhoneTypeCode.Contains("Business") && x.PhoneNumberType == phoneNumberTypeFilter).Any() ? partyModel.PhoneModels.Where(x => x.PhoneTypeCode.Contains("Business") && x.PhoneNumberType == phoneNumberTypeFilter).First() : new PhoneModel();

                        clienAddressRecord.CLI_CNCT_WORK_PH = businessPhoneModel.DialNumber.Length > 0 ? formattedDialNumber(businessPhoneModel) : null;
                        clienAddressRecord.CLI_CNCT_HOME_PH = homePhoneModel.DialNumber.Length > 0 ? formattedDialNumber(homePhoneModel) : null;
                        clienAddressRecord.CLI_CNCT_CELL_PH = mobilePhoneModel.DialNumber.Length > 0 ? formattedDialNumber(mobilePhoneModel) : null;
                        clienAddressRecord.CLI_CNCT_EMAIL = partyModel.EmailModel.Email;
                    }
                    else
                    {
                        phoneNumberTypeFilter = PhoneNumberType.Foreign;
                        PhoneModel homePhoneModel = partyModel.PhoneModels.Where(x => x.PhoneTypeCode.Contains("Home") && x.PhoneNumberType == phoneNumberTypeFilter).Any() ? partyModel.PhoneModels.Where(x => x.PhoneTypeCode.Contains("Home") && x.PhoneNumberType == phoneNumberTypeFilter).First() : new PhoneModel();
                        PhoneModel mobilePhoneModel = partyModel.PhoneModels.Where(x => x.PhoneTypeCode.Contains("Mobile") && x.PhoneNumberType == phoneNumberTypeFilter).Any() ? partyModel.PhoneModels.Where(x => x.PhoneTypeCode.Contains("Mobile") && x.PhoneNumberType == phoneNumberTypeFilter).First() : new PhoneModel();
                        PhoneModel businessPhoneModel = partyModel.PhoneModels.Where(x => x.PhoneTypeCode.Contains("Business") && x.PhoneNumberType == phoneNumberTypeFilter).Any() ? partyModel.PhoneModels.Where(x => x.PhoneTypeCode.Contains("Business") && x.PhoneNumberType == phoneNumberTypeFilter).First() : new PhoneModel();

                        clienAddressRecord.CLI_CNCT_WORK_PH = businessPhoneModel.DialNumber.Length > 0 ? formattedDialNumber(businessPhoneModel) : null;
                        clienAddressRecord.CLI_CNCT_HOME_PH = homePhoneModel.DialNumber.Length > 0 ? formattedDialNumber(homePhoneModel) : null;
                        clienAddressRecord.CLI_CNCT_CELL_PH = mobilePhoneModel.DialNumber.Length > 0 ? formattedDialNumber(mobilePhoneModel) : null;
                    }
                }

                bool isDuplicate = checkForDuplicate(context, clienAddressRecord);
                if (!isDuplicate)
                {
                    // Save Client Address Record
                    context.Add(clienAddressRecord);
                    context.SaveChanges();
                }
            }
        }

        #endregion

        #region Helper Functions
        private string getCountryCode(EFContext context, PartyModel partyModel)
        {
            string countryCode = partyModel.Address.AddressCountryTC;
            countryCode = translateText(context, countryCode, TranslationType.IP_CNTRY, true);
            return countryCode;
        }

        private string getCountryCode(EFContext context, AddressModel addressModel)
        {
            string countryCode = addressModel.AddressCountryTC;
            countryCode = translateText(context, countryCode, TranslationType.IP_CNTRY, true);
            countryCode = countryCode.Replace(" ", "").Trim();
            return countryCode;
        }

        private string getCountryCode(EFContext context, string countryCode)
        {
            string countryCodeTranslate = translateText(context, countryCode, TranslationType.IP_CNTRY, true);
            countryCodeTranslate = countryCodeTranslate.Replace(" ", "").Trim();
            return countryCodeTranslate;
        }

        private string getAddressTypeCode(PartyModel partyModel)
        {
            string addressTypeCode = "PR";

            if (partyModel.Address.AddressTypeCode.Contains("Foreign"))
                addressTypeCode = "FM";

            return addressTypeCode;
        }

        private string getAddressTypeCode(AddressModel addressModel)
        {

            string addressTypeCode = "PR";

            if (addressModel.AddressTypeCode.Contains("Foreign"))
                addressTypeCode = "FM";

            return addressTypeCode;
        }

        private string formattedDialNumber(PhoneModel phoneModel)
        {
            string phone = phoneModel.DialNumber;
            if (phoneModel.DialNumber.Length >= 3)
            {
                string formattedDialNumber1 = phoneModel.DialNumber.Substring(0, 3);
                string formattedDialNumber2 = phoneModel.DialNumber.Substring(3, phoneModel.DialNumber.Length - 3);
                phone = phoneModel.AreaCode + "-" + formattedDialNumber1 + "-" + formattedDialNumber2;
            }

            return phone;
        }

        private LifeParticipantModel checkLifeParticipantObjectExists(Holding.Policy policy)
        {
            LifeParticipantModel? lifeParticipantModel = null;

            if (policy.LifeModels.Count > 0)
            {
                if (policy.LifeModels.First().CoverageModels.Count > 0)
                {
                    CoverageModel coverageModel = policy.LifeModels.First().CoverageModels.First();

                    if (coverageModel.LifeParticipants.Count > 0)
                    {
                        LifeParticipantModel lifeParticipantModel1 = coverageModel.LifeParticipants.First();
                        lifeParticipantModel = lifeParticipantModel1;
                    }
                }
            }

            return lifeParticipantModel;
        }

        private string translateText(EFContext context, string translationText, TranslationType translationType, bool removeSpaces)
        {
            string txt = translationText;

            string translationTypeText = convertToString(translationType);

            IEnumerable<SP_TTTAB_Results> results = context.Get_TTTAB_Results(translationTypeText, translationText);

            if (results.Count() > 0)
            {
                if (removeSpaces) txt = results.First().TranslationTo.Replace(" ", "");
                else txt = results.First().TranslationTo;
            }
            else
            {
                if (translationType == TranslationType.IP_PLAN_CD) txt = "BST20C";
                else txt = string.Empty;
            }

            return txt;
        }

        private static string convertToString(TranslationType translationType)
        {
            return translationType.ToString();
        }

        private string getRelationRoleCode(InsuredType insuredType)
        {
            string relationRoleCode = insuredType == InsuredType.Insured ? "insured" : insuredType == InsuredType.JointInsured ? "joint insured" : insuredType == InsuredType.Owner ? "owner" : insuredType == InsuredType.Beneficiary ? "beneficiary" : "";
            return relationRoleCode;
        }

        private string getRelationRoleCodeConverter(InsuredType insuredType)
        {
            string relationRoleCode = insuredType == InsuredType.Insured ? "P1" : insuredType == InsuredType.JointInsured ? "P2" : insuredType == InsuredType.Owner ? "OP" : insuredType == InsuredType.Beneficiary ? "beneficiary" : "";
            return relationRoleCode;
        }

        private InsuredType getInsuredType(string relationRoleCode)
        {
            InsuredType insuredType = InsuredType.None;
            string formattedRelationRoleCode = relationRoleCode.ToLower().Replace(" ", string.Empty);
            insuredType = formattedRelationRoleCode == "insured" ? InsuredType.Insured : formattedRelationRoleCode == "jointinsured" ? InsuredType.JointInsured : formattedRelationRoleCode == "owner" ? InsuredType.Owner : InsuredType.None;
            return insuredType;
        }


        private bool checkForDuplicate(EFContext context, TCLI record)
        {
            bool isDuplicate = false;

            string policyID = record.POL_ID.ToLower().Trim();
            string cli_typ_cd = record.CLI_TYP_CD.ToLower().Trim();

            if (context.Clients.Count() > 0)
            {
                List<TCLI> clients = context.Clients.Where(x => x.POL_ID.ToLower().Trim() == policyID && x.CLI_TYP_CD.ToLower().Trim() == cli_typ_cd).ToList();
                if (clients.Count > 0) isDuplicate = true;
            }

            return isDuplicate;
        }

        private bool checkForDuplicate(EFContext context, TPOL record)
        {
            bool isDuplicate = false;

            string policyID = record.POL_ID.ToLower().Trim();
            string pol_ins_purp_cd = record.POL_INS_PURP_CD.ToLower().Trim();

            if (context.Policies.Count() > 0)
            {
                List<TPOL> policies = context.Policies.Where(x => x.POL_ID.Trim().ToLower() == policyID
                        && x.POL_INS_PURP_CD.Trim().ToLower() == pol_ins_purp_cd).ToList();

                if (policies.Count > 0) isDuplicate = true;

            }

            return isDuplicate;
        }

        private bool checkForDuplicate(EFContext context, TCLIA record)
        {
            bool isDuplicate = false;

            string policyID = record.POL_ID.ToLower().Trim();
            string cli_typ_cd = record.CLI_TYP_CD.ToLower().Trim();
            string cli_addr_typ_cd = record.CLI_ADDR_TYP_CD.ToLower().Trim();

            if (context.Addresses.Count() > 0)
            {
                List<TCLIA> addresses = context.Addresses.Where(x => x.POL_ID.ToLower().Trim() == policyID
                             && x.CLI_TYP_CD != null && x.CLI_TYP_CD.ToLower().Trim() == cli_typ_cd &&
                               x.CLI_ADDR_TYP_CD != null && x.CLI_ADDR_TYP_CD.ToLower().Trim() == cli_addr_typ_cd).ToList();


                if (addresses.Count > 0) isDuplicate = true;
            }

            return isDuplicate;
        }

        private bool checkForDuplicate(EFContext context, TBENE record)
        {
            bool isDuplicate = false;

            string policyID = record.POL_ID.ToLower().Trim().Replace(" ", "");
            string beneficiaryName = record.BNFY_NM.ToLower().Trim().Replace(" ", "");
            string bnfy_typ_cd = record.BNFY_TYP_CD.ToLower().Trim().Replace(" ", "");
            string bnfy_rel_insrd_cd = record.BNFY_REL_INSRD_CD.ToLower().Trim().Replace(" ", "");

            if (context.Beneficiaries.Count() > 0)
            {
                List<TBENE> beneficiariies = context.Beneficiaries.Where(x => x.POL_ID.ToLower().Trim().Replace(" ", "") == policyID
                   && x.BNFY_NM.ToLower().Trim().Replace(" ", "") == beneficiaryName && x.BNFY_TYP_CD.ToLower().Trim().Replace(" ", "") ==
                     bnfy_typ_cd && x.BNFY_REL_INSRD_CD.ToLower().Trim().Replace(" ", "") == bnfy_rel_insrd_cd).ToList();

                if (beneficiariies.Count > 0) isDuplicate = true;

            }

            return isDuplicate;
        }

        private bool checkForDuplicate(EFContext context, TCDOC record)
        {
            bool isDuplicate = false;

            string policyID = record.POL_ID.ToLower().Trim().Replace(" ", "");
            string cli_DOC_ID = record.CLI_DOC_ID.ToLower().Trim().Replace(" ", "");

            if (context.PolicyDocuments.Count() > 0)
            {
                List<TCDOC> documents = context.PolicyDocuments.Where(x => x.POL_ID.ToLower().Trim() == policyID
                                && x.CLI_DOC_ID.ToLower().Trim() == cli_DOC_ID).ToList();

                if (documents.Count > 0) isDuplicate = true;
            }

            return isDuplicate;
        }

        private bool checkForDuplicate(EFContext context, TCVG record)
        {
            bool isDuplicate = false;
            string policyID = record.POL_ID.ToLower().Trim();
            string cli_typ_cd = record.ClI_TYP_CD.ToLower().Trim();
            string cvg_plan_id = record.CVG_PLAN_ID.ToLower().Trim();

            if (context.Coverages.Count() > 0)
            {
                List<TCVG> coverages = context.Coverages.Where(x => x.POL_ID.ToLower().Trim() == policyID
                  && x.ClI_TYP_CD == cli_typ_cd && x.CVG_PLAN_ID.ToLower().Trim() == cvg_plan_id).ToList();

                if (coverages.Count > 0) isDuplicate = true;
            }

            return isDuplicate;
        }

        private bool is_Beneficiary_Related_To_PrimaryInsured(RelationModel relationModel)
        {
            bool isPrimaryInsured = false;

            string[] split = relationModel.RelatedObjectID.Split('_', ',', ';', '?');

            foreach (string s in split)
            {
                if (s.ToUpper().Contains("PI"))
                {
                    isPrimaryInsured = true;
                    break;
                }
            }

            return isPrimaryInsured;
        }

        private bool is_Beneficiary_Related_To_PrimaryInsured_Using_LifeParticpant(Policy policy, PartyModel party)
        {
            LifeParticipantModel? lifeParticipantModel = null;
            bool isPrimary = false;
            Util.Utils utils = new Util.Utils();

            if (policy.LifeModels.Count > 0)
            {
                if (policy.LifeModels.First().CoverageModels.Count > 0)
                {
                    CoverageModel coverageModel = policy.LifeModels.First().CoverageModels.First();

                    if (coverageModel.LifeParticipants.Count > 0)
                    {
                        lifeParticipantModel = coverageModel.LifeParticipants.Where(x => x.PartyID.ToLower() == party.PartyID.ToLower()).First();
                        if (lifeParticipantModel != null)
                        {

                            string[] s = lifeParticipantModel.PartyID.Split(' ', '-', '_');
                            foreach (string s2 in s)
                            {
                                if (s2.ToUpper().Contains("PI"))
                                {
                                    isPrimary = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return isPrimary;
        }

        private List<AgentModel> getAgentPartyModels(List<RelationModel> relationModels, List<PartyModel> partyModels)
        {
            List<RelationModel> agentRelations = new List<RelationModel>();
            List<AgentModel> agentModels = new List<AgentModel>();
            foreach (RelationModel relationModel in relationModels)
            {
                string[] splitWords = relationModel.RelationRoleCode.Split(' ', ';', ',', ':', '.', '?', '|');
                foreach (string word in splitWords)
                {
                    if (word.ToLower().Contains("agent"))
                    {
                        agentRelations.Add(relationModel);
                    }
                }
            }

            foreach (RelationModel agentRelationModel in agentRelations)
            {
                bool doesPartyModelExists = partyModels.Where(x => x.PartyID.ToLower().Trim() == agentRelationModel.RelatedObjectID.ToLower().Trim()).ToList().Count() > 0 ? true : false;
                if (doesPartyModelExists)
                {
                    PartyModel agentPartyModel = partyModels.Where(x => x.PartyID.ToLower().Trim() == agentRelationModel.RelatedObjectID.ToLower().Trim()).First();

                    string agentFirstName = string.Empty;
                    string agentLastName = string.Empty;
                    if (agentPartyModel.Persons.Count > 0)
                    {
                        agentFirstName = agentPartyModel.Persons.First().FirstName;
                        agentLastName = agentPartyModel.Persons.First().LastName;
                    }

                    AgentModel agentModel = new AgentModel(agentPartyModel.ProducerModel.CompanyProducerID, agentRelationModel.InterestPercent, agentFirstName, agentLastName);
                    agentModels.Add(agentModel);
                }
            }
            return agentModels;
        }

        private List<ClientModel> getClients(List<PartyModel> partyModels, List<RelationModel> relationModels)
        {
            List<ClientModel> clientModels = new List<ClientModel>();
            OwnerModel ownerModel = getPrimaryOwner(relationModels, partyModels);
            string id_Owner = "owner";
            bool doesRelationExists = doesObjectExists(ownerModel, id_Owner, false);

            foreach (RelationModel relationModel in relationModels)
            { 
                // Get Party information based on relation model 
                List<PartyModel> insuredPartyModels = partyModels.Where(x => x.PartyID.ToLower() == relationModel.RelatedObjectID.ToLower()).ToList();

                foreach (PartyModel partyModel in insuredPartyModels)
                {
                    foreach (PersonModel personModel in partyModel.Persons)
                    {
                        List<AgentModel> agents = getAgentPartyModels(relationModels, partyModels);
                        string agentName = string.Empty;

                        if (agents.Count > 0)
                            agentName = string.Format("{0} {1}", agents.First().FirstName, agents.First().LastName);

                        string idParty_PrimaryInsured = "Party_PrimaryInsured";
                        ClientType clientType = ClientType.None;
                        if (partyModel.PartyID.ToLower() == idParty_PrimaryInsured.ToLower())
                            clientType = ClientType.PrimaryInsured;

                        ClientModel clientModel = new ClientModel(personModel.FirstName, personModel.LastName, partyModel.NIBNumber, personModel.BirthDate, agentName, partyModel.PartyID, clientType, ownerModel, doesRelationExists);
                        clientModels.Add(clientModel);
                    }
                }
            }

            return clientModels;
        }


        private PartyModel getPrimaryInsured(List<PartyModel> partyModels)
        {
            string id_PartyInsured = "Party_PrimaryInsured";
            PartyModel primaryInsuredModel = partyModels.Where(x => x.PartyID.ToLower() == id_PartyInsured.ToLower()) != null ? partyModels.Where(x => x.PartyID.ToLower() == id_PartyInsured.ToLower()).First() : null;
            return primaryInsuredModel;
        }

        private OwnerModel getPrimaryOwner(List<RelationModel> relationalModels, List<PartyModel> partyModels)
        {
            RelationModel? ownerRelation = null;
            bool found = false;
            OwnerModel ownerModel = null;

            bool foundUnwantedWords = false;



            foreach (RelationModel relationModel in relationalModels)
            {
                string[] splitWords = relationModel.RelationRoleCode.Split(' ', ';', ',', ':', '.', '?', '|');
                int wordCounter = 0;
                found = false;
                foundUnwantedWords = false;

                foreach (string word in splitWords)
                {
                    if (word.ToLower().Contains("owner"))
                    {
                        ownerRelation = relationModel;
                        found = true;
                    }

                    if (word.ToLower().Contains("contingent"))
                    {
                        foundUnwantedWords = true;
                    }

                    wordCounter++;
                    if (found && !foundUnwantedWords && wordCounter == splitWords.Length)
                    {
                        ownerRelation = relationModel;
                        break;
                    }
                }
                if (ownerRelation != null) break;


            }

            if (found && ownerRelation != null)
            {
                List<RelationModel> foundRelations = relationalModels.Where(x => x.RelatedObjectID == ownerRelation.RelatedObjectID).ToList();
                bool doesPartyModelExists = partyModels.Where(x => x.PartyID.ToLower().Trim() == ownerRelation?.RelatedObjectID.ToLower().Trim()).ToList().Count() > 0 ? true : false;
                if (doesPartyModelExists)
                {
                    PartyModel ownerPartyModel = partyModels.Where(x => x.PartyID.ToLower().Trim() == ownerRelation?.RelatedObjectID.ToLower().Trim()).First();
                    if (ownerPartyModel.Persons.Count > 0)
                    {
                        PersonModel ownerPersonModel = ownerPartyModel.Persons.First();

                        ownerModel = new OwnerModel(ownerPersonModel.FirstName, ownerPersonModel.LastName, ownerPartyModel.Address.AddressCountryTC, ownerPersonModel.IsBirthDateValid, ownerPersonModel.BirthDate, ownerPartyModel.Address, ownerPartyModel, foundRelations);
                    }
                    else
                    {
                        if (ownerPartyModel.Address != null)
                        {
                            ownerModel = new OwnerModel(ownerPartyModel.PartyFullName, ownerPartyModel.Address.AddressCountryTC, false, ownerPartyModel.Address, ownerPartyModel, foundRelations);
                        }
                    }
                }
            }

            return ownerModel;
        }


        private ContingentOwnerModel getContingentOwner(List<RelationModel> relationModels, List<PartyModel> partyModels)
        {
            RelationModel? contingentRelation = null;
            bool found = false;
            ContingentOwnerModel contingentOwnerModel = new ContingentOwnerModel();

            foreach (RelationModel relationModel in relationModels)
            {
                string[] splitWords = relationModel.RelationRoleCode.Split(' ', ';', ',', ':', '.', '?', '|');

                foreach (string word in splitWords)
                {
                    if (word.ToLower().Contains("contingent"))
                    {
                        contingentRelation = relationModel;
                        found = true;
                        break;
                    }
                }

                if (found) break;
            }

            if (found)
            {
                bool doesPartyModelExists = partyModels.Where(x => x.PartyID.ToLower().Trim() == contingentRelation?.RelatedObjectID.ToLower().Trim()).ToList().Count() > 0 ? true : false;
                if (doesPartyModelExists)
                {
                    PartyModel contingentPartyModel = partyModels.Where(x => x.PartyID.ToLower().Trim() == contingentRelation?.RelatedObjectID.ToLower().Trim()).First();

                    if (contingentPartyModel.Persons.Count > 0)
                    {
                        PersonModel contingentPersonModel = contingentPartyModel.Persons.First();

                        if (contingentRelation != null && relationModels != null)
                        {
                            bool doesRelationModelExists = doesMainRelationalModelExists(relationModels, contingentRelation);

                            if (doesRelationModelExists)
                            {
                                RelationModel? mainRelationModel = relationModels.Where(x => x.RelatedObjectID == contingentRelation.RelatedObjectID)?.First();

                                string description = mainRelationModel != null ? mainRelationModel.RelationDescription : string.Empty;
                                string roleCode = mainRelationModel != null ? mainRelationModel.RelationRoleCode : string.Empty;

                                contingentOwnerModel = new ContingentOwnerModel(contingentPersonModel.FirstName, contingentPersonModel.LastName, contingentPersonModel.BirthDate, description, roleCode, contingentPersonModel.IsBirthDateValid, contingentPartyModel);
                                contingentOwnerModel.WasContingentOwnerFound = true;
                            }
                        }
                    }
                }
            }
            else
            {
                OwnerModel ownerModel = getPrimaryOwner(relationModels, partyModels);
                contingentOwnerModel = new ContingentOwnerModel(ownerModel.PartyFulllName, string.Empty, DateTime.Now.Date, string.Empty, string.Empty, ownerModel.IsBirthDateValid, null);
                contingentOwnerModel.WasContingentOwnerFound = false;
            }

            return contingentOwnerModel;
        }

        private bool doesMainRelationalModelExists(List<RelationModel> relationModels, RelationModel contingentRelation)
        {
            bool doesRelationModelExists = false;

            foreach (RelationModel relationModel in relationModels)
            {
                if(relationModel.RelatedObjectID.ToLower() == contingentRelation.RelatedObjectID.ToLower())
                {
                    doesRelationModelExists = true;
                    break;
                }
            }

            return doesRelationModelExists;
        }

        private string getCurrentUserName()
        {
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            return userName;
        }

        private bool doesObjectExists(OwnerModel ownerModel, string relationRoleCode, bool findIfRoleExists)
        {
            bool exists = false;
            if (ownerModel != null)
            {
                foreach (RelationModel relationModel in ownerModel.AllRelations)
                {
                    if (findIfRoleExists)
                    {
                        if (relationModel.RelationRoleCode.ToLower() == relationRoleCode.ToLower())
                            exists = true;
                    }
                    else
                    {
                        if (relationModel.RelationRoleCode.ToLower() != relationRoleCode.ToLower())
                            exists = true;
                    }
                }
            }

            return exists;
        }

        #endregion

        #region Recovery Archive XML Save 

        private void saveXMLArchive(ReadConfig readConfig, XElement xml)
        {
            string dateFileFormat = DateTime.Now.Month + "-" + DateTime.Now.Day + "-" + DateTime.Now.Year + "-" + DateTime.Now.Minute + DateTime.Now.Second;
            string archiveXMLfileName = string.Format("{0}{1}.xml", "iPipeline", dateFileFormat);
            string archiveFolderPath = readConfig.StagingPath + "\\" + "Archive";
            if (!Directory.Exists(archiveFolderPath)) Directory.CreateDirectory(archiveFolderPath);
            if (!archiveFolderPath.EndsWith("\\")) archiveFolderPath += "\\";
            string archiveFilePath = archiveFolderPath + archiveXMLfileName;
            xml.Save(archiveFilePath);
        }

        private void saveXMLArchivePolicy(ReadConfig readConfig, XElement xml, string policyNumber)
        {
            string archiveXMLfileName = string.Format("{0}_{1}.xml", "iPipeline", policyNumber);
            string archiveFolderPath = readConfig.StagingPath + "\\" + policyNumber;
            if (!Directory.Exists(archiveFolderPath)) Directory.CreateDirectory(archiveFolderPath);
            if (!archiveFolderPath.EndsWith("\\")) archiveFolderPath += "\\";
            string archiveFilePath = archiveFolderPath + archiveXMLfileName;
            xml.Save(archiveFilePath);
        }

        #endregion 

        #region DocuWare Helper Functions
        private FileCabinet connectToDocuWare()
        {
            ReadConfig readConfig = new ReadConfig();
            readConfig.GetSettings();

            DWSettings dWSettings = new DWSettings();
            dWSettings.DocURI = readConfig.DW_Login_URI;
            dWSettings.DWUser = readConfig.DW_Login_USER;
            dWSettings.DWPassword = readConfig.DW_Login_Password;
            ServiceConnection serviceConnection = DWConnection.Account.DWLogin(dWSettings);

            var org = serviceConnection.Organizations[0];
            List<FileCabinet> fileCabinets = org.GetFileCabinetsFromFilecabinetsRelation().FileCabinet;
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            FileCabinet foundCabinet = fileCabinets.Find(x => x.Name.ToLower() == readConfig.DW_CabinetName.ToLower());
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            return foundCabinet;
          
        }

        #endregion 
    }

}

