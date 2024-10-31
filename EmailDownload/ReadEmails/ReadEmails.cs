using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Graph.Models;
using Microsoft.Graph;
using Azure.Identity;
using System.IO;
using Microsoft.Kiota;
using System.Threading.Tasks;

namespace EmailDownload.ReadEmails
{
    public static class ReadEmails
    {
        private static void deleteEmailQuery(GraphServiceClient graphClient, Config.EmailSetting emailSetting, MailFolder mailFolder, string dateFilter)
        {
            string mailBox = emailSetting.Email;
            var result = graphClient.Users[mailBox].MailFolders[mailFolder.Id].Messages.GetAsync((requestConfiguration) =>
            {
                requestConfiguration.QueryParameters.Orderby = new string[] { "sentDateTime desc" };
                requestConfiguration.QueryParameters.Top = emailSetting.DeleteHardLimit;
                requestConfiguration.QueryParameters.Filter = dateFilter;

            }).Result.Value;

            var messages = result;

            foreach (var message in messages)
                graphClient.Users[emailSetting.Email].Messages[message.Id].DeleteAsync();  
        }
        
        private static void deleteEmails(Config.AppSettings appSettings, GraphServiceClient graphClient)
        {
            foreach (Config.EmailSetting emailSetting in appSettings.emailSettings)
            {
                string[] mailFolders = emailSetting.DeleteMailFolders.Split(',', ';');

                foreach (string mailFolder in mailFolders)
                {
                    string mailBox = emailSetting.Email;
                  
                    string lessDate = DateTime.Now.AddDays(-emailSetting.DeletionPolicyInDays).ToUniversalTime().ToString("o");
                    string dateFilter = "sentDateTime lt {0}";
                    dateFilter = String.Format(dateFilter, lessDate);

                    var validMailFolders = graphClient.Users[mailBox].MailFolders.GetAsync().Result.Value;
                    foreach (MailFolder validMailFolder in validMailFolders)
                    {
                        if (validMailFolder.DisplayName.ToLower() != mailFolder.ToLower()) continue;// If the folder does not exists in the config ignore

                        deleteEmailQuery(graphClient, emailSetting, validMailFolder, dateFilter);
                    }
                }
            }
        }
        public static void Read(Config.AppSettings appSettings)
        {
            string appID = appSettings.MicrosoftGraphAppID;
            string clientSecret = appSettings.MicrosoftGraphClientSecretKey;
            string tenantID = appSettings.MicrosoftGraphTenantID;
            var scopes = new[] { appSettings.MicrosoftGraphURL };

            var options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
            };

            var clientSecretCredential = new ClientSecretCredential(tenantID, appID, clientSecret, options);
            var graphClient = new GraphServiceClient(clientSecretCredential, scopes);

            foreach (Config.EmailSetting emailSetting in appSettings.emailSettings)
            {
                string[] mailFolders = emailSetting.ReadMailFolders.Split(',', ';');

                foreach (string mailFolder in mailFolders)
                {
                    string mailBox = emailSetting.Email;

                    var validMailFolders = graphClient.Users[mailBox].MailFolders.GetAsync().Result.Value;
                    foreach (MailFolder validMailFolder in validMailFolders)
                    {
                        if (validMailFolder.DisplayName.ToLower() != mailFolder.ToLower()) continue;

                        string lessDate = DateTime.Now.AddDays(-emailSetting.EmailPastReadDays).ToUniversalTime().ToString("o");
                        string dateFilter = "sentDateTime ge {0}";
                        dateFilter = String.Format(dateFilter, lessDate);
                        var result = graphClient.Users[mailBox].MailFolders[validMailFolder.Id].Messages.GetAsync((requestConfiguration) =>
                        {
                            requestConfiguration.QueryParameters.Orderby = new string[] { "sentDateTime desc" };
                            requestConfiguration.QueryParameters.Top = appSettings.EmailHardToplimit;
                            requestConfiguration.QueryParameters.Expand = new string[] { "Attachments" };

                        }).Result.Value;

                        var requestInformation = graphClient.Users[mailBox].MailFolders[validMailFolder.Id].ToGetRequestInformation();
                        requestInformation.QueryParameters.Add("Expand", "Attachments");
                        var messages = result;

                        foreach (var message in messages)
                        {
                            // Ignore senders - these senders are blocked from receiving an email notification and having their files
                            // downloaded from an inbox
                            bool continueProcess = ignoreSender(message.Sender.EmailAddress.Address, appSettings, emailSetting);
                            if (!continueProcess) continue; // Skip email if sender is in the block list
                            bool hasFiles = false; 
                            if ((bool)message.HasAttachments)
                            {
                                if (message.Attachments != null)
                                {
                                    // Include only file attachments
                                    foreach (Attachment attachment in message.Attachments)
                                    {
                                        if (attachment is FileAttachment)
                                        {
                                            string[] fileExtension = attachment.Name.Split('.');
                                            bool isReadEmail = (message.IsRead != null && (bool)message.IsRead);
                                            bool doEmail = emailSetting.OnlyUnreadEmails ? !isReadEmail : true;

                                            if ((!attachment.Name.StartsWith("image") && !attachment.Name.StartsWith("logo") && !attachment.Name.StartsWith("ColinaLogo")) && doEmail)
                                            {
                                                var fileAttachment = (FileAttachment)attachment;
                                                string filePath = emailSetting.DownloadPath + "\\" + fileAttachment.Name;
                                                System.IO.File.WriteAllBytes(filePath, fileAttachment.ContentBytes); // Copy attached file to folder
                                                hasFiles = true;
                                            }
                                        } 
                                    }

                                    if (emailSetting.SendEmailConfirmation && hasFiles)
                                        SendConfirmationEmail.SendEmailAsync(graphClient, appSettings, emailSetting, message);
                                }
                            }
                            else
                            {
                                // The message contained no file attachments 
                                // Send an email confirmation if applicable
                                bool isReadEmail = (message.IsRead != null && (bool)message.IsRead);
                                bool doEmail = emailSetting.OnlyUnreadEmails ? !isReadEmail : true;
                                if (emailSetting.SendEmailConfirmation && doEmail) SendConfirmationEmail.SendEmailAsync(graphClient, appSettings, emailSetting, message, appSettings.NoFileAttachedMessage);
                            }
                        }
                    }
                }
            }

            // Delete emails based on a retention policy 
             deleteEmails(appSettings, graphClient);
        }

        #region Helper Methods 
        private static bool ignoreSender(string emailAddress, Config.AppSettings appSettings, Config.EmailSetting emailSetting)
        {
            bool continueProcess = true;
            string[] senderEmails = appSettings.IgnoreSenders.Split(',', ';');
            foreach (string senderEmail in senderEmails)
                if (emailAddress.ToLower() == senderEmail.ToLower() && emailAddress.Length > 0)
                {
                    continueProcess = false;
                    break;
                }

            if (continueProcess)
            {
               senderEmails = emailSetting.CustomIgnoreSenders.Split(',', ';');
                foreach (string senderEmail in senderEmails)
                {
                    if (emailAddress.ToLower() == senderEmail.ToLower() && emailAddress.Length > 0)
                    {
                        continueProcess = false;
                        break;
                    }
                }
            }
            return continueProcess;
        }

        #endregion
    }
}


