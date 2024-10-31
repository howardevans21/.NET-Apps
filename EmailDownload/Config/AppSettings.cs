using System;
using System.Collections.Generic;
using System.Text;

namespace EmailDownload.Config
{
    public class AppSettings
    {
        public List<EmailSetting> emailSettings { get; set; }
        public string MicrosoftGraphAppID { get; set; }
        public string MicrosoftGraphClientSecretKey { get; set; }
        public string MicrosoftGraphTenantID { get; set; }
        public string MicrosoftGraphURL { get; set; }
        public string FromAddress { get; set; }
        public string AcknowledgeMessage { get; set; }
        public int EmailHardToplimit { get; set; }
        public string NoFileAttachedMessage { get; set; }
        public string AcknowledgeMessageNoFilesAttached { get; set; }        
        public string SubjectPrefix { get; set; }
        public string IgnoreSenders { get; set; }
    }

    public class EmailSetting
    {
        public string Email { get; set; }
        public string DownloadPath { get; set; }
        public int EmailPastReadDays { get; set; }
        public bool  OnlyUnreadEmails { get; set; }
        public string SendToAddress { get; set; }
        public string CCAddress { get; set; }

        // Flag to send email confirmations for file attached or not attached
        public bool SendEmailConfirmation { get; set; }
        public string CustomIgnoreSenders { get; set; }
        public int DeletionPolicyInDays { get; set; } 
        public int DeleteHardLimit { get; set; }
        public string DeleteMailFolders { get; set; }
        public string ReadMailFolders { get; set; }
        public bool   AddSenderToRecipient { get; set; }
    }
}
