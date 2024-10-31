namespace DWPipelineWebService.DataModel
{
    public enum InsuredType
    {
        Insured,
        Owner,
        JointInsured,
        Beneficiary,
        None
    }

    public class PersonModel
    {
        public PersonModel(string firstName, string lastName, string middleName, int age, DateTime birthDate, string citizenship, string birthCountry, string birthJurisdiction, string suffix, string prefix, string title, string gender, string smokerStat, string smokerStatAccordCode, bool isBirthDateValid)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.middleName = middleName;
            this.birthDate = birthDate;
            this.age = age;
            this.citizenship = citizenship;
            this.birthCountry = birthCountry;
            this.birthJurisdiction = birthJurisdiction;
            this.prefix = prefix;
            this.suffix = suffix;
            this.title = title; 
            this.gender = gender;
            this.smokerStat = smokerStat;
            this.smokerStatAccordCode = smokerStatAccordCode;
            this.isBirthDateValid = isBirthDateValid;
        }

        private string firstName = string.Empty;
        public string FirstName { get { return firstName; } }

        private string lastName = string.Empty;
        public string LastName { get { return lastName; } }

        private string middleName = string.Empty;
        public string MiddleName { get { return middleName; } }

        private DateTime birthDate;
        public DateTime BirthDate { get {  return birthDate; } }

        private int age = -1;
        public int Age { get { return age; } }

        private string citizenship = string.Empty;
        public string Citizenship { get {  return citizenship; } }

        private string birthCountry = string.Empty;
        public string BirthCountry { get { return birthCountry; } } 

        private string birthJurisdiction = string.Empty;
        public string BirthJurisdiction {  get { return birthJurisdiction; } }

        private string prefix = string.Empty;
        public string Prefix { get { return prefix; } }

        private string suffix = string.Empty;
        public string Suffix { get { return suffix; } }

        private string title  = string.Empty;
        public string Title { get { return title; } }

        private string gender = string.Empty;
        public string Gender {  get { return gender; } }

        private string smokerStat = string.Empty;
        public string SmokerStat { get { return smokerStat; } }

        private string smokerStatAccordCode = string.Empty;
        public string SmokerStatAccordCode { get { return smokerStatAccordCode; } }

        private EducationModel education = new EducationModel();
        public EducationModel Education { get {  return education; } set { education = value; } }

        private bool isBirthDateValid = false;
        public bool IsBirthDateValid { get { return isBirthDateValid; } }

    }
}
