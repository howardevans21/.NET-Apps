namespace DWPipelineWebService.DataModel
{
    
    public class ClientModel
    {

        public ClientModel(string firstName, string lastName,  string nibNumber, DateTime dob, string agent, string partyID, ClientType clientType, OwnerModel ownerModel, bool doesOwnerExists) {

            this.firstName = firstName;
            this.lastName = lastName;
            this.nibNumber = nibNumber;
            this.dob = dob;
            this.agent = agent;
            this.clientType = clientType;
            this.partyID = partyID;
            this.ownerModel = ownerModel;
            this.doesOwnerExists = doesOwnerExists;
        }

        private string firstName = string.Empty;
        public string FirstName { get { return firstName; } }

        private string lastName = string.Empty;
        public string LastName { get { return lastName; } } 

        private string nibNumber = string.Empty;
        public string NIBNumber { get { return nibNumber; } }


        private DateTime dob = DateTime.Now;
        public DateTime DOB { get { return dob; } }

        public DateTime ReceivedDate = DateTime.Now;

        private string agent = string.Empty;
        public string Agent { get { return agent; } }

        private string partyID = string.Empty;
        public string PartyID {  get { return partyID; } }

        private ClientType clientType = ClientType.None;
        public ClientType ClientType { get { return clientType; } }

        private OwnerModel ownerModel;
        public OwnerModel OwnerModel { get { return ownerModel; } }

        private bool doesOwnerExists = false;
        public bool DoesOwnerExists
        {
            get { return doesOwnerExists; }
        }
    }
}
