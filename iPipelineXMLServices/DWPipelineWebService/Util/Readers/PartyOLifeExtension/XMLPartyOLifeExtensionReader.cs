using System.Xml.Linq;

namespace DWPipelineWebService.Util
{
    partial class XMLMainReader
    {

        internal partial class XMLPartyOLifeExtensionReader
        {

            private string id_MainElement = string.Empty;
            private string id_Nationality = string.Empty;
            private string id_Nationality_Desc = string.Empty;
            private string id_NIB_Number = string.Empty;
            private string id_SSN = string.Empty;
            private string id_TaxIDNumber = string.Empty;

            public XMLPartyOLifeExtensionReader(string id_MainElement, string id_Nationality, string id_Nationality_Desc, string id_NIB_Number, string id_SSN, string id_TaxIDNumber)
            {
                this.id_MainElement = id_MainElement;
                this.id_Nationality = string.Empty;
                this.id_Nationality_Desc = id_Nationality_Desc;
                this.id_NIB_Number = id_NIB_Number;
                this.id_SSN = id_SSN;
                this.id_TaxIDNumber = id_TaxIDNumber;
            }

            public void GetPartyOLifeExtensionData(XElement xdn)
            {
                Utils util = new Utils();
                IEnumerable<XElement> oLifeElements = xdn.Elements().Where(x => x.Name.LocalName.ToLower() == id_MainElement);
                foreach(XElement xElement in oLifeElements)
                {
                    bool exists_nationality = util.doesXMLElementExists(xElement, id_Nationality);
                    string nationality = exists_nationality ? xElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_Nationality.ToLower()).First().Value : string.Empty;

                    bool exists_NationalityDesc = util.doesXMLElementExists(xElement, id_Nationality_Desc);
                    string nationality_Desc = exists_NationalityDesc ? xElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_Nationality_Desc.ToLower()).First().Value : string.Empty;

                    bool exists_NIB_Number = util.doesXMLElementExists(xElement, id_NIB_Number);
                    string nib_Number = exists_NIB_Number ? xElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_NIB_Number.ToLower()).First().Value : string.Empty;

                    bool exists_SSN = util.doesXMLElementExists(xElement, id_SSN);
                    string ssn = exists_SSN ? xElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_SSN.ToLower()).First().Value : string.Empty;

                    bool exists_TaxIDNumber = util.doesXMLElementExists(xElement, id_TaxIDNumber);
                    string taxIDNumber = exists_TaxIDNumber ? xElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_TaxIDNumber.ToLower()).First().Value : string.Empty;
                }
            }
        }
    }
}

