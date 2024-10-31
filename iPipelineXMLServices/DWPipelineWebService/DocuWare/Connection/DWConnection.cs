using System;
using System.Collections.Generic;
using System.Text;
using DocuWare.Platform.ServerClient;
using System.Net;
using System.Net.Cache;
using System.Net.Http;
using System.Runtime;
using DWiPipeline;
using DWPipelineWebService.DataModel;

namespace DWPipelineWebService
{
    public static partial class DWConnection
    {
        public static class Account
        {
            public static ServiceConnection DWLogin(DWSettings dWSettings)
            {
                Uri uri = new Uri(dWSettings.DocURI);
                return ServiceConnection.Create(uri, dWSettings.DWUser, dWSettings.DWPassword);
            }

            public static void UploadFile(FileCabinet fileCabinet, FileInfo fileInfo, ReadConfig readConfig, ClientModel clientModel, string policyNumber, string receivedDate, string document_Type) 
            {
                string clientFirstName = clientModel.FirstName;
                string clientLastName = clientModel.LastName;
                if (clientModel.DoesOwnerExists)
                {
                    clientFirstName = clientModel.OwnerModel.FirstName;
                    clientLastName = clientModel.OwnerModel.LastName;
                }

                var indexData = new Document()
                {
                    Fields = new List<DocumentIndexField>()
                {
                    DocumentIndexField.Create(readConfig.DWIndexField_PolicyNumber, policyNumber),
                    DocumentIndexField.Create(readConfig.DWIndexField_ClientFirstName, clientFirstName),
                    DocumentIndexField.Create(readConfig.DWIndexField_ClientLastName, clientLastName),
                    DocumentIndexField.Create(readConfig.DWIndexField_NIB, clientModel.NIBNumber),
                    DocumentIndexField.Create(readConfig.DWIndexField_Agent, clientModel.Agent),
                    DocumentIndexField.Create(readConfig.DWIndexField_DOB, clientModel.DOB),
                    DocumentIndexField.Create(readConfig.DWIndexField_CALL_INFO, readConfig.DWFixedValue_CALL_INFO),
                    DocumentIndexField.Create(readConfig.DWIndexField_DEPARTMENT, readConfig.DWFixedValue_DEPARTMENT),
                    DocumentIndexField.Create(readConfig.DWIndexField_DocumentStatus, readConfig.DWFixedValue_DOCUMENT_STATUS),
                    DocumentIndexField.Create(readConfig.DWIndexField_DOC_TYPE, document_Type),
                    DocumentIndexField.Create(readConfig.DWIndexField_RECEIVED_DATE, receivedDate),
                    DocumentIndexField.Create(readConfig.DWIndexField_DOCUMENT_TYPE, "iGO")
                  }
                };


                var document = fileCabinet.EasyUploadSingleDocument(
               fileInfo,
              DocumentIndexField.Create(readConfig.DWIndexField_PolicyNumber, policyNumber),
                    DocumentIndexField.Create(readConfig.DWIndexField_ClientFirstName, clientFirstName),
                    DocumentIndexField.Create(readConfig.DWIndexField_ClientLastName, clientLastName),
                    DocumentIndexField.Create(readConfig.DWIndexField_NIB, clientModel.NIBNumber),
                    DocumentIndexField.Create(readConfig.DWIndexField_Agent, clientModel.Agent),
                    DocumentIndexField.Create(readConfig.DWIndexField_DOB, clientModel.DOB),
                    DocumentIndexField.Create(readConfig.DWIndexField_CALL_INFO, readConfig.DWFixedValue_CALL_INFO),
                    DocumentIndexField.Create(readConfig.DWIndexField_DEPARTMENT, readConfig.DWFixedValue_DEPARTMENT),
                    DocumentIndexField.Create(readConfig.DWIndexField_DocumentStatus, readConfig.DWFixedValue_DOCUMENT_STATUS),
                    DocumentIndexField.Create(readConfig.DWIndexField_DOC_TYPE, document_Type),
                    DocumentIndexField.Create(readConfig.DWIndexField_RECEIVED_DATE, receivedDate),
                    DocumentIndexField.Create(readConfig.DWIndexField_DOCUMENT_TYPE, "iGO"));

                var section = document.EasyUploadFile(fileInfo);

                //var uploadedDocument = fileCabinet.UploadDocument(indexData, fileInfo);

                // document.EasyUploadFile(new System.IO.FileInfo("HRS.pdf"));
                // indexData.EasyUploadFile(fileInfo);

            }

            public static bool DoesFileAlreadyExistsInCabinet (FileCabinet fileCabinet, ReadConfig readConfig, ClientModel clientModel, string policyNumber, string recievedDate, string fileName)
            {
                bool fileAlreadyExists = false;
                var dialogInfoItems = fileCabinet.GetDialogInfosFromSearchesRelation();

                // Check if the specific Search Dialog Exists 
                bool does_Specific_Search_DialogExists = false;
                foreach(DialogInfo diaInfos in dialogInfoItems.Dialog)
                {
                    if (diaInfos.Id == readConfig.DWFixedValue_SEARCH_DIALOG_ID)
                    {
                        does_Specific_Search_DialogExists = true;
                        break;
                    }
                }

                // If the specific Search Dialog Exists 
                // Find the results if it exists 
                if (does_Specific_Search_DialogExists)
                {
                    var dialogInfo = dialogInfoItems.Dialog.Where(x => x.Id == readConfig.DWFixedValue_SEARCH_DIALOG_ID).First();
                    var dialog = dialogInfo.GetDialogFromSelfRelation();
                    DocumentsQueryResult queryResult = runQuery(dialog, fileCabinet, readConfig, clientModel, policyNumber, recievedDate, fileName);
                    if (queryResult != null && queryResult.Items.Count > 0) fileAlreadyExists = true;
                }
                else
                {
                    // If Specific Search Dialog cannot be found 
                    // Get the default dialog
                    var dialog = dialogInfoItems.Dialog[0].GetDialogFromSelfRelation();
                    DocumentsQueryResult queryResult = runQuery(dialog, fileCabinet, readConfig, clientModel, policyNumber, recievedDate, fileName);
                    if (queryResult != null && queryResult.Items.Count > 0)
                    {
                        fileAlreadyExists = true;
                    }
                }

                return fileAlreadyExists;
            }

            private static DocumentsQueryResult runQuery(Dialog dialog, FileCabinet fileCabinet, ReadConfig readConfig, ClientModel clientModel, string policyNumber, string recievedDate, string fileName)
            { 
                // Conditions must contain a value 
                // No empty string are acceptable from DocuWare
                // If empty, use null instead

                var q = new DialogExpression()
                {
                    Operation = DialogExpressionOperation.And,
                    Condition = new List<DialogExpressionCondition>()
                {
                    DialogExpressionCondition.Create(readConfig.DWIndexField_PolicyNumber, policyNumber),
                    DialogExpressionCondition.Create(readConfig.DWIndexField_NIB, clientModel.NIBNumber.Length > 0 ? clientModel.NIBNumber : null),
                    DialogExpressionCondition.Create(readConfig.DWIndexField_LDOCUMENTNAME,fileName),
                    DialogExpressionCondition.Create(readConfig.DWIndexField_RECEIVED_DATE, recievedDate)
                },
                    Count = 2,
                    SortOrder = new List<SortedField>
                {
                    SortedField.Create(readConfig.DWIndexField_DWSTOREDATETIME, SortDirection.Desc)
                        }
                };

                var queryResult = dialog.GetDocumentsResult(q);

                return queryResult;
            }
        }
    }
}
