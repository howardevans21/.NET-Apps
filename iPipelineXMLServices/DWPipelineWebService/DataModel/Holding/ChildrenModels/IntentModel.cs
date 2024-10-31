namespace DWPipelineWebService.DataModel
{
    internal class IntentModel
    {
        public IntentModel() { }
       

        private string expenseNeedTypeCode = string.Empty;
        public string ExpenseNeedTypeCode { get { return expenseNeedTypeCode; } set { expenseNeedTypeCode = value; } }

        private string expenseNeedKey = string.Empty;
        public string ExpenseNeedKey { get {  return expenseNeedKey; } set { expenseNeedKey = value; } }
    }
}
