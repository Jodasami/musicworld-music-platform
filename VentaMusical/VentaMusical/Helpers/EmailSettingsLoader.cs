using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;


namespace VentaMusical.Helpers
{
    public class EmailSettingsLoader
    {
        public static EmailSettings CargarDesdeConfig()
        {
            var config = ConfigurationManager.GetSection("EmailSettings") as NameValueCollection;

            return new EmailSettings
            {
                SmtpServer = config["SmtpServer"],
                Port = int.Parse(config["Port"]),
                Username = config["Username"],
                Password = config["Password"],
                FromEmail = config["FromEmail"],
                FromName = config["FromName"],
                EnableSsl = bool.Parse(config["EnableSsl"]),
                TimeoutSeconds = int.Parse(config["TimeoutSeconds"])
            };
        }
    }
}