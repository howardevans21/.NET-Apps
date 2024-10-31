using DWiPipeline.DataModel;
using System.Xml;
using System.Xml.Linq;

namespace DWPipelineWebService.Util
{
    partial class XMLMainReader
    {
        internal class XMLAttachmentReader
        {
            private string parent_Node_Name_AttachmentNode = string.Empty;
            private string element_Name_AttachmentBasicType = string.Empty;
            private string element_Name_AttachmentData = string.Empty;
            private string element_Name_AttachmentImageType = string.Empty;
            private string element_iPipelineDocumentType = string.Empty;

            public XMLAttachmentReader(string parent_Name_AttachmentNode, string element_Name_AttachmentBasicType, string element_Name_AttachmentData, string element_Name_AttachmentImageType, string element_iPipelineDocumentType)
            {

                parent_Node_Name_AttachmentNode = parent_Name_AttachmentNode;
                this.element_Name_AttachmentBasicType = element_Name_AttachmentBasicType;
                this.element_Name_AttachmentData = element_Name_AttachmentData;
                this.element_Name_AttachmentImageType = element_Name_AttachmentImageType;
                this.element_iPipelineDocumentType = element_iPipelineDocumentType;
            }


            public List<AttachmentModel> GetDocumentData(IEnumerable<XElement> formElements)
            {
                Utils util = new Utils();

                // List of Attachments 
                List<AttachmentModel> attachments = new List<AttachmentModel>();

                foreach (XElement formElement in formElements)
                {
                    foreach (XAttribute xAttribute in formElement.Attributes())
                    {
                        string formName = xAttribute.Value;
                        string formInstanceName = util.getXMLElementValue(formElement, "FormName");
                       
                        foreach (XElement xElement in formElement.Elements())
                        {
                            if (xElement.Name.LocalName.ToLower() == parent_Node_Name_AttachmentNode)
                            {
                                bool doesBasicTypeExists = util.doesXMLElementExists(xElement, element_Name_AttachmentBasicType);
                                bool doesAttachmentDataExists = util.doesXMLElementExists(xElement, element_Name_AttachmentData);
                                bool doesFileTypeExists = util.doesXMLElementExists(xElement, element_Name_AttachmentImageType);
                                bool doesDocumentTypeExists = util.doesXMLElementExists(xElement, element_iPipelineDocumentType);
                                
                                if (doesBasicTypeExists && doesAttachmentDataExists && doesFileTypeExists)
                                {
                                    // Get attachment info 
                                    string v_FileBasicType = string.Empty;
                                    string v_FileData = string.Empty;
                                    string v_FileType = string.Empty;
                                    string v_iPipeleine_Document_Type = string.Empty;

                                    XElement attachmentBasicType = xElement.Elements().Where(x => x.Name.LocalName.ToLower() == element_Name_AttachmentBasicType).First();
                                    v_FileBasicType = attachmentBasicType.Value;

                                    XElement attachmentData = xElement.Elements().Where(x => x.Name.LocalName.ToLower() == element_Name_AttachmentData).First();
                                    v_FileData = attachmentData.Value;

                                    XElement attachmentType = xElement.Elements().Where(x => x.Name.LocalName.ToLower() == element_Name_AttachmentImageType).First();
                                    v_FileType = attachmentType.Value;

                                    if (doesDocumentTypeExists)
                                    {
                                        XElement iPipeline_DocumentType = xElement.Elements().Where(x => x.Name.LocalName.ToLower() == element_iPipelineDocumentType).First();
                                        v_iPipeleine_Document_Type = iPipeline_DocumentType.Value;
                                    }

                                    AttachmentModel.AttachmentType.AttachmentImageType fileType = AttachmentModel.AttachmentType.convertString(v_FileType);

                                    // Remove any file extensions 
                                    string documentTypeWithoutExt = Path.GetFileNameWithoutExtension(v_iPipeleine_Document_Type);
                                    string formNameWithoutExt = Path.GetFileNameWithoutExtension(formInstanceName);

                                    AttachmentModel attachmentModel = new AttachmentModel(formName, fileType, v_FileBasicType, v_FileData, documentTypeWithoutExt, formNameWithoutExt);
                                    attachments.Add(attachmentModel);
                                }
                            }
                        }
                    }
                }

                return attachments;
            }
        }
    }
}
