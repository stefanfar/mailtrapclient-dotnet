using Mailtrap.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Mailtrap
{
    public class MailtrapClientConfiguration
    {

        public AuthorizationType AuthorizationType { get; set; }

        public string? Token { get; set; }
    }
}
