namespace DWPipelineWebService.DataModel
{
    public class PriorNameModel
    {
        public PriorNameModel() { }

        public PriorNameModel(string lastName)
        {
            this.lastName = lastName;
        }

        private string lastName = string.Empty;
        public string LastName
        {
            get { return lastName; }
        }
    }
}
