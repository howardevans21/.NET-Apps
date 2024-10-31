using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Identity.Client;
using System.Reflection.Metadata.Ecma335;


namespace DWiPipeline
{
    public class ReadConfig
    {
        public ReadConfig()
        {

        }

        public void GetSettings()
        {
            var asmbly = Assembly.GetExecutingAssembly();
            var projectName = Assembly.GetExecutingAssembly().GetName().Name;

            var configBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var configSection = configBuilder.GetSection("Settings");
            stagingPath = configSection["StagingPath"] ?? "";

            configSection = configBuilder.GetSection("DocuWare");

            /*********************************
             * DocuWare Index Fields 
             *********************************/

            dwIndexField_PolicyNumber = configSection["DWIndexField_POLICY_NUMBER"] ?? string.Empty;
            dwIndexField_ClientFirstName = configSection["DWIndexField_CLIENT_FIRSTNAME"] ?? string.Empty;
            dwIndexField_ClientLastName = configSection["DWIndexField_CLIENT_LASTNAME"] ?? string.Empty;
            dwIndexField_NIB = configSection["DWIndexField_NIB"] ?? string.Empty;
            dwIndexField_DOC_TYPE = configSection["DWIndexField_DOC_TYPE"] ?? string.Empty;
            dwIndexField_CALL_INFO = configSection["DWIndexField_CALL_INFO"] ?? string.Empty;
            dwIndexField_Agent = configSection["DWIndexField_AGENT"] ?? string.Empty;
            dwIndexField_DOB = configSection["DWIndexField_DOB"] ?? string.Empty;
            dwIndexField_DEPARTMENT = configSection["DWIndexField_DEPARTMENT"] ?? string.Empty;
            dwIndexField_DocumentStatus = configSection["DWIndexField_DOCUMENTSTATUS"] ?? string.Empty;
            dwIndexField_RECEIVED_DATE = configSection["DWIndexField_RECEIVED_DATE"] ?? string.Empty;
            dwIndexField_DOCUMENT_TYPE = configSection["DWIndexField_DOCUMENT_TYPE"] ?? string.Empty;
            dwIndexField_LDOCUMENTNAME = configSection["DWIndexField_LDDOCUMENTNAME"] ?? string.Empty;
            dwIndexField_DWSTOREDATETIME = configSection["DWIndexField_DWSTOREDATETIME"] ?? string.Empty;

            /*********************************
            * DocuWare Index Fields 
            *********************************/
            configSection = configBuilder.GetSection("DocuWareFixedValues");
            dwFixedValue_CALL_INFO = configSection["CALL_INFO"] ?? string.Empty;
            dwFixedValue_DOCUMENT_STATUS = configSection["DOCUMENT_STATUS"] ?? string.Empty;
            dwFixedValue_DEPARTMENT = configSection["DEPARTMENT"] ?? string.Empty;
            dwFixedValue_SEARCH_DIALOG_ID = configSection["SEARCH_DIALOG_ID"] ?? string.Empty;

            /*********************************
            * DocuWare Login Account
           *********************************/
            configSection = configBuilder.GetSection("DocuWare_Account");
            dw_Login_URI = configSection["DocURI"] ?? string.Empty;
            dw_Login_User = configSection["DWUser"] ?? string.Empty;
            dw_Login_password = configSection["DWPassword"] ?? string.Empty;
            dw_CabinetName = configSection["Cabinet"] ?? string.Empty;
           
            
        }

        /// <summary>
        /// XML PATH 
        /// </summary>
        private string xmlPath = "";
        public string XMLPath

        {
            get { return xmlPath; }
        }


        /// <summary>
        /// Staging Path
        /// </summary>
        private string stagingPath = "";
        public string StagingPath { get { return stagingPath; } }

        /***********************************************************
         * DocuWare Configured Index Fields 
         ************************************************************/
        private string dwIndexField_PolicyNumber = string.Empty;
        public string DWIndexField_PolicyNumber { get { return dwIndexField_PolicyNumber; } }

        private string dwIndexField_ClientFirstName = string.Empty;
        public string DWIndexField_ClientFirstName { get { return dwIndexField_ClientFirstName; } }

        private string dwIndexField_ClientLastName = string.Empty;
        public string DWIndexField_ClientLastName { get { return dwIndexField_ClientLastName; } }

        private string dwIndexField_NIB = string.Empty;
        public string DWIndexField_NIB { get { return dwIndexField_NIB; } }

        private string dwIndexField_DOB = string.Empty;
        public string DWIndexField_DOB { get { return dwIndexField_DOB; } }

        private string dwIndexField_Agent = string.Empty;
        public string DWIndexField_Agent { get { return dwIndexField_Agent; } }

        private string dwIndexField_CALL_INFO = string.Empty;
        public string DWIndexField_CALL_INFO { get { return dwIndexField_CALL_INFO; } }

        private string dwIndexField_DEPARTMENT = string.Empty;
        public string DWIndexField_DEPARTMENT { get { return dwIndexField_DEPARTMENT; } }

        private string dwIndexField_DocumentStatus = string.Empty;
        public string DWIndexField_DocumentStatus { get { return dwIndexField_DocumentStatus; } }

        private string dwIndexField_DOC_TYPE = string.Empty;
        public string DWIndexField_DOC_TYPE { get { return dwIndexField_DOC_TYPE; } }

        private string dwIndexField_RECEIVED_DATE = string.Empty;
        public string DWIndexField_RECEIVED_DATE { get { return dwIndexField_RECEIVED_DATE; } }

        private string dwIndexField_DOCUMENT_TYPE = string.Empty;
        public string  DWIndexField_DOCUMENT_TYPE { get { return dwIndexField_DOCUMENT_TYPE; } }

        private string dwIndexField_LDOCUMENTNAME = string.Empty;
        public string  DWIndexField_LDOCUMENTNAME { get { return dwIndexField_LDOCUMENTNAME; } }

        private string dwIndexField_DWSTOREDATETIME = string.Empty;
        public string DWIndexField_DWSTOREDATETIME { get { return dwIndexField_DWSTOREDATETIME; } }

        /*********************************************************************
         * DocuWare Configured Fixed Values 
         *********************************************************************/
        private string dwFixedValue_DOCUMENT_STATUS = string.Empty;
        public string DWFixedValue_DOCUMENT_STATUS { get { return dwFixedValue_DOCUMENT_STATUS; } }

        private string dwFixedValue_CALL_INFO = string.Empty;
        public string DWFixedValue_CALL_INFO { get { return dwFixedValue_CALL_INFO; } }

        private string dwFixedValue_DEPARTMENT = string.Empty;
        public string DWFixedValue_DEPARTMENT { get { return dwFixedValue_DEPARTMENT; } }

        private string dwFixedValue_SEARCH_DIALOG_ID = string.Empty;
        public string DWFixedValue_SEARCH_DIALOG_ID { get { return dwFixedValue_SEARCH_DIALOG_ID; } }

        /**********************************************
         * DocuWare Account 
        *********************************************/
        private string dw_Login_URI = string.Empty;
        public string DW_Login_URI { get { return dw_Login_URI; } }

        private string dw_Login_User = string.Empty;
        public string DW_Login_USER { get { return dw_Login_User; } }

        private string dw_Login_password = string.Empty;
        public string DW_Login_Password {  get { return dw_Login_password; } }

        private string dw_CabinetName = string.Empty;
        public string DW_CabinetName {  get {  return dw_CabinetName; } }
    }
}
