namespace DWPipelineWebService.DataModel
{
    internal class AgentModel
    {
        private string producerID = string.Empty;
        private double interestPercent = 0;
        public AgentModel(string producerID, double interestPercent, string firstName, string lastName)
        {
            this.producerID = producerID;
            this.interestPercent = interestPercent;
            this.firstName = firstName;
            this.lastName = lastName;
        }

        public string ProducerID { get { return producerID;} }
        public double InterestPercent { get { return interestPercent; } }

        private string firstName = string.Empty;
        public string FirstName { get { return firstName; } }   

        private string lastName = string.Empty; 
        public string LastName { get { return lastName; } } 
    }
}
