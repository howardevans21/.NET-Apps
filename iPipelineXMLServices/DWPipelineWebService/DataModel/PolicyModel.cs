using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DWiPipeline.DataModel
{
    internal class PolicyModel
    {

        public PolicyModel() { }

        public PolicyModel(string policyID)
        {
            this.policyID = policyID;
        }

        /// <summary>
        /// Policy ID 
        /// </summary>
        private string policyID = string.Empty;
        public string PolicyId  { get { return policyID; }}

        private string lob = string.Empty;
       
        /// <summary>
        /// List of Attachments for Policy
        /// </summary>
        public List<AttachmentModel> attachments = new List<AttachmentModel>();
    }
}
