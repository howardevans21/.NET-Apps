using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace iPipelinePStepWebService
{
    internal class ReadConfig
    {
        public ReadConfig() { }

        public void GetSettings()
        {
            var asmbly = Assembly.GetExecutingAssembly();
            var projectName = Assembly.GetExecutingAssembly().GetName().Name;

            var configBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var configSection = configBuilder.GetSection("Settings");

            /********************************************
              * SECURITY SECTION 
            **************************************/
            string id_processClassID = "ProcessClassID";
            processClassID = configSection[id_processClassID.ToLower()] ?? string.Empty;

            configSection = configBuilder.GetSection("Security");
            string id_User = "User";
            string id_Password = "Password";
            string id_Domain = "Domain";
            service_Account_UserID = configSection[id_User.ToLower()] ?? string.Empty;
            service_Account_password = configSection[id_Password.ToLower()] ?? string.Empty;
            service_Account_Domain = configSection[id_Domain.ToLower()] ?? string.Empty;

            /********************************************
             * PATHFINDER URL SECTION 
            **************************************/
            string id_PROTOCOL = "PROTOCOL";
            string id_HTTP_RESOURCE = "RESOURCE";
            string id_domain = "DOMAIN";
            string id_HTTP_PortNumber = "PORT";

            configSection = configBuilder.GetSection("URL");
            http_PROTOCOL = configSection[id_PROTOCOL.ToLower()] ?? string.Empty;
            http_RESOURCE = configSection[id_HTTP_RESOURCE.ToLower()] ?? string.Empty;
            http_DOMAIN = configSection[id_domain.ToLower()] ?? string.Empty;
            http_PORT_NUMBER = configSection[id_HTTP_PortNumber.ToLower()] ?? string.Empty;

            /********************************************
             * API KEYS SECTION 
            **************************************/
            configSection = configBuilder.GetSection("APIKEYS");
            string id_Default = "DEFAULT";
            apiKey = configSection[id_Default.ToLower()] ?? string.Empty;

            /********************************************
            * ERROR RETURN CODES SECTION 
           **************************************/
            configSection = configBuilder.GetSection("ERROR_CODES");
            string id_codes = "Codes";
            string error_codes = configSection[id_codes.ToLower()] ?? string.Empty;
            string[] codes = error_codes.Split(',', ';');
            foreach(string code in codes) { errorCodes.Add(code); }
        }

        private string processClassID = string.Empty;
        public string ProcessClassID { get { return processClassID; }}

        private string http_RESOURCE = string.Empty;
        public string HTTP_RESOURCE { get { return http_RESOURCE; }}

        private string http_WEB_URI = string.Empty;
        public string HTTP_WEB_URI { get { return http_WEB_URI; }}

        private string http_PORT_NUMBER = string.Empty;
        public string HTTP_PORT_NUMBER { get { return http_PORT_NUMBER; } }

        private string http_DOMAIN = string.Empty;
        public string HTTP_DOMAIN { get { return http_DOMAIN; } }

        private string http_PROTOCOL = string.Empty;
        public string HTTP_PROTOCOL { get { return http_PROTOCOL; } }

        private string service_Account_UserID = string.Empty;
        public string Service_Account_UserID { get { return service_Account_UserID; }}

        private string service_Account_password = string.Empty;
        public string Service_Account_Password { get { return service_Account_password; }}
    
        private string service_Account_Domain = string.Empty;
        public string Service_Account_Domain { get { return service_Account_Domain; }}

        private string apiKey = string.Empty;
        public string APIKey { get { return apiKey; } }

        private List<string> errorCodes = new List<string>();
        public List<string> ErrorCodes
        {
            get { return errorCodes; }
        }
    }
}


