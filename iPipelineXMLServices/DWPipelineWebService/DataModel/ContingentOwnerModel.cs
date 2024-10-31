namespace DWPipelineWebService.DataModel
{
    internal class ContingentOwnerModel
    {
        public ContingentOwnerModel() { }
        public ContingentOwnerModel(string firstName, string lastName, DateTime birthDate, string relationDescription, string relationRoleCode, bool isBirthDateValid, PartyModel partyModel)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.birthDate = birthDate;
            this.relationDescription = relationDescription;
            this.relationRoleCode = relationRoleCode;
            this.isBirthDateValid = isBirthDateValid;
            this.partyModel = partyModel;
        }

        private string firstName = string.Empty;
        public string FirstName { get { return firstName; } }

        private string lastName = string.Empty;
        public string LastName { get { return lastName; } }

        private DateTime birthDate = DateTime.MinValue;
        public DateTime BirthDate { get { return birthDate; } }

        private string relationDescription = string.Empty;
        public string RelationDescription { get { return relationDescription; } }

        private string relationRoleCode = string.Empty;
        public string RelationRoleCode { get { return relationRoleCode; } }

        private bool isBirthDateValid = false;
        public bool IsBirthDateNull { get { return isBirthDateValid; } }

        private PartyModel partyModel = null;
        public PartyModel PartyModel { get { return partyModel; } }

        private bool wasContingentOwnerFound = false;
        public bool WasContingentOwnerFound
        {
            get { return wasContingentOwnerFound; }
            set { wasContingentOwnerFound = value; }
        }
    }
}
