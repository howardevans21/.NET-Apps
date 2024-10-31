using DWPipelineWebService.DataModel.Holding.ChildrenModels.OLifeExtension;

namespace DWPipelineWebService.DataModel
{
    public class EmploymentModel
    {
        public EmploymentModel() { }

        public EmploymentModel(string employerName,
            string occupation, int yearsAtEmployment, 
            decimal annualSalary
            ) { 
                this.employerName = employerName;
                this.occupation = occupation;
                this.yearsAtEmployment = yearsAtEmployment;
                this.annualSalary = annualSalary;
        }

        private decimal annualSalary = 0;
        public decimal AnnualSalary
        {
            get { return annualSalary; }
            set { annualSalary = value; }
        }

        private string employerName = string.Empty;
        public string EmployerName
        {
            get { return employerName; }
            set { employerName = value; }
        }

        private string occupation = string.Empty;
        public string Occupation 
        { 
            get { return occupation; }
            set { occupation = value; }
        }

        private double yearsAtEmployment = 0;
        public double YearsAtEmployment
        {
            get { return yearsAtEmployment; }
            set { yearsAtEmployment = value; }
        }

        private EmploymentOLifeExtensionModel employmentOLifeExtension = new EmploymentOLifeExtensionModel();
        public EmploymentOLifeExtensionModel EmploymentOLifeExtension
        {
            get { return employmentOLifeExtension;} 
            set { employmentOLifeExtension = value;}
        }
    }
}
