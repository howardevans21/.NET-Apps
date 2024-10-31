namespace DWPipelineWebService.DataModel.Holding.ChildrenModels
{
    public class ProducerModel
    {
        public ProducerModel() {  }

        private string companyProducerID = string.Empty;
        public string CompanyProducerID
        {
            get { return companyProducerID;}
            set { companyProducerID = value; }
        }

        private string licenseNumber = string.Empty;
        public string LicenseNumber
        {
            get { return licenseNumber; }
            set { licenseNumber = value; }
        }
    }
}
