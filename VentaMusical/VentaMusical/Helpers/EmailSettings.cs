using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;

namespace VentaMusical.Helpers
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int Port { get; set; } = 587; 
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FromEmail { get; set; } = string.Empty;
        public string FromName { get; set; } = string.Empty;
        public bool EnableSsl { get; set; } = true;
        public int TimeoutSeconds { get; set; } = 30;

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