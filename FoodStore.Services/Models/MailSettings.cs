using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Services.Models
{
    public class MailSettings
    {
        public string From { get; set; } = default!;
        public string SmtpServer { get; set; } = default!;
        public int Port { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string DisplayName { get; set; } = default!;

    }
}