using Mailtrap;
using System;
using System.Collections.Generic;

namespace DotNETFrameworkConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var mail = new Mailtrap.MailtrapClient("fc93772517a75afa21bb3c49ce7bdf75");

            var mailparams = new Mail
            {
                To = new List<Address> { new Address { Email = "4plx4.test@inbox.testmail.app", Name = "abc" } },
                From = new Address { Email = "stefan.farcas9@gmail.com", Name = "abc" },
                Subject = "subiect",
                //Text = "un text",
                Html = "<!doctype html><p>a</p>",
                Category = "API test"
            };


            mail.SendAsync(mailparams).Wait();

            Console.ReadLine();
        }
    }
}
