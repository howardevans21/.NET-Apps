using System.Net;
using System.Net.Mail;
using System.Collections.Generic;
using Microsoft.Graph.Models;
using Microsoft.Graph;
using System.Threading.Tasks;

namespace EmailDownload.ReadEmails
{
    public static class SendConfirmationEmail
    {
		public static void SendEmailAsync(GraphServiceClient graphServiceClient, Config.AppSettings appSettings, Config.EmailSetting emailSetting, Message message, string customMessage)
		{
			// Create Send To List 
			List<Recipient> listRecipients = new List<Recipient>();
			var listTo = emailSetting.SendToAddress.Split(',', ';');
			foreach (string s in listTo)
			{
				bool validEmail = ValidateEmail(s);
				if (s.Length > 0 && validEmail)
				{
					EmailAddress email = new EmailAddress();
					email.Address = s;
					email.Name = s;
					Recipient recipient = new Recipient();
					recipient.EmailAddress = email;

					bool continueProcess = ignoreSender(email.Address, appSettings, emailSetting);
					bool wasRecipientAlreadyAdded = listRecipients.Find(x => x.EmailAddress.Address.ToLower() == recipient.EmailAddress.Address.ToLower()) != null ? true : false;
					if (continueProcess && !wasRecipientAlreadyAdded)
						listRecipients.Add(recipient);
				}
			}

			// Add Sender to be a recipient to show acknowledgement receipt 
			Recipient fromRecipient = new Recipient();
			fromRecipient.EmailAddress = message.From.EmailAddress;
			 if(emailSetting.AddSenderToRecipient)listRecipients.Add(fromRecipient);

			// Create CC List 
			List<Recipient> ccRecipients = new List<Recipient>();
			var listCC = emailSetting.CCAddress.Split(',', ';');
			foreach (string s in listCC)
			{
				bool validEmail = ValidateEmail(s);
				if (s.Length > 0 && validEmail)
				{
					EmailAddress email = new EmailAddress();
					email.Address = s;
					email.Name = s;
					Recipient recipient = new Recipient();
					recipient.EmailAddress = email;

					bool continueProcess = ignoreSender(email.Address, appSettings, emailSetting);
					bool wasRecipientAlreadyAdded = ccRecipients.Find(x => x.EmailAddress.Address.ToLower() == recipient.EmailAddress.Address.ToLower()) != null ? true : false;
					if (continueProcess && !wasRecipientAlreadyAdded)
						ccRecipients.Add(recipient);
				}
			}

			string paragraphStyle = "<style>.p2 {font - family: Arial, Helvetica, sans - serif;}</style>";
			string html = "<!DOCTYPE html><html><head>{0}<title>Colina Acknowledgment Email</title></head><body><p class=\"p2\">" + appSettings.AcknowledgeMessageNoFilesAttached + "<br><br>" +  customMessage + "</p>" + "{1}</body></html>";

			// Colina Logo Image
			var logoImagePath = "ColinaLogo.jpeg";
			var logoImageID = "colinaLogo";
			byte[] imageArray = System.IO.File.ReadAllBytes(logoImagePath);
			string colinaLogoHtml = "<br><img src = 'cid:" + logoImageID + "'/>";
			html = string.Format(html, paragraphStyle, colinaLogoHtml); // Add Colina Logo HTML 

			var fileAttachmentLogo = new FileAttachment
			{
				OdataType = "#microsoft.graph.fileAttachment",
				Name = "logo",
				ContentBytes = imageArray,
				ContentId = logoImageID,
				ContentType = "image/jpeg"
			};

			var requestInformation = graphServiceClient.Users[appSettings.FromAddress].ToGetRequestInformation();

			//var pathRequestConfiguation = Microsoft.Graph.Users.Item.UserItemRequestBuilder.UserItemRequestBuilderPatchRequestConfiguration;
			MailFolder mailFolder = new MailFolder();
			mailFolder.Messages = new List<Message>();
			mailFolder.Messages.Add(message);

			Microsoft.Kiota.Abstractions.ResponseHandlerOption option = new Microsoft.Kiota.Abstractions.ResponseHandlerOption();
		
			graphServiceClient.Users[emailSetting.Email].MailFolders["inbox"].ToPatchRequestInformation(mailFolder, (requestConfiguration) =>
				requestConfiguration.Options.Add(option)
			);  

			requestInformation.QueryParameters.Add("Expand", "Message");
			var requestBody = new Microsoft.Graph.Users.Item.SendMail.SendMailPostRequestBody
			{
				Message = new Message
				{
					Subject = appSettings.SubjectPrefix + message.Subject,
					Body = new ItemBody
					{
						ContentType = BodyType.Html,
						Content = html
					},
					ToRecipients = listRecipients,
					CcRecipients = ccRecipients,
				},
				SaveToSentItems = false,
			};
			requestBody.Message.Importance = Importance.High;
			requestBody.Message.Attachments = new List<Microsoft.Graph.Models.Attachment>();
			requestBody.Message.Attachments.Add(fileAttachmentLogo);
			markMessageReadAsync(graphServiceClient, appSettings, emailSetting, message);
			graphServiceClient.Users[appSettings.FromAddress].SendMail.PostAsync(requestBody);
		}

		public static void SendEmailAsync(GraphServiceClient graphServiceClient, Config.AppSettings appSettings, Config.EmailSetting emailSetting, Message message)
		{
			// Create Send To List 
			List<Recipient> listRecipients = new List<Recipient>();
			var listTo = emailSetting.SendToAddress.Split(',', ';');
			foreach (string s in listTo)
			{
				bool validEmail = ValidateEmail(s);
				if (s.Length > 0 && validEmail)
				{
					EmailAddress email = new EmailAddress();
					email.Address = s;
					email.Name = s;
					Recipient recipient = new Recipient();
					recipient.EmailAddress = email;

					bool continueProcess = ignoreSender(email.Address, appSettings, emailSetting);
					bool wasRecipientAlreadyAdded = listRecipients.Find(x => x.EmailAddress.Address.ToLower() == recipient.EmailAddress.Address.ToLower()) != null ? true : false;
					if (continueProcess && !wasRecipientAlreadyAdded)
						listRecipients.Add(recipient);
				}
			}

			// Add Sender to be a recipient to show acknowledgement receipt 
			Recipient fromRecipient = new Recipient();
			fromRecipient.EmailAddress = message.From.EmailAddress;
			if(emailSetting.AddSenderToRecipient) listRecipients.Add(fromRecipient);

			// Create CC List 
			List<Recipient> ccRecipients = new List<Recipient>();
			var listCC = emailSetting.CCAddress.Split(',', ';');
			foreach (string s in listCC)
			{
				bool validEmail = ValidateEmail(s);
				if (s.Length > 0 && validEmail)
				{
					EmailAddress email = new EmailAddress();
					email.Address = s;
					email.Name = s;
					Recipient recipient = new Recipient();
					recipient.EmailAddress = email;

					bool continueProcess = ignoreSender(email.Address, appSettings, emailSetting);
					bool wasRecipeintAlreadyAdded = ccRecipients.Find(x => x.EmailAddress.Address.ToLower() == recipient.EmailAddress.Address.ToLower()) != null ? true : false;
					if (continueProcess && !wasRecipeintAlreadyAdded)
						ccRecipients.Add(recipient);
				}
			}

			// Get a list of file attachments 
			List<FileAttachment> listOfFileAttachments = getFileAttachments(message);
			string tableHTML = createHTMLFileList(listOfFileAttachments);
			string paragraphStyle = "<style>.p2 { font family: Arial, Helvetica, sans - serif; }";
			string html = "<!DOCTYPE html><html><head>{0}{2}<title>Colina Acknowledgment Email</title></head><body><p class=\"p2\">" + appSettings.AcknowledgeMessage + "</p>" + "\r\n" + tableHTML + "\r\n{1}</body></html>";

			string bootstrapStyle = "table, th, td {border: 1px solid black; },th, td { padding: 5px; text - align: left;}</style>";
			
			// Colina Logo Image
			var logoImagePath = "ColinaLogo.jpeg";
			var logoImageID = "colinaLogo";
			byte[] imageArray = System.IO.File.ReadAllBytes(logoImagePath);
			string colinaLogoHtml = "<br><img src = 'cid:" + logoImageID + "'/>";
			html = string.Format(html, paragraphStyle, colinaLogoHtml, bootstrapStyle); // Add Colina Logo HTML 

			var fileAttachmentLogo = new FileAttachment
			{
				OdataType = "#microsoft.graph.fileAttachment",
				Name = "logo",
				ContentBytes = imageArray,
				ContentId = logoImageID,
				ContentType = "image/jpeg"
			};

			var requestInformation = graphServiceClient.Users[appSettings.FromAddress].ToGetRequestInformation();
			requestInformation.QueryParameters.Add("Expand", "Message");
			var requestBody = new Microsoft.Graph.Users.Item.SendMail.SendMailPostRequestBody
			{
				Message = new Message
				{
					Subject = appSettings.SubjectPrefix + message.Subject,
					Body = new ItemBody
					{
						ContentType = BodyType.Html,
						Content = html
					},
					ToRecipients = listRecipients,
					CcRecipients = ccRecipients,
				},
				SaveToSentItems = false,
			};

			requestBody.Message.Importance = Importance.High;
			requestBody.Message.Attachments = new List<Microsoft.Graph.Models.Attachment>();
			requestBody.Message.Attachments.Add(fileAttachmentLogo);
			markMessageReadAsync(graphServiceClient, appSettings, emailSetting, message);
			graphServiceClient.Users[appSettings.FromAddress].SendMail.PostAsync(requestBody);
		}

		private static List<FileAttachment> getFileAttachments(Message message)
		{
			List<FileAttachment> fileAttachments = new List<FileAttachment>();
			foreach (Microsoft.Graph.Models.Attachment attachment in message.Attachments)
			{
				if (attachment is FileAttachment)
				{
					if (!attachment.Name.StartsWith("image"))
					{
						fileAttachments.Add((FileAttachment)attachment);
					}
				}
			}

			return fileAttachments;
		}

		private static string createHTMLFileList(List<FileAttachment> fileAttachments)
		{
			string html = "<table style=\"width: 100 %\"><tr><th>File Attachments</th></tr>{0}</table>";
			string tableRows = "";
			foreach (FileAttachment f in fileAttachments)
				tableRows += string.Format("<tr><td>{0}</td></tr>", f.Name);

			string closingHTML = string.Format(html, tableRows);
			return closingHTML;
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

		private static void markMessageReadAsync(GraphServiceClient graphServiceClient, Config.AppSettings appSettings, Config.EmailSetting email, Message message)
		{
			var requestBody = new Message
			{
				IsRead = true,
			};

			var result = graphServiceClient.Users[email.Email].Messages[message.Id].PatchAsync(requestBody);
			message.IsRead = true;
		}


		private static bool ValidateEmail(string email)
		{
			if (string.IsNullOrWhiteSpace(email))
			{
				return false;
			}

			string[] parts = email.Split('@');
			if (parts.Length != 2)
			{
				return false; // email must have exactly one @ symbol
			}

			string localPart = parts[0];
			string domainPart = parts[1];
			if (string.IsNullOrWhiteSpace(localPart) || string.IsNullOrWhiteSpace(domainPart))
			{
				return false; // local and domain parts cannot be empty
			}

			// check local part for valid characters
			foreach (char c in localPart)
			{
				if (!char.IsLetterOrDigit(c) && c != '.' && c != '_' && c != '-')
				{
					return false; // local part contains invalid character
				}
			}

			// check domain part for valid format
			if (domainPart.Length < 2 || !domainPart.Contains(".") || domainPart.Split(".").Length != 2 || domainPart.EndsWith(".") || domainPart.StartsWith("."))
			{
				return false; // domain part is not a valid format
			}

			return true;
		}

		#endregion
	}
}

