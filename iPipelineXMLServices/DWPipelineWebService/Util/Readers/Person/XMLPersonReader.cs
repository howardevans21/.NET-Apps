using DWPipelineWebService.DataModel;
using System.Xml.Linq;

namespace DWPipelineWebService.Util
{
    partial class XMLMainReader
    {
        internal partial class XMLPersonReader
        {

            private string id_Person = string.Empty;
            private string id_FirstName = string.Empty;
            private string id_LastName = string.Empty;
            private string id_Marstat = string.Empty;
            private string id_Gender = string.Empty;
            private string id_BirthDate = string.Empty;
            private string id_Age = string.Empty;
            private string id_BirthCountry = string.Empty;
            private string id_BirthJurisdictionTC = string.Empty;
            private string id_Citizenship = string.Empty;
            private string id_MiddleName = string.Empty;
            private string id_Prefix = string.Empty;
            private string id_Suffix = string.Empty;
            private string id_Title = string.Empty;
            private string id_SmokerStat = string.Empty;

            public XMLPersonReader(string id_Person, string id_FirstName, string id_LastName, string id_Marstat, string id_Gender, string id_BirthDate, string id_Age, string id_Citizenship

                , string id_BirthCountry, string id_BirthJurisdictionTC, string id_MiddleName, string id_Prefix, string id_Suffix, string id_Title, string id_SmokerStat)
            {
                this.id_Person = id_Person;
                this.id_FirstName = id_FirstName;
                this.id_LastName = id_LastName;
                this.id_Marstat = id_Marstat;
                this.id_Gender = id_Gender;
                this.id_BirthDate = id_BirthDate;
                this.id_Age = id_Age;
                this.id_BirthCountry = id_BirthCountry;
                this.id_BirthJurisdictionTC = id_BirthJurisdictionTC;
                this.id_Citizenship = id_Citizenship;
                this.id_MiddleName = id_MiddleName;
                this.id_Prefix = id_Prefix;
                this.id_Suffix = id_Suffix;
                this.id_Title = id_Title;
                this.id_SmokerStat = id_SmokerStat;
            }

            public List<PersonModel> GetPersonData(XElement xdn)
            {
                Utils util = new Utils();

                List<PersonModel> personModels = new List<PersonModel>();
                string iPipelineAccordCodeAttribute = "tc"; // iPipleline Accord Code Attribute 

                /********************************************************************************************************
                    PERSON DATA    
                ********************************************************************************************************/
                IEnumerable<XElement> personElements = xdn.Elements().Where(x => x.Name.LocalName.ToLower() == id_Person);
                foreach (XElement personElement in personElements)
                {

                    bool doesPersonFirstName = util.doesXMLElementExists(personElement, id_FirstName);
                    string firstName = doesPersonFirstName ? personElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_FirstName.ToLower()).First().Value : string.Empty;

                    bool doesPersonLastName = util.doesXMLElementExists(personElement, id_LastName);
                    string lastName = doesPersonLastName ? personElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_LastName.ToLower()).First().Value : string.Empty;

                    bool does_MiddleName = util.doesXMLElementExists(personElement, id_MiddleName);
                    string middleName = does_MiddleName ? personElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_MiddleName.ToLower()).First().Value : string.Empty;

                    bool doesMarStat = util.doesXMLElementExists(personElement, id_Marstat);
                    string marStat = doesMarStat ? personElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_Marstat.ToLower()).First().Value : string.Empty;

                    bool doesGender = util.doesXMLElementExists(personElement, id_Gender);
                    string gender = doesGender ? personElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_Gender.ToLower()).First().Value : string.Empty;

                    bool doesBirthDate = util.doesXMLElementExists(personElement, id_BirthDate);
                    string birthDateS = doesBirthDate ? personElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_BirthDate.ToLower()).First().Value : string.Empty;

                    DateTime birthDate = new DateTime();
                    bool isBirthDateValid =  DateTime.TryParse(birthDateS, out birthDate);

                    bool doesAge = util.doesXMLElementExists(personElement, id_Age);
                    string ageS = doesAge ? personElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_Age.ToLower()).First().Value : string.Empty;
                    int age = -1;
                    int.TryParse(ageS, out age);

                    bool doesCitizienship = util.doesXMLElementExists(personElement, id_Citizenship);
                    string citizenship = doesCitizienship ? personElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_Citizenship.ToLower()).First().Value : string.Empty;

                    bool doesCountry = util.doesXMLElementExists(personElement, id_BirthCountry);
                    string birthCountry = doesCountry ? personElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_BirthCountry.ToLower()).First().Value : string.Empty;

                    bool doesBirthJurisdiction = util.doesXMLElementExists(personElement, id_BirthJurisdictionTC);
                    string birthJurisdiction = doesBirthJurisdiction ? personElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_BirthJurisdictionTC.ToLower()).First().Value : string.Empty;

                    bool doesPrefix = util.doesXMLElementExists(personElement, id_Prefix);
                    string prefix = doesPrefix ? personElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_Prefix.ToLower()).First().Value : string.Empty;

                    bool doesSuffix = util.doesXMLElementExists(personElement, id_Suffix);
                    string suffix = doesSuffix ? personElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_Suffix.ToLower()).First().Value : string.Empty;

                    bool doesTitle = util.doesXMLElementExists(personElement, id_Title);
                    string title = doesTitle ? personElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_Title.ToLower()).First().Value : string.Empty;

                    bool doesSmokerStatExsts = util.doesXMLElementExists(personElement, id_SmokerStat);
                    string smokerStat = doesSmokerStatExsts ? personElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_SmokerStat.ToLower()).First().Value : string.Empty;
                    string smokerStatCode = string.Empty;
                    if (doesSmokerStatExsts)
                    {
                        XElement smokerStatElement = personElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_SmokerStat.ToLower()).First();
                        if (smokerStatElement.HasAttributes) smokerStatCode = smokerStatElement.Attributes().Where(x => x.Name.LocalName.ToLower() == "tc").First().Value;
                    }


                    /***************************************************************************
                     *  Education Data 
                     ***************************************************************************/
                    string id_Education_MainElement = "education";
                    string id_ProviderOrSchool = "ProviderOrSchool";
                    IEnumerable<XElement> educationElements = personElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_Education_MainElement.ToLower());
                    EducationModel educationModel = new EducationModel();
                    foreach (XElement educationElement in educationElements)
                    {
                        bool exists_ProviderOrSchool = util.doesXMLElementExists(educationElement, id_Education_MainElement);
                        string providerOrSchool = exists_ProviderOrSchool ? educationElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_ProviderOrSchool).First().Value : string.Empty;
                        educationModel.ProviderOrSchool = providerOrSchool.Length > 0 ? true : false;
                        educationModel.ProviderOrSchoolText = providerOrSchool;
                        break;
                    }

                    PersonModel personModel = new PersonModel(firstName, lastName, middleName, age, birthDate, citizenship, birthCountry, birthJurisdiction, suffix, prefix, title, gender, smokerStat, smokerStatCode, isBirthDateValid);
                    personModel.Education = educationModel;
                    personModels.Add(personModel);
                }

                return personModels;
            }
        }
    }
}