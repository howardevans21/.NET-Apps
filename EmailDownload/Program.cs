using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using System;
using System.Threading.Tasks;
using Microsoft.Graph.Models;

namespace EmailDownload.Config
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                AppSettings appSettings = EmailDownload.Config.ConfigRead.ReadAppConfig();
                ReadEmails.ReadEmails.Read(appSettings);
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
