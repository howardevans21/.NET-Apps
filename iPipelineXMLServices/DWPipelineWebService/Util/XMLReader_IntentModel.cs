using DWPipelineWebService.DataModel;
using System.Linq;
using System.Xml.Linq;

namespace DWPipelineWebService.Util 
{
    partial class XMLMainReader
    {
        internal partial class XMLHoldingReader 
        {
          
            internal partial class XMLIntentReader
            {
                private string id_ParentElement = string.Empty;
                private string id_expenseNeedKey = string.Empty;
                private string id_expenseNeedTypeCode = string.Empty;

                public XMLIntentReader(string id_ParentElement, string id_expenseNeedKey, string id_expenseNeedTypeCode)
                {
                    this.id_ParentElement = id_ParentElement;
                    this.id_expenseNeedKey = id_expenseNeedKey;
                    this.id_expenseNeedTypeCode =id_expenseNeedTypeCode;
                }

                public IntentModel GetIntentData(XElement xdn)
                {
                    /*********************************************
                     * INTENT DATA 
                     ************************************************/
                  
                    IEnumerable<XElement> intentElements = xdn.Descendants().Where(x => x.Name.LocalName.ToLower() == id_ParentElement.ToLower());
                    IEnumerable<XNode> intentNodes = from intentNode in intentElements select intentNode;

                    IntentModel intentModel = new IntentModel();

                    foreach(XNode xNode in intentNodes)
                    {
                        XElement xdnIntent = (XElement)xNode;

                        Utils util = new Utils();

                        bool exists_ExpenseNeedKey = util.doesXMLElementExists(xdnIntent, id_expenseNeedKey);
                        string expenseNeedKey = exists_ExpenseNeedKey ? xdnIntent.Elements().Where(x => x.Name.LocalName.ToLower() == id_expenseNeedKey).First().Value : string.Empty;

                        bool exists_ExpenseNeedTypeCode = util.doesXMLElementExists(xdnIntent, id_expenseNeedTypeCode);
                        string expenseNeedTypeCode = exists_ExpenseNeedTypeCode ? xdnIntent.Elements().Where(x => x.Name.LocalName.ToLower() == id_expenseNeedTypeCode).First().Value : string.Empty;

                        intentModel.ExpenseNeedKey = expenseNeedKey;
                        intentModel.ExpenseNeedTypeCode = expenseNeedTypeCode;
                     
                        break; 
                    }

                    return intentModel;
                }
            }
        }
    }
   
}
