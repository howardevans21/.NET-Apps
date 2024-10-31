
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
                    private string id_Life = string.Empty;
                    private string id_FaceAmt = string.Empty;
                    private string id_DivType = string.Empty; 

                    public XMLLifeReader(string id_ParentElement, string id_FaceAmt, string id_DivType)
                    {
                        this.id_Life = id_ParentElement;
                        this.id_FaceAmt = id_FaceAmt;
                        this.id_DivType = id_DivType;
                    }

                    public List<LifeModel> GetLifeData(XElement xdnPolicy)
                    {
                        Utils util = new Utils();

                        List<LifeModel> lifeModels = new List<LifeModel>();

                        /********************************************************************************************************
                             LIFE DATA
                        ********************************************************************************************************/
                        IEnumerable<XElement> lifeRequestElements = xdnPolicy.Descendants().Where(x => x.Name.LocalName.ToLower() == id_Life);
                        IEnumerable<XNode> lifeNodes = from lifeNode in lifeRequestElements.DescendantNodesAndSelf() select lifeNode;

                        foreach (XNode lifeNode in lifeNodes)
                        {
                            if (lifeNode is XElement)
                            {
                                XElement xdnLife = (XElement)lifeNode;

                                // Life Face Amount 
                                bool exists_FaceAmt = util.doesXMLElementExists(xdnLife, id_FaceAmt);
                                string faceAmt = exists_FaceAmt ? xdnLife.Elements().Where(x => x.Name.LocalName.ToLower() == id_FaceAmt).First().Value : string.Empty;
                                decimal faceAmtDecimal = 0;
                                decimal.TryParse(faceAmt, out faceAmtDecimal);

                                // Life Div Type 
                                bool exists_DivType = util.doesXMLElementExists(xdnLife, id_DivType);
                                string divType = exists_DivType ? xdnLife.Elements().Where(x => x.Name.LocalName.ToLower() == id_DivType).First().Value : string.Empty;

                               XMLCoverageReader xMLCoverageReader = new XMLCoverageReader("coverage", "id", "currentamt", "productcode", "initcovtamt", "indicatorCode");
                               List<CoverageModel> coverageModels = xMLCoverageReader.GetCoverageData(lifeRequestElements);

                                /****************************************
                                 * Populate life models 
                                 *****************************************/
                                LifeModel lifeModel = new LifeModel(faceAmtDecimal, divType, coverageModels);
                                lifeModels.Add(lifeModel);

                                /*********************************************
                                     * Break the loop for faster processing
                                    * Already obtained Policy.Life data; no need to look for its descendants
                                    * since that will be in another process
                                ************************************************/
                                break;
                            }
                        }
                        
                        return lifeModels;
                    }
                }
            }     
        }
    }
}