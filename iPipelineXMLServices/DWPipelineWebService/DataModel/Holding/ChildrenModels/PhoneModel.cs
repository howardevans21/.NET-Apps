using System.Collections.Specialized;

namespace DWPipelineWebService.DataModel.Holding.ChildrenModels
{
    public class PhoneModel
    {
        public PhoneModel() { }
        public PhoneModel(string phoneTypeCode, string areaCode, string dialNumber, PhoneNumberType phoneNumberType) { 
        
            this.phoneTypeCode = phoneTypeCode;
            this.areaCode = areaCode;
            this.dialNumber = dialNumber;
            this.phoneNumberType = phoneNumberType;
        
        }

        private string phoneTypeCode = string.Empty;
        public string PhoneTypeCode { get {  return phoneTypeCode; } }

        private string areaCode = string.Empty;
        public string AreaCode { get { return areaCode; } }

        private string dialNumber = string.Empty;
        public string DialNumber
        {
            get
            {
                return dialNumber;
            }
        }

        private string phoneTypeDescription = string.Empty;
        public string PhoneTypeDescription
        {
            get { return phoneTypeDescription; }
            set { phoneTypeDescription = value; }
        }

        private PhoneNumberType phoneNumberType = PhoneNumberType.None;
        public PhoneNumberType PhoneNumberType
        {
            get { return phoneNumberType; }
        }
    }

    
}
