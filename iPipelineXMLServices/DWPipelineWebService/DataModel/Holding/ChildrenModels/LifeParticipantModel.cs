namespace DWPipelineWebService.DataModel
{
    internal class LifeParticipantModel
    {
        public LifeParticipantModel() { }

        public LifeParticipantModel(string partyID, string smokerStat, string lifeParticipantRoleCode,
            int issueAge, string issueGender, double lifeParticipant)
        {

            this.partyID = partyID;
            this.smokerStat = smokerStat;
            this.lifeParticipantRoleCode = lifeParticipantRoleCode;
            this.issueAge = issueAge;
            this.issueGender = issueGender;
            this.lifeParticipantPct = lifeParticipant;
        }

        private string partyID = string.Empty;
        public string PartyID { get { return partyID; } }

        private string smokerStat = string.Empty;
        public string SmokerStat { get { return smokerStat; } }

        private string lifeParticipantRoleCode = string.Empty;
        public string LifeParticipantRoleCode { get { return lifeParticipantRoleCode; } }

        private int issueAge = -1;
        public int IssueAge { get { return issueAge; } }

        public string issueGender = string.Empty;
        public string IssueGender { get { return issueGender; } }

        public double lifeParticipantPct = 0;
        public double LifeParticipantPct { get { return lifeParticipantPct; } }
    }
}

