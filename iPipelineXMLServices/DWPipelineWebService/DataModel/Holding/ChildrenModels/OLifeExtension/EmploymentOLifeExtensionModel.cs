namespace DWPipelineWebService.DataModel.Holding.ChildrenModels.OLifeExtension
{
    public class EmploymentOLifeExtensionModel
    {
       
        public EmploymentOLifeExtensionModel() { }

        public EmploymentOLifeExtensionModel(string otherIncomeAndSourceComment, string unemploymentDetailsComment, string unemploymentSpecify, string unemploymentIncomeAndSource)
        {
            this.otherIncomeAndSourceComment=otherIncomeAndSourceComment;
            this.unemploymentDetailsComment = unemploymentDetailsComment;
            this.unemploymentSpecify=unemploymentSpecify;   
            this.unemploymentIncomeAndSource = unemploymentIncomeAndSource;

        }

        private string otherIncomeAndSourceComment = string.Empty;
        public string OtherIncomeAndSourceComment {
            get { return this.otherIncomeAndSourceComment; } 
        }

        private string unemploymentDetailsComment = string.Empty;
        public string UnemploymentDetailsComment
        {
            get { return this.unemploymentDetailsComment; }
        }

        private string unemploymentSpecify = string.Empty;
        public string UnemploymentSpecify
        {
            get { return this.unemploymentSpecify; }
        }

        private string unemploymentIncomeAndSource = string.Empty;
        public string UnemploymentIncomeAndSource
        {
            get { return this.unemploymentIncomeAndSource; }
        }
    }
}
