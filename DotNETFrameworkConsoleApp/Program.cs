using Mailtrap;
using Mailtrap.Entities;
using System;
using System.Collections.Generic;

namespace DotNETFrameworkConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var mailtrapClient = new MailtrapClient("<YOUR-TOKEN-HERE>");

            var mail = new Mail
            {
                To = new List<Address> { new Address { Email = "john_doe@example.com", Name = "John Doe" } },
                From = new Address { Email = "sales@example.com", Name = "Example Sales Team" },
                Subject = "Your Example Order Confirmation",
                Html = "<p>Congratulations on your order no. <strong>1234</strong>.</p>",
                Category = "API test"
            };

            mailtrapClient.SendAsync(mail).Wait();

            Console.ReadLine();
        }
    }
}
