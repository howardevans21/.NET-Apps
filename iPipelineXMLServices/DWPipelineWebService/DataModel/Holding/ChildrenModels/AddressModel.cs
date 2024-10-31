namespace DWPipelineWebService.DataModel
{
    public class AddressModel
    {
        public AddressModel() { }
        public AddressModel(string addressTypeCode, string line1, string line2, string line3, string city, string addressStateTC, string addressState, string zip, string addressCountryTC, string postalDropCode, 
                decimal yearsAtAddress)
        {
            this.addressTypeCode = addressTypeCode;
            this.line1 = line1;
            this.line2 = line2;
            this.city = city; 
            this.addressStateTC = addressStateTC;
            this.zip = zip; 
            this.addressCountryTC = addressCountryTC;   
            this.addressState = addressState;
            this.postalDropCode = postalDropCode;
            this.yearsAtAddress = yearsAtAddress;
        }

        private string addressTypeCode = string.Empty;
        public string AddressTypeCode { get { return addressTypeCode; } }

        private string line1 = string.Empty;
        public string Line1 { get { return line1;}}

        private string line2 = string.Empty;
        public string Line2 { get { return line2;}}

        private string line3 = string.Empty;
        public string Line3 { get { return line3;}}

        private string city = string.Empty;
        public string City { get { return city; } }

        private string addressStateTC = string.Empty;
        public string AddressStateTC { get { return addressStateTC; }}

        private string addressState = string.Empty;
        public string AddrressState { get { return addressState; }}

        private string zip = string.Empty;
        public string Zip { get { return zip; }}    
    
        private string addressCountryTC = string.Empty;
        public string AddressCountryTC { get { return addressCountryTC; }}

        private string postalDropCode = string.Empty;
        public string PostalDropCode { get { return postalDropCode; }}

        private decimal yearsAtAddress = 0;
        public decimal YearsAtAddress { get { return yearsAtAddress; }} 
    }
}
