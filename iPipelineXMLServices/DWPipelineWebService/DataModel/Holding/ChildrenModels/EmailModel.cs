namespace DWPipelineWebService.DataModel.Holding.ChildrenModels
{
    public class EmailModel
    {
        public EmailModel() { }

        public EmailModel(string email) { 
            this.email = email;
         }

        private string email = string.Empty;
        public string Email { get { return email; } }
    }
}
