/***********************************************************
To initialize MS secret keys read the documentation below:
https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-7.0&tabs=windows

After initializing secret keys, run the command in the NUGET Package Console to set the SendGrid API Key. 
Note <key> is a placeholder, so use Colina's SendGrid API Key.

dotnet user-secrets set SendGridKey <key>
***********************************************************/

/***********************************************************
Secret Keys are protects sensitive information so its
not exposed while in development.

For production use environment variables to safeguard 
the SendGrid API key.
***********************************************************/

/***********************************************
Super ADMIN ACCOUNT LOGIN LINK 
https://localhost:44387/Identity/Account/Login
The link above should be hidden and not accesible 
to other users. Only IT administrators should 
have knowledge of this link for security reasons. 

The super admin account is used to manage acccounts 
especially if the website does not have any other users.
***************************************************/