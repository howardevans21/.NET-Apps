using DWiPipeline.DataModel;
using DWPipelineWebService.DataModel;
using System.Xml.Linq;


namespace DWPipelineWebService.Util
{
    partial class XMLMainReader
    {
        internal partial class XMLHoldingReader
        {
            public class XMLArrangementReader
            {
                private string id_MainElement = string.Empty;
                private string id_PaymentMethod = string.Empty;
                private string id_ArrMode = string.Empty;

                public XMLArrangementReader(string id_MainElement, string id_PaymentMethod, string id_ArrMode)
                {
                    this.id_MainElement = id_MainElement;
                    this.id_PaymentMethod = id_PaymentMethod;
                    this.id_ArrMode = id_ArrMode;
                }

                public ArrangementModel GetArrangementData(XElement xdn)
                {
                    /********************************************************************************************************
                    Arrangement  DATA
                   ********************************************************************************************************/
                    IEnumerable<XElement> arrangementElements = xdn.Descendants().Where(x => x.Name.LocalName.ToLower() == id_MainElement.ToLower());
                    IEnumerable<XNode> arrangementNodes = from holdingNode in arrangementElements.DescendantNodesAndSelf() select holdingNode;

                    ArrangementModel arrangementModel = new ArrangementModel();
                    foreach(XNode node in arrangementNodes)
                    {
                        if (node is XElement)
                        {
                            XElement xdnArrangement = (XElement)node;

                            Utils util = new Utils();
                            bool exists_paymentMethod = util.doesXMLElementExists(xdnArrangement, id_PaymentMethod);
                            string paymentMethod = exists_paymentMethod ? xdnArrangement.Elements().Where(x => x.Name.LocalName.ToLower() == id_PaymentMethod.ToLower()).First().Value : string.Empty;
                            arrangementModel.PaymentMethod = paymentMethod;

                            bool exists_ArrMode = util.doesXMLElementExists(xdnArrangement, id_ArrMode);
                            string arrMode = exists_ArrMode ? xdnArrangement.Elements().Where(x => x.Name.LocalName.ToLower() == id_ArrMode.ToLower()).First().Value : string.Empty;
                            arrangementModel.ArrMode = arrMode; 

                            break;
                        }
                    }

                    return arrangementModel;
                }
            }
        }
    }
}


