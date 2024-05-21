# Mailtrap .NET client

This library offers integration with the [official API](https://api-docs.mailtrap.io/) for [Mailtrap](https://mailtrap.io).

It is extensible, so far offering the possibility to asynchronously send an email.
It targets .NET Standard 2,0, so it can be used in both .NET, .NET Core and .NET Framework (4.6.1 or higher). 

## Usage

### Minimal

```cs
using Mailtrap;
using Mailtrap.Entities;

/**
 * For this example to work, you need to set up a sending domain,
 * and obtain a token that is authorized to send from the domain.
 */

const token = "<YOUR-TOKEN-HERE>";
const senderEmail = "<SENDER@YOURDOMAIN.COM>";
const recipientEmail = "<RECIPIENT@EMAIL.COM>";

var mailtrapClient = new MailtrapClient(token);

var mail = new Mail
{
    To = new List<Address> { new Address { Email = senderEmail, Name = "John Doe" } },
    From = new Address { Email = recipientEmail, Name = "Example Sales Team" },
    Subject = "Your Example Order Confirmation",
    Html = "<p>Congratulations on your order no. <strong>1234</strong>.</p>",
    Category = "API test"
};

mailtrapClient.SendAsync(mail).Wait();
```

### Creating the MailtrapClient

As seen in the example above, the simplest method is passing the token directly to the constructor, method which can be used in .NET Framework and .NET:
```cs
var mailtrapClient = new MailtrapClient("<YOUR-TOKEN-HERE>");
```

By using the dependency injection build into .NET, the MailtrapClient service can be registered using the following extension method:
```cs
builder.Services.AddMailtrap();
```

This approach offers more flexibility, since the MailtrapClient configuration is retrieved from the application settings:
```json
  "Mailtrap": {
    "Token": "<YOUR-TOKEN-HERE>",
    "AuthorizationType": "ApiKey",
    "SendingEnpoint": "<SENDING-ENDPOINT>"
  }
```

Alternatively, the configuration can be provided using the options pattern:
```cs
builder.Services.AddMailtrap(options =>
{
    options.Token = "<YOUR-TOKEN-HERE>";
    options.SendingEnpoint = "<SENDING-ENDPOINT>";
    options.AuthorizationType = AuthorizationType.BearerAuth;
});
```

Refer to the [`DotNETFrameworkConsoleApp`](DotNETFrameworkConsoleApp) and [`DotNETWebApplication`](DotNETWebApplication) projects for examples of how to use the service.

### Retry mechanism

In the case that the sending request fails with a status code >= 500 (server errors) or status code 408 (request timeout), the sending is retried one more time.

### Logging

MailtrapClient uses the [ILogger](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.ilogger?view=netstandard-2.0) interface, received through dependency injection, allowing the configured logging provider to receive logs from the mailing service.

### Notes

The token is required when creating an instance of [MailtrapClient](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=netstandard-2.0), and should be provided to the constructor in one of the three possible ways: the _token_ parameter, settings or options.

There are two types of authorization which can be configured: Api-token and Bearer authorization.

When working with a .NET application, it is recommended to register the library in configuration through the provided extension method, due to known limitations of [HttpClient](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=netstandard-2.0), which is used behind the scenes ([Guidelines for using HttpClient](https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient-guidelines)).

The message serialization was done using the [System.Text.Json](https://learn.microsoft.com/en-us/dotnet/api/system.text.json?view=netstandard-2.0) library.

Possible exceptions thrown by the [SendAsync](https://github.com/stefanfar/Mailtrap/blob/master/Mailtrap/MailtrapClient.cs) method: 
[ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception?view=netstandard-2.0), 
[ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception?view=netstandard-2.0), 
[NotSupportedException](https://learn.microsoft.com/en-us/dotnet/api/system.notsupportedexception?view=netstandard-2.0), 
[HttpRequestException](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httprequestexception?view=netstandard-2.0), 
[JsonException](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.jsonexception?view=netstandard-2.0)
