namespace DWPipelineWebService.DataModel
{
    public class OrganizationModel
    {
        public OrganizationModel() { }

        private string organizationType = string.Empty;
        public string OrgannizationType { get { return organizationType; } set { organizationType = value; } }

        private string orgForm = string.Empty;
        public string OrgForm { get { return orgForm; } set { orgForm = value; } }

 
    }
}
