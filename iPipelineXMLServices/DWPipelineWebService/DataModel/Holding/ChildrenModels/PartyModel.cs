using DWPipelineWebService.DataModel.Holding.ChildrenModels;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace DWPipelineWebService.DataModel;

public class PartyModel
{
    public PartyModel(string partyID, string partyTypeCode, string partyFullName, string partyGovtID, string partyGovtIDC, string partyResidenceState, string partyResidenceCountry) {

        this.partyID = partyID;
        this.partyTypeCode = partyTypeCode;
        this.partyFullName = partyFullName;
        this.partyGovtID = partyGovtID;
        this.partyGovtIDC = partyGovtIDC;
        this.partyResidenceState = partyResidenceState;
        this.partyResidenceCountry = partyResidenceCountry;
        

    }

    private string partyID = string.Empty;
    public string PartyID
    {
        get { return this.partyID; }
    }

    public bool IsATrustee
    {
        get {
            bool isATrustee = false;
            string[] splitWords = this.partyID.Split('_');
            foreach (string word in splitWords)
            {
                if (word.Contains("Trustee") || word.Contains("Trust")) { isATrustee = true; break; }
            }
            return isATrustee;
        }
    }

    private string partyTypeCode = string.Empty;
    public string PartyTypeCode { get { return partyTypeCode; } }

    private string partyFullName = string.Empty;
    public string PartyFullName { get { return partyFullName; } }

    private string partyGovtID = string.Empty;
    public string PartyGovtID { get { return partyGovtID; } }

    private string partyGovtIDC = string.Empty;
    public string PartyGovtIDC { get { return partyGovtIDC; } }

    private string partyResidenceState = string.Empty;
    public string PartyResidenceState { get { return partyResidenceState; } }

    private string partyResidenceCountry = string.Empty;
    public string PartyResidenceCountry { get { return partyResidenceCountry; } }

    private List<PersonModel> persons = new List<PersonModel>();
    public List<PersonModel> Persons
    {
        get { return persons; }
        set { persons = value; }
    }

    private OrganizationModel organization = new OrganizationModel();
    public OrganizationModel Organization { get { return organization; } set { organization = value; } }

    private AddressModel address = new AddressModel();
    public AddressModel Address { get { return address; } set { address = value; } }

    private List<AddressModel> addresses =  new List<AddressModel>();
    public List<AddressModel> Addresses { get { return addresses; } set { addresses = value; } }

    private List<PhoneModel> phoneModels = new List<PhoneModel>();
    public List<PhoneModel> PhoneModels { get { return phoneModels; } set { phoneModels = value; } }

    private EmailModel emailModel = new EmailModel();
    public EmailModel EmailModel { get { return emailModel; } set { emailModel = value; } }

    private RiskModel riskModel = new RiskModel();
    public RiskModel RiskModel {
        get { return riskModel; }
        set { riskModel = value; }
    }

    private PriorNameModel priorNameModel = new PriorNameModel();
    public PriorNameModel PriorNameModel {
        get { return priorNameModel; }
        set { priorNameModel = value; }
    }

    private EmploymentModel employmentModel = new EmploymentModel();
    public EmploymentModel EmploymentModel
    {
        get { return employmentModel; }
        set { employmentModel = value; }
    }

    private ProducerModel producerModel = new ProducerModel();
    public ProducerModel ProducerModel {
        get { return producerModel; }
        set { producerModel = value; }
    }

    private string socialSecurityNumber = string.Empty;
    public string SocialSecurityNumber
    {
        get {  return  socialSecurityNumber; } 
        set {  socialSecurityNumber = value; }    
    }

    private string taxIDNumber = string.Empty;
    public string TaxIDNumber
    {
        get { return taxIDNumber; }
        set { taxIDNumber = value; }
    }

    private string nibNumber = string.Empty;
    public string NIBNumber
    {
        get { return nibNumber; }
        set { nibNumber = value; }
    }

    private string nationality = string.Empty;
    public string Nationality
    {
        get { return nationality; } 
        set { nationality = value; }    
    }

    private GovernmentInfoModel governmentInfoModel = new GovernmentInfoModel();
    public GovernmentInfoModel  GovernmentInfo
    {
        get { return governmentInfoModel;}
        set { governmentInfoModel = value; }
    }

    private List<GovernmentInfoModel> governmentInfoModels = new List<GovernmentInfoModel>();
    public List<GovernmentInfoModel> GovernmentInfoModels
    {
        get { return governmentInfoModels; }
        set { governmentInfoModels = value; }
    }

    private bool isAPerson = false;
    public bool ISAPerson { get { return isAPerson; } set { isAPerson = value; } }

    
}
