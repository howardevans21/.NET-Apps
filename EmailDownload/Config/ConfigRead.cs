using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.IO;
using Newtonsoft.Json;

namespace EmailDownload.Config
{
    public static class ConfigRead
    {
        public static AppSettings ReadAppConfig()
        {
            string jsonPath = "Config\\appSettings.json";
            string jsonString = File.ReadAllText(jsonPath);
            AppSettings appSettings = JsonConvert.DeserializeObject<AppSettings>(jsonString);
            return appSettings;
        }
    }
}
