using DWiPipeline.DataModel;
using DWPipelineWebService.DataModel;
using System.Collections.Frozen;
using System.Xml.Linq;

namespace DWPipelineWebService.Util
{
    partial class XMLMainReader
    {
        internal partial class XMLHoldingReader
        {

            internal partial class XMLPolicyReaderGetLifeParticipantData
            {
                internal partial class XMLLifeParticipantReader
                {
                    private string id_ParticipationPct = string.Empty;
                    private string id_Party = string.Empty;
                    private string id_SmokerStat = string.Empty;
                    private string id_IssueGender = string.Empty;
                    private string id_IssueAge = string.Empty;
                    private string id_LifeParticipantRoleCode = string.Empty;

                    public XMLLifeParticipantReader(string id_ParticipationPct, string id_Party, string id_SmokerStat, string id_issueGender, string id_IssueAge, string id_LifeParticipantRoleCode)
                    {
                        this.id_ParticipationPct = id_ParticipationPct;
                        this.id_Party = id_Party;
                        this.id_SmokerStat = id_SmokerStat;
                        this.id_IssueGender = id_issueGender;
                        this.id_IssueAge = id_IssueAge;
                        this.id_LifeParticipantRoleCode =id_LifeParticipantRoleCode;
                    }

                    public List<LifeParticipantModel> GetLifeParticipantData(XElement xdnLifeParticipant)
                    {
                        Utils util = new Utils();

                        List<LifeParticipantModel> lifeParticipantModels = new List<LifeParticipantModel>();    

                        /********************************************************************************************************
                            LIFE PARTCIPANT DATA
                       ********************************************************************************************************/
                        IEnumerable<XElement> lifeRequestElements = xdnLifeParticipant.Descendants().Where(x => x.Name.LocalName.ToLower() == id_ParticipationPct);
                        IEnumerable<XNode> lifeNodes = from lifeNode in lifeRequestElements.DescendantNodesAndSelf() select lifeNode;

                        double lifeParticipantPercentage = 0;
                        foreach (XNode lifeNode in lifeNodes)
                        {
                            XElement xdnLife = (XElement)lifeNode;

                            // Party ID 
                            bool exists_PartyID = util.doesAttributeExists(xdnLife, id_Party);
                            string partyID = exists_PartyID ? xdnLife.Elements().Where(x => x.Name.LocalName.ToLower() == id_Party).First().Value : string.Empty;

                            if (partyID.Length > 0)
                            {
                                // Smoker Stat 
                                bool existsSmokerStat = util.doesXMLElementExists(xdnLife, id_SmokerStat);
                                string smokerStat = existsSmokerStat ? xdnLife.Elements().Where(x => x.Name.LocalName.ToLower() == id_SmokerStat).First().Value : string.Empty;

                                // Life Face Amount 
                                bool exists_LifeParticipantPct = util.doesXMLElementExists(xdnLife, id_ParticipationPct);
                                string lifeParticipantPct = exists_LifeParticipantPct ? xdnLife.Elements().Where(x => x.Name.LocalName.ToLower() == id_ParticipationPct).First().Value : string.Empty;
                                double lifeParticipantDouble = 0;
                                double.TryParse(lifeParticipantPct, out lifeParticipantDouble);
                                lifeParticipantPercentage = lifeParticipantDouble;

                                // Life Participant Role Code 
                                bool exists_LifeParticipantRoleCode = util.doesXMLElementExists(xdnLife, id_LifeParticipantRoleCode);
                                string lifeParticipantRoleCode = exists_LifeParticipantRoleCode ? xdnLife.Elements().Where(x => x.Name.LocalName.ToLower() == id_LifeParticipantRoleCode).First().Value : string.Empty;

                                // Issue Age 
                                bool exists_IssueAge = util.doesXMLElementExists(xdnLife, id_IssueAge);
                                string issueAgs = exists_IssueAge ? xdnLife.Elements().Where(x => x.Name.LocalName.ToLower() == id_IssueAge).First().Value : string.Empty;
                                int issueAge = -1;
                                int.TryParse(issueAgs, out issueAge);

                                // Issue Gender 
                                bool existsIssueGender = util.doesXMLElementExists(xdnLife, id_IssueGender);
                                string issueGender = existsIssueGender ? xdnLife.Elements().Where(x => x.Name.LocalName.ToLower() == id_IssueGender).First().Value : string.Empty;

                                // Life Participant Model 

                                LifeParticipantModel lifeParticipantModel = new LifeParticipantModel(partyID, smokerStat, lifeParticipantRoleCode, issueAge, issueGender, lifeParticipantDouble);
                                lifeParticipantModels.Add(lifeParticipantModel);
                            }
                        }

                        return lifeParticipantModels; 
                    }
                }
            }
        }
    }
}
