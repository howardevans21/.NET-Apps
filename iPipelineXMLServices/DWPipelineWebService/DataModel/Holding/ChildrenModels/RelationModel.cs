namespace DWPipelineWebService.DataModel.Holding.ChildrenModels
{
    public class RelationModel
    {
        public RelationModel(string relationPrimaryKey, string relatedObjectID, string originatingObjectID, string originatingObjectType, string relatedObjectType,
            string relationRoleCode, string relationDescription)
        {
            this.relationPrimaryKey = relationPrimaryKey; 
            this.relatedObjectID = relatedObjectID;
            this.originatingObjectID = originatingObjectID;
            this.originatingObjectType = originatingObjectType;
            this.relatedObjectType = relatedObjectType;
            this.relationRoleCode = relationRoleCode;
            this.relationDescription = relationDescription;
             
        }

        private string relatedObjectID = string.Empty;
        public string RelatedObjectID
        {
            get { return relatedObjectID; }
        }

        private string relationPrimaryKey = string.Empty;
        public string RelationPrimaryKey
        {
            get { return relationPrimaryKey; }
        }

        private string originatingObjectID = string.Empty;
        public string OriginatingObjectID
        {
            get { return originatingObjectID; }
        }

        private string originatingObjectType = string.Empty;
        public string OriginatingObjectType
        {
            get { return originatingObjectType; }
        }

        private string relatedObjectType = string.Empty;
        public string RelatedObjectType
        {
            get { return relatedObjectType; }   
        }

        private string relationRoleCode = string.Empty;
        public string RelationRoleCode
        {
            get { return relationRoleCode; }
        }

        private string relationDescription = string.Empty;
        public string RelationDescription
        {
            get { return relationDescription; } 
        }

        private double interestPercent = 0;
        public double InterestPercent
        {
            get { return interestPercent; }
            set { interestPercent = value; }
        }

        private string irrevokableInd = string.Empty;
        public string IrrevokableInd
        {
            get { return irrevokableInd; }
            set { irrevokableInd = value; }
        }

        private string distributionOption = string.Empty;
        public string DistributionOption
        {
            get { return distributionOption; }
            set { distributionOption = value; }
        }
    }
}
