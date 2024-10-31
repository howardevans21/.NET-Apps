using DWPipelineWebService.DataModel.Holding.ChildrenModels;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Security.Cryptography.Xml;
using System.Xml.Linq;


namespace DWPipelineWebService.Util
{
    partial class XMLMainReader
    {

        internal partial class XMLLifeReader
        {

            internal class XMLRelationReader
            {
                /*********************************************
                 * Main IDs for Relation 
                 ****************************************************/
                private string id_RelationKey = string.Empty;
                private string id_OriginatingObjectID = string.Empty;
                private string id_main = string.Empty;

                /******************************************
                 * Child IDs for relaiton
                 ************************************************/
                private string id_originatingObjectType = string.Empty;
          
                private string id_relationRoleCode = string.Empty;
                private string id_interestPercent = string.Empty;
                private string id_IrrevokableInd = string.Empty;
                private string id_RelationDescription = string.Empty;
                private string id_RelatedObjectID = string.Empty;
                private string id_RelatedObjectType = string.Empty;
                private string id_DistributionOption = string.Empty;

                public XMLRelationReader(string id_main, string id_RelationKey, string id_OriginationObjectID,  string id_originatingObjectType, string id_relationRoleCode,
                    string id_interestPercent, string id_IrrevokableInd, string id_RelationDescription, string id_RelatedObjectID, string id_RelatedObjectType, string id_DistributionOption)
                {
                    this.id_RelationKey = id_RelationKey;
                    this.id_OriginatingObjectID = id_OriginationObjectID;
                    this.id_main = id_main;
                    this.id_originatingObjectType = id_originatingObjectType;
                    
                    this.id_relationRoleCode = id_relationRoleCode;
                    this.id_interestPercent = id_interestPercent;
                    this.id_IrrevokableInd = id_IrrevokableInd;
                    this.id_RelationDescription = id_RelationDescription;
                    this.id_RelatedObjectID = id_RelatedObjectID;
                    this.id_RelatedObjectType = id_RelatedObjectType;
                    this.id_DistributionOption = id_DistributionOption;
                }

                public List<RelationModel> GetRelationData(IEnumerable<XNode> relationNodes)
                {
                    Utils util = new Utils();

                    List<RelationModel> relationModels = new List<RelationModel>();

                    foreach (XNode relationNode in relationNodes)
                    {
                        if (relationNode is XElement) 
                        {
                            XElement xdn = (XElement)relationNode;

                            // Party ID Link to Party Element
                            bool doesRelationIDExists = util.doesAttributeExists(xdn, id_RelatedObjectID);
                            string relatedObjectID = doesRelationIDExists ? xdn.Attributes().Where(x => x.Name.LocalName.ToLower() == id_RelatedObjectID.ToLower()).First().Value : string.Empty;

                            // Get Relation Object ID
                            // This ID relates to the Party ID to establish a relationship
                            bool does_RelationKey = util.doesAttributeExists(xdn, id_RelationKey);
                            bool does_RelationSysKey = util.doesAttributeExists(xdn, id_OriginatingObjectID);
                            bool does_InterestPercentExists = util.doesXMLElementExists(xdn, id_interestPercent);

                            string relationKey = does_RelationKey ? xdn.Attributes().Where(x => x.Name.LocalName.ToLower() == id_RelationKey.ToLower()).First().Value : string.Empty;
                            string originatingObjectID = does_RelationSysKey ? xdn.Attributes().Where(x => x.Name.LocalName.ToLower() == id_OriginatingObjectID.ToLower()).First().Value : string.Empty;
                           

                            string interestPercent = does_InterestPercentExists ? xdn.Elements().Where(x => x.Name.LocalName.ToLower() == id_interestPercent.ToLower()).First().Value : string.Empty;
                            double interestPercentDouble = 0; 
                            double.TryParse(interestPercent, out interestPercentDouble);

                            /*****************************************************************
                             * Detailed Relation Information
                               This information can be used to determine the relationCode for a Party
                             ***********************************************************************/
                            bool doesRelationCode = util.doesXMLElementExists(xdn, id_relationRoleCode);
                            string relationCode = doesRelationCode ? xdn.Elements().Where(x => x.Name.LocalName.ToLower() == id_relationRoleCode.ToLower()).First().Value : string.Empty;

                            /********************************************************
                             * More Relation Data
                             ******************************************************************/
                            bool does_IrrevokableInd = util.doesXMLElementExists(xdn, id_IrrevokableInd);
                            string irrevokableInd = does_IrrevokableInd ? xdn.Elements().Where(x => x.Name.LocalName.ToLower() == id_IrrevokableInd.ToLower()).First().Value : string.Empty;

                            bool doesRelationDescription = util.doesXMLElementExists(xdn, id_RelationDescription);
                            string relationDescription = doesRelationDescription ? xdn.Elements().Where(x => x.Name.LocalName.ToLower() == id_RelationDescription.ToLower()).First().Value : string.Empty;

                            bool doesOriginatingObjectTypeExists = util.doesXMLElementExists(xdn, id_originatingObjectType);
                            string originatingObjectType = doesOriginatingObjectTypeExists ? xdn.Elements().Where(x => x.Name.LocalName.ToLower() == id_originatingObjectType.ToLower()).First().Value : string.Empty;

                            bool doesRelatedObjectTypeExists = util.doesXMLElementExists(xdn, id_RelatedObjectType);
                            string relatedObjectType = doesRelatedObjectTypeExists ? xdn.Elements().Where(x => x.Name.LocalName.ToLower() == id_RelatedObjectType.ToLower()).First().Value : string.Empty;

                            RelationModel model = new RelationModel(relationKey, relatedObjectID, originatingObjectID, originatingObjectType, relatedObjectType, relationCode, relationDescription);
                            model.InterestPercent = interestPercentDouble;
                            model.IrrevokableInd = irrevokableInd;

                            /*************************************
                             * DistributionOption used to determine if Beneficiary
                             * percentages should be calculated if they 
                             * are equally distributed
                             *****************************************/
                            bool doesRelatedDistributionOptionExists = util.doesXMLElementExists(xdn, id_DistributionOption);
                            string relatedDistributionOption = doesRelatedDistributionOptionExists ? xdn.Elements().Where(x => x.Name.LocalName.ToLower() == id_DistributionOption.ToLower()).First().Value : string.Empty;
                            model.DistributionOption = relatedDistributionOption;

                            if (doesRelationIDExists)
                            {
                                relationModels.Add(model);
                            }
                        }
                    }

                    return relationModels;
                }
            }
        }
    }
}
