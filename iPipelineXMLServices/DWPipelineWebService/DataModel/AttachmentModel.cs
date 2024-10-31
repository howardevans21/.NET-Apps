using DocuWare.Platform.ServerClient;
using DWPipelineWebService.DataModel;
using DWPipelineWebService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DWiPipeline.DataModel
{


    internal class AttachmentModel
    {
        public AttachmentModel(string formName, AttachmentType.AttachmentImageType attachmentImageType, string attachmentBasicType, string attachmentData, string iPipeline_Document_Type, string formInstanceName)
        {
            this.formName = formName;
            this.attachmentImageType = attachmentImageType;
            this.attachmentBasicType = attachmentBasicType;
            this.attachmentData = attachmentData;
            this.iPipeline_Document_Type = iPipeline_Document_Type;
            this.formInstanceName = formInstanceName;
        }

        private WhichAttachmentName whichAttachmentName = WhichAttachmentName.None;
        public WhichAttachmentName WhichAttachmentName
        {
            get
            {
                if (iPipeline_Document_Type.Length > 0) { whichAttachmentName = WhichAttachmentName.Description; }
                else whichAttachmentName = WhichAttachmentName.FormName;

                return whichAttachmentName;
            }
        }

        /// <summary>
        /// Attachment Basic Type 
        /// </summary>
        private AttachmentType.AttachmentImageType attachmentImageType = AttachmentType.AttachmentImageType.None;
        public AttachmentType.AttachmentImageType AttachmentImageFileType
        {
            get { return attachmentImageType; }
        }

        private string convertedFormInstanceName = string.Empty;
        public string ConvertedFormInstanceName
        {
            get { return convertedFormInstanceName; }
            set {  convertedFormInstanceName = value;}
        }

        private string formInstanceName = string.Empty;
        public string FormInstanceName { get { return formInstanceName; } }

        private string formName = string.Empty;
        public string FormName
        {
            get { return formName; }
        }

        private string attachmentBasicType = string.Empty;
        public string AttachmentBasicType
        {
            get { return attachmentBasicType; }
        }

        private string attachmentData = string.Empty;
        public string AttachmentData { get { return attachmentData; } }

        private string iPipeline_Document_Type = string.Empty;
        public string Document_Type
        {
            get { return iPipeline_Document_Type; }
        }
        public void CreateDataFiles(Holding.Policy policy, ClientModel client, FileCabinet fileCabinet)
        {
            Utils util = new Utils();
            util.ConvertHexToFile(attachmentData, policy, this, client, fileCabinet);

        }
        public static class AttachmentType
        {
            public enum AttachmentImageType
            {
                None,
                Image,
                PDF,
                TXT,
                JPG, 
                TIF, 
                PNG,
                JPEG,
                WORD
            }

            public static AttachmentImageType convertString(string str)
            {
                AttachmentImageType attachmentImageType = AttachmentImageType.None;

                switch (str.ToLower())
                {
                    case "image":
                        attachmentImageType = AttachmentImageType.Image;
                        break;

                    case "pdf":
                        attachmentImageType = AttachmentImageType.PDF;
                        break;

                    case "jpg":
                        attachmentImageType = AttachmentImageType.JPG;
                        break;

                    case "png":
                        attachmentImageType = AttachmentImageType.PNG;
                        break;

                    case "tif":
                        attachmentImageType = AttachmentImageType.TIF;
                        break;

                    case "jpeg":
                        attachmentImageType = AttachmentImageType.JPEG;
                        break;

                    case "word":
                        attachmentImageType = AttachmentImageType.WORD;
                        break;

                }

                return attachmentImageType;
            }

          
        }
    }
}
