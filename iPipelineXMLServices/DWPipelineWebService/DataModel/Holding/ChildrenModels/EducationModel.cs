namespace DWPipelineWebService.DataModel
{
    public class EducationModel
    {
        public EducationModel() { }
        public EducationModel(bool providerOrSchool)
        {

            this.providerOrSchool = providerOrSchool;
        }

        private bool providerOrSchool = false;

        public bool ProviderOrSchool
        {
            get { return providerOrSchool; }
            set { providerOrSchool = value; }
        }

        private string providerOrSchoolText = string.Empty;
        public string ProviderOrSchoolText
        {
            get { return providerOrSchoolText; }
            set { providerOrSchoolText = value; }
        }
    }
}


