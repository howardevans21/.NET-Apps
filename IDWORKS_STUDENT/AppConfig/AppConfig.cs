using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDWORKS_STUDENT.AppConfig
{
    public static class IDWorksAppConfig
    {
        public static List<string> _coverageProviderColumns = new List<string>();
        public static List<string> _coverageColumns = new List<string>();
        public static List<string> _firstNameColumns = new List<string>();
        public static List<string> _lastNameColumns = new List<string>();
        public static List<string> _policyColumns = new List<string>();
        public static List<string> _schoolColumns = new List<string>();
        public static List<string> _effectiveDateColumns = new List<string>();
        public static int _numberOfColumns = 7; // Default
        public static string Environment = "";
        public static string SendGridAPIKey = "";
        public static string SendGrid_Sender = "";
        public static string DefaultSuperAdmin = "";
        public static string DefaultSuperAdminPassword = "";  
    }

    public static class UserSignAuth
    {
        public enum SignedOnPage
        {
            StudentUploadFile = 0,
            StudentPageList = 1,
            Roles = 2,
            UsersRoles = 3
        }

        public static bool userAlreadySignedIn = false;
        public static bool RoleSuperAdmin = false;
        public static bool RoleAdmin = false;
        public static bool RoleModerator = false;
        public static bool RoleBasic = false;
        public static SignedOnPage WhichSignedOnPage = SignedOnPage.StudentUploadFile; 
    }
}
