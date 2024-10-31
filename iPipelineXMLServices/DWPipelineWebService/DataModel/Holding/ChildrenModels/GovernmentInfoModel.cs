namespace DWPipelineWebService.DataModel
{
    public class GovernmentInfoModel
    {
        public GovernmentInfoModel() { }

        public GovernmentInfoModel(string govtID, DateTime govtIDExpDate, string nation)
        {
            this.govtID = govtID;
            this.govtIDExpDate = govtIDExpDate;
            this.nation = nation;
        }

        private string govtID = string.Empty;
        public string GovtID {  get { return govtID; } }

        private DateTime govtIDExpDate;
        public DateTime GovtIDExpDate { get {  return govtIDExpDate; } }

        private string nation = string.Empty;
        public string Nation { get {  return nation; } }
    }
}
