using DWPipelineWebService.DataModel;
using DWPipelineWebService.DataModel.Holding.ChildrenModels.OLifeExtension;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Xml.Linq;
//using static Microsoft.EntityFrameworkCore.Query.Internal.ExpressionTreeFuncletizer;

namespace DWPipelineWebService.Util
{
    partial class XMLMainReader {
    
        internal partial class XMLLifeReader
        {
            internal partial class XMLPartyReader
            {
                internal partial class XMLEmploymentReader
                {
                    private string id_main = string.Empty;
                    private string id_annualSalary = string.Empty;
                    private string id_employerName = string.Empty;
                    private string id_Occupation = string.Empty;
                    private string id_YearsAtEmployment = string.Empty;
                 
                    public XMLEmploymentReader(string id_main, string id_annualSalary, string id_employerName, string id_Occupation, string id_YearsAtEmployment)
                    {
                        this.id_main = id_main;
                        this.id_annualSalary = id_annualSalary;
                        this.id_employerName = id_employerName;
                        this.id_Occupation = id_Occupation;
                        this.id_YearsAtEmployment = id_YearsAtEmployment; 
                    }

                    public EmploymentModel GetEmploymentData(XElement xdn)
                    {
                        Utils util = new Utils();

                        EmploymentModel employmentModel = new EmploymentModel();

                        /********************************************************************************************************
                         Employment DATA
                        ****************************************************************************************************/
                        IEnumerable<XElement> employmentElemments = xdn.Elements().Where(x => x.Name.LocalName.ToLower() == id_main);
                        foreach(XElement employmentElement in employmentElemments)
                        {
                            // Annual Salary 
                            bool exists_annualSalary = util.doesXMLElementExists(employmentElement, id_annualSalary);
                            string annualSalary = exists_annualSalary ? employmentElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_annualSalary.ToLower()).First().Value : string.Empty;
                            decimal annualSalaryDecimal = 0; 
                            decimal.TryParse(annualSalary, out annualSalaryDecimal);
                            employmentModel.AnnualSalary = annualSalaryDecimal;

                            // Employer Name  
                            bool exists_EmployerName = util.doesXMLElementExists(employmentElement, id_employerName);
                            string employerName = exists_EmployerName ? employmentElement.Elements().Where(x => x.Name.LocalName.ToLower() ==  id_employerName.ToLower()).First().Value : string.Empty;
                            employmentModel.EmployerName = employerName;

                            // Occupation 
                            bool exists_Occupation = util.doesXMLElementExists(employmentElement, id_Occupation);
                            string occupation = exists_Occupation ? employmentElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_Occupation.ToLower()).First().Value : string.Empty;
                            employmentModel.Occupation = occupation;

                            // Years at Employment 
                            bool exists_YearsAtEmployment = util.doesXMLElementExists(employmentElement, id_YearsAtEmployment);
                            string yearsAtEmployment = exists_YearsAtEmployment ? employmentElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_YearsAtEmployment.ToLower()).First().Value : string.Empty;
                            double yearsAtEmploymentDouble = 0;
                            double.TryParse(yearsAtEmployment, out yearsAtEmploymentDouble);
                            employmentModel.YearsAtEmployment = yearsAtEmploymentDouble;

                            // OLifeExtension 
                            string id_OLifeExtension = "OLifEExtension";
                            IEnumerable<XElement> lifeExtensionElements = employmentElement.Elements().Where(x => x.Name.LocalName.ToLower() == id_OLifeExtension.ToLower());
                            EmploymentOLifeExtensionModel employmentOLifeExtensionModel = new EmploymentOLifeExtensionModel();
                            foreach(XElement element in lifeExtensionElements)
                            {
                                string id_OtherIncomeAndSource = "OtherIncomeAndSource";
                                bool exists_OtherIncomeAndSource = util.doesXMLElementExists(element, id_OtherIncomeAndSource);
                                string otherIncomeAndSource = exists_OtherIncomeAndSource ? element.Elements().Where(x => x.Name.LocalName.ToLower() == id_OtherIncomeAndSource.ToLower()).First().Value : string.Empty;
                             
                                string id_UnemploymentDetails = "UnemploymentDetails";
                                bool exists_UnemploymentDetails = util.doesXMLElementExists(element, id_UnemploymentDetails);
                                string unemploymentDetails = exists_UnemploymentDetails ? element.Elements().Where(x => x.Name.LocalName.ToLower() == id_UnemploymentDetails.ToLower()).First().Value : string.Empty;

                                string id_UnemploymentSpecify = "UnemploymentSpecify";
                                bool exists_UnemploymentSpecify = util.doesXMLElementExists(element, id_UnemploymentSpecify);
                                string unemploymentSpecify = exists_UnemploymentSpecify ? element.Elements().Where(x => x.Name.LocalName.ToLower() == id_UnemploymentSpecify.ToLower()).First().Value : string.Empty;

                                string id_UnemploymentIncomeAndSource = "UnemploymentIncomeAndSource";
                                bool exists_UnemploymentIncomeAndSource = util.doesXMLElementExists(element, id_UnemploymentIncomeAndSource);
                                string unemploymentIncomeAndSource = exists_UnemploymentIncomeAndSource ? element.Elements().Where(x => x.Name.LocalName.ToLower() == id_UnemploymentIncomeAndSource.ToLower()).First().Value : string.Empty;

                                // Employment OLifeExtension
                                employmentOLifeExtensionModel = new EmploymentOLifeExtensionModel(otherIncomeAndSource, unemploymentDetails, unemploymentSpecify, unemploymentIncomeAndSource);

                                break;
                            }

                            employmentModel.EmploymentOLifeExtension = employmentOLifeExtensionModel;

                            break;
                        }

                        return employmentModel;
                    }
                }

            }
        }

    }

}
