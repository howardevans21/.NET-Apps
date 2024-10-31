using DWPipelineWebService.DataModel.Holding.ChildrenModels;
using System.Net;

namespace DWPipelineWebService.DataModel
{
    public class OwnerModel
    {
        public OwnerModel(string firstName, string lastName, string country, bool isBirthDateValid, DateTime birthDate, AddressModel addressModel, PartyModel partyModel, List<RelationModel> relationModels)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.country = country;
            this.isBirthDateValid = isBirthDateValid;
            this.birthDate = birthDate;
            this.addressModel = addressModel;
            this.partyModel = partyModel;
            this.relationModels = relationModels;
        }

        public OwnerModel(string partyFullName, string country, bool isBirthDateValid, AddressModel addressModel, PartyModel partyModel, List<RelationModel> relationModels)
        {
            this.partyFullName = partyFullName;
            this.country = country;
            this.isBirthDateValid = isBirthDateValid;
            this.addressModel = addressModel;
            this.partyModel = partyModel;
            this.relationModels = relationModels;

        }

        private string firstName = string.Empty;
        public string FirstName { get { return firstName; } }

        private string lastName = string.Empty;
        public string LastName { get { return lastName; } }

        private string country = string.Empty;
        public string Country { get { return country; } }

        private string partyFullName = string.Empty;
        public string PartyFulllName { get { return partyFullName; } }

        private bool isBirthDateValid = false;
        public bool IsBirthDateValid { get { return isBirthDateValid; } }

        private DateTime birthDate = DateTime.Now.Date;
        public DateTime BirthDate { get { return birthDate; } }

        private AddressModel addressModel = null;
        public AddressModel AddressData { get { return addressModel; }}

        private PartyModel partyModel = null;
        public PartyModel PartyData { get { return partyModel; }}

        private List<RelationModel> relationModels = new List<RelationModel>();

        public List<RelationModel> AllRelations { get {  return relationModels; }}
    }
}
