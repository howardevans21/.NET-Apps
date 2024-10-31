using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DocuWare.Platform.ServerClient;
using DWiPipeline;
using DWiPipeline.DataModel;
using DWPipelineWebService.DataModel;
using static System.Net.Mime.MediaTypeNames;

namespace DWPipelineWebService.Util
{
    internal class Utils
    { 

        public Utils() { }

        public decimal TruncateDecimal(decimal value, int precision)
        {
            decimal step = (decimal)Math.Pow(10, precision);
            decimal tmp = Math.Truncate(step * value);
            return tmp / step;
        }

      
        string GetSpaces(string fieldName)
        {
            string spaces = "  ";
            return spaces;
        }

        public void ConvertHexToFile(string hex, Holding.Policy policy, AttachmentModel attachmentModel, ClientModel client, FileCabinet fileCabinet)
        {
            ReadConfig readConfig = new ReadConfig();
            readConfig.GetSettings();
            byte[] data = Convert.FromBase64String(hex);

           
        

            string dateFormat = "yyyy-MM-dd";

            string policyNumber = policy.PolicyNumber.Length > 0 ? policy.PolicyNumber : "NoPolicy";
            string policyDirPath = string.Format("{0}\\{1}", readConfig.StagingPath, policyNumber);

            // Determine if the owner exists 
            string clientFirstName = client.FirstName;
            string clientLastName = client.LastName;
            if (client.DoesOwnerExists)
            {
                clientFirstName = client.OwnerModel.FirstName;
                clientLastName = client.OwnerModel.LastName;
            }

            if (!Directory.Exists(policyDirPath))
                Directory.CreateDirectory(policyDirPath);

            /************************************************************************
             * Always Step 1 Create companion file for DocuWare Index Fields
             * Always Step 2 Create PDF Document
             * 
             * Both PDF and Companion file should have the exact same name except for file extension
             ***************************************************************************/

            string textExtension = AttachmentModel.AttachmentType.AttachmentImageType.TXT.ToString().ToLower();
            string companionTextFile = Path.Combine(policyDirPath, attachmentModel.FormName);
            string formattedCompanionTextFile = string.Format("{0}.{1}", companionTextFile, textExtension);

            string document_Type = attachmentModel.ConvertedFormInstanceName;

            if (!File.Exists(formattedCompanionTextFile))
            {
                using (var stream = File.OpenWrite(formattedCompanionTextFile))
                {
                    stream.Flush();
                    stream.Close();
                };
            }

            /**************************************************
             * Create Index fields in Companion File 
             ***********************************************/
            string spaces = GetSpaces(readConfig.DWIndexField_PolicyNumber);
            string companionFileLine = string.Format("{0}{1}{2}", readConfig.DWIndexField_PolicyNumber, spaces, policyNumber);
            string line_Policy = companionFileLine;

            spaces = GetSpaces(readConfig.DWIndexField_ClientFirstName);
            companionFileLine = string.Format("{0}{1}{2}", readConfig.DWIndexField_ClientFirstName, spaces, clientFirstName);
            string line_ClientFirstName = companionFileLine;

            spaces = GetSpaces(readConfig.DWIndexField_ClientLastName);
            companionFileLine = string.Format("{0}{1}{2}", readConfig.DWIndexField_ClientLastName, spaces, clientLastName);
            string line_ClientLastName = companionFileLine;

            spaces = GetSpaces(readConfig.DWIndexField_NIB);
            companionFileLine = string.Format("{0}{1}{2}", readConfig.DWIndexField_NIB, spaces, client.NIBNumber);
            string line_NIB = companionFileLine;

            spaces = GetSpaces(readConfig.DWIndexField_Agent);
            companionFileLine = string.Format("{0}{1}{2}", readConfig.DWIndexField_Agent, spaces, client.Agent);
            string line_Agent = companionFileLine;

            spaces = GetSpaces(readConfig.DWIndexField_DOB);
            companionFileLine = string.Format("{0}{1}{2}", readConfig.DWIndexField_DOB, spaces, client.DOB.ToString(dateFormat));
            string line_DOB = companionFileLine;

            spaces = GetSpaces(readConfig.DWIndexField_CALL_INFO);
            companionFileLine = string.Format("{0}{1}{2}", readConfig.DWIndexField_CALL_INFO, spaces, readConfig.DWFixedValue_CALL_INFO);
            string line_CALL_INFO = companionFileLine;

            spaces = GetSpaces(readConfig.DWIndexField_DEPARTMENT);
            companionFileLine = string.Format("{0}{1}{2}", readConfig.DWIndexField_DEPARTMENT, spaces, readConfig.DWFixedValue_DEPARTMENT);
            string line_DEPARTMENT = companionFileLine;

            spaces = GetSpaces(readConfig.DWIndexField_DocumentStatus);
            companionFileLine = string.Format("{0}{1}{2}", readConfig.DWIndexField_DocumentStatus, spaces, readConfig.DWFixedValue_DOCUMENT_STATUS);
            string line_DocumentStatus = companionFileLine;

            spaces = GetSpaces(readConfig.DWIndexField_DOC_TYPE);
            companionFileLine = string.Format("{0}{1}{2}", readConfig.DWIndexField_DOC_TYPE, spaces, document_Type);
            string line_DocType = companionFileLine;

            spaces = GetSpaces(readConfig.DWIndexField_RECEIVED_DATE);
            string receivedDate = DateTime.Now.ToString(dateFormat);
            companionFileLine = string.Format("{0}{1}{2}", readConfig.DWIndexField_RECEIVED_DATE, spaces, receivedDate);
            string line_ReceivedDate = companionFileLine;

            // Append lines to the companion File
            string[] lines = { line_Policy, line_ClientFirstName, line_ClientLastName, line_NIB, line_Agent, line_DOB, line_CALL_INFO,
            line_DEPARTMENT, line_DocumentStatus, line_DocType, line_ReceivedDate};

            string companionFilePath = companionTextFile + ".txt";
            if (new FileInfo(companionFilePath).Length == 0)
            {
                using (var stream = File.OpenWrite(formattedCompanionTextFile))
                {
                    stream.Flush();
                    stream.Close();

                    using (StreamWriter w = File.AppendText(formattedCompanionTextFile))
                    {
                        foreach (string line in lines)
                        {
                            w.WriteLine(line);
                        }
                    }
                }
            }

            /******************************************************
             * Create PDF Document 
             * Backup Documents to a specific location in case of failures
             ***********************************************************/
            string filePDF = Path.Combine(policyDirPath, attachmentModel.FormName);
            string pdfExtension = AttachmentModel.AttachmentType.AttachmentImageType.PDF.ToString().ToLower();

            bool doesPathExtensionAlreadyExists = Path.GetExtension(filePDF) != null && Path.GetExtension(filePDF).Length > 0 ? true : false;
            string policyFilePath = !filePDF.ToLower().EndsWith(".pdf") == true || !doesPathExtensionAlreadyExists ? string.Format("{0}.{1}", filePDF, pdfExtension) : filePDF;

            if (!File.Exists(policyFilePath))
                File.WriteAllBytes(@policyFilePath, data);

            /*****************************************
             * Upload to DocuWare 
             ************************************************/
            FileInfo fileInfo = new FileInfo(policyFilePath);

            string fileNameToUpload = Path.GetFileNameWithoutExtension(fileInfo.Name);
            
            bool doesFileAlreadyExistsInDW = DWConnection.Account.DoesFileAlreadyExistsInCabinet(fileCabinet, readConfig, client, policyNumber, receivedDate, fileNameToUpload);

            // Upload File to DocuWare if it does not exists
            // Check to prevent duplication of records in the cabinet 
            if (doesFileAlreadyExistsInDW)
                DWConnection.Account.UploadFile(fileCabinet, fileInfo, readConfig, client, "Test-" + policyNumber , receivedDate, document_Type);
       
            /**********************************************
             * Upload Files to Sharepoint
             *******************************************/
       
            Sharepoint.SharepointUpload sharepointUpload = new Sharepoint.SharepointUpload();
            sharepointUpload.uploadFileToSharePointAsync();


        }

        public bool doesXMLElementExists(XElement xdn, string elementName)
        {
            bool isExists = false;
            foreach (XElement xElement in xdn.Elements())
            {
                if (xElement.Name.LocalName.ToLower() == elementName.ToLower())
                {
                    isExists = true; break;
                }
            }

            return isExists;
        }

        public string getValueOfFormElement(XElement xdn, string elementName)
        {
            string val = string.Empty;
            foreach (XElement xElement in xdn.Elements())
            {
                if (xElement.Name.LocalName.ToLower() == elementName.ToLower())
                {
                     val = xElement.Value;
                      break;
                }
            }

            return val;
        }

        public string getXMLElementValue(XElement xdn, string elementName)
        {
            string val = string.Empty;
            foreach (XElement xElement in xdn.Elements())
            {
                if (xElement.Name.LocalName.ToLower() == elementName.ToLower())
                {
                    val = xElement.Value;
                    break;
                }
            }

            return val;
        }

        public bool doesAttributeExists(XElement xdn, string attributeName)
        {
            bool isExists = false;

            foreach(XAttribute xAttribute in xdn.Attributes())
            {
                if(xAttribute.Name.LocalName.ToLower() == attributeName.ToLower())
                    isExists = true;
            }

            return isExists;
        }


        public string GenerateNumber()
        {
            Random random = new Random();
            string r = "";
            int i;
            for (i = 1; i < 11; i++)
            {
                r += random.Next(0, 9).ToString();
            }
            return r;
        }


    }
}
