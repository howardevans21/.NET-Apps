
using DWiPipeline.DataModel;
using DWPipelineWebService.DataModel;
using DWPipelineWebService.DataModel.Holding;
using System.Globalization;
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
                        internal partial class XMLLifeParticipantReader
                        {
                            private string id_lifeParticipant = string.Empty;
                            private string id_PartyID = string.Empty;
                            private string id_lifeparticipantrolecode = string.Empty;
                            private string id_issueAge = string.Empty;
                            private string id_issueGender = string.Empty;
                            private string id_tobaccopremiumbasis = string.Empty;
                            private string id_underwritingclass = string.Empty;
                            private string id_SmokerStat = string.Empty;
                            
                            public XMLLifeParticipantReader(string id_lifeParticipant, string id_PartyID, string id_lifeparticipantrolecode, string id_issueAge, string id_issueGender, string id_tobaccopremiumbasis, string id_underwritingclass, string id_SmokerStat)
                            {
                                this.id_lifeParticipant = id_lifeParticipant;
                                this.id_PartyID = id_PartyID;
                                this.id_lifeparticipantrolecode = id_lifeparticipantrolecode;
                                this.id_issueAge = id_issueAge;
                                this.id_issueGender = id_issueGender;
                                this.id_tobaccopremiumbasis = id_tobaccopremiumbasis;
                                this.id_underwritingclass = id_underwritingclass;
                                this.id_SmokerStat= id_SmokerStat;
                            }

                            public List<LifeParticipantModel> GetLifeParticipantData(XElement xdnCoverage)
                            {
                                Utils util = new Utils();
                                List<LifeParticipantModel> lifeParticipantModels = new List<LifeParticipantModel>();

                                IEnumerable<XElement> lifeParticipantElements = xdnCoverage.Descendants().Where(x => x.Name.LocalName.ToLower() == id_lifeParticipant);
                                IEnumerable<XNode> lifeParticipantNodes = from lifeParticipantNode in lifeParticipantElements.DescendantNodesAndSelf() select lifeParticipantNode;
                                foreach (XNode xNode in lifeParticipantNodes)
                                {
                                    if (xNode is XElement)
                                    {
                                        XElement xdnLifeParticipantNode = (XElement)xNode;

                                        bool doesPartyIDExists = util.doesAttributeExists(xdnLifeParticipantNode, id_PartyID);
                                        string partyID = doesPartyIDExists ? xdnLifeParticipantNode.Attributes().Where(x => x.Name.LocalName.ToLower() == id_PartyID.ToLower()).First().Value : string.Empty;

                                        if (doesPartyIDExists)
                                        {
                                            bool exists_SmokerStat = util.doesXMLElementExists(xdnLifeParticipantNode, id_SmokerStat);
                                            bool exists_participantcode = util.doesXMLElementExists(xdnLifeParticipantNode, id_lifeparticipantrolecode);
                                            bool exists_issueAge = util.doesXMLElementExists(xdnLifeParticipantNode, id_issueAge);
                                            bool exists_issueGender = util.doesXMLElementExists(xdnLifeParticipantNode, id_issueGender);
                                            bool exists_tobaccopremiumbasis = util.doesXMLElementExists(xdnLifeParticipantNode, id_tobaccopremiumbasis);
                                            bool exists_underwritingClass = util.doesXMLElementExists(xdnLifeParticipantNode, id_underwritingclass);

                                            string smokerStat = exists_SmokerStat ? xdnLifeParticipantNode.Elements().Where(x => x.Name.LocalName.ToLower() == id_SmokerStat.ToLower()).First().Value : string.Empty;
                                            string lifeParticipantCode = exists_participantcode ? xdnLifeParticipantNode.Elements().Where(x => x.Name.LocalName.ToLower() == id_lifeparticipantrolecode.ToLower()).First().Value : string.Empty;

                                            string issueAge = exists_issueAge ? xdnLifeParticipantNode.Elements().Where(x => x.Name.LocalName.ToLower() == id_issueAge.ToLower()).First().Value : string.Empty;
                                            int issueAgeInt = 0;
                                            int.TryParse(issueAge, out issueAgeInt);

                                            string issueGender = exists_issueAge ? xdnLifeParticipantNode.Elements().Where(x => x.Name.LocalName.ToLower() == id_issueGender.ToLower()).First().Value : string.Empty;
                                            string underwritingclass = exists_underwritingClass ? xdnLifeParticipantNode.Elements().Where(x => x.Name.LocalName.ToLower() == id_underwritingclass.ToLower()).First().Value : string.Empty;

                                            /*******************************************************
                                             * Life Participant Models 
                                             ********************************************************/
                                            LifeParticipantModel lifeParticipant = new LifeParticipantModel(partyID, smokerStat, lifeParticipantCode, issueAgeInt, issueGender,0);
                                            lifeParticipantModels.Add(lifeParticipant);
                                        }
                                    }
                                }

                                return lifeParticipantModels;
                            }
                        }
                    }
                }
            }

        }
    }
}