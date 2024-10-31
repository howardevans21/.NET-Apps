using System.Xml.Linq;

namespace DWPipelineWebService.Util
{
    partial class XMLMainReader 
    {
        internal partial class XMLGovermentReader
        {
            private string id_MainElement = string.Empty;
            private string id_GovtID = string.Empty;
            private string id_GovtExpDate = string.Empty;
            private string id_Nation = string.Empty;
           

            public XMLGovermentReader(string id_MainElement, string id_GovtID, string id_GovtExpDate, string id_Nation)
            {
                this.id_MainElement = id_MainElement;
                this.id_GovtID = id_GovtID;   
                this.id_GovtExpDate = id_GovtExpDate;
                this.id_Nation = id_Nation;
            }

            public void GetGovernmentData(XElement xdn)
            {
                Utils util = new Utils();

                /********************************************************************************************************
                    Government DATA    
                ********************************************************************************************************/
                IEnumerable<XElement> governmentElements = xdn.Elements().Where(x => x.Name.LocalName.ToLower() == id_MainElement);
                foreach(XElement element in governmentElements)
                {
                    bool exists_GovtID = util.doesXMLElementExists(element, id_GovtID);
                    string govtID = exists_GovtID ? element.Elements().Where(x => x.Name.LocalName.ToLower() == id_GovtID).First().Value : string.Empty;

                    // Government Expiration Date
                    bool exists_GovtExpDate = util.doesXMLElementExists(element, id_GovtExpDate);
                    string govtExpDate = exists_GovtExpDate ? element.Elements().Where(x => x.Name.LocalName.ToLower() == id_GovtExpDate).First().Value : string.Empty; ;
                    DateTime govtExpDate_Date = DateTime.Now;
                    DateTime.TryParse(govtExpDate, out govtExpDate_Date);

                    // Nation 
                    bool exists_Nation = util.doesXMLElementExists(element, id_Nation);
                    string nation = exists_Nation ? element.Elements().Where(x => x.Name.LocalName.ToLower() == id_Nation).First().Value : string.Empty;
                
                
                }
            
            }
        }
    }
}
