# Mailtrap .NET client

This library offers integration with the [official API](https://api-docs.mailtrap.io/) for [Mailtrap](https://mailtrap.io).
It is extensible, so far offering the possibility to asynchronously send an email.
It targets .NET Standard, so it can be used in both .NET Framework and .NET. 

## Usage

### Minimal

```cs
using Mailtrap;

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
});
```

Refer to the [`DotNETFrameworkConsoleApp`](DotNETFrameworkConsoleApp) and [`DotNETWebApplication`](DotNETWebApplication) projects for examples of how to use the service.

### Retry mechanism

In the case that the sending request fails with a status codes >= 500 (server errors) or status code 408 (request timeout), the sending is retried one more time.

### Logging

MailtrapClient uses the ILogger interface, received through dependency injection, allowing the configured logging provider to receive logs from the mailing service.

### Sending API

 - [Advanced](examples/sending/everything.ts)
 - [Minimal](examples/sending/minimal.ts)
 - [Send email using template](examples/sending/template.ts)
 - [Nodemailer transport](examples/sending/transport.ts)

### Email testing API

 - [Attachments](examples/testing/attachments.ts)
 - [Inboxes](examples/testing/inboxes.ts)
 - [Messages](examples/testing/messages.ts)
 - [Projects](examples/testing/projects.ts)
 - [Send mail using template](examples/testing/template.ts)

### Nodemailer Transport

> NOTE: [Nodemailer](https://www.npmjs.com/package/nodemailer) is needed as a dependency.

```sh
yarn add nodemailer

# or, if you are using NPM:
npm install --s nodemailer
```

If you're using Typescript, install `@types/nodemailer` as a `devDependency`.

```sh
yarn add --dev @types/nodemailer

# or, if you are using NPM:
npm install --s-dev @types/nodemailer

You can provide Mailtrap specific keys like `category`, `customVariables`, `templateUuid` and `templateVariables`.

```
See transport usage below:

 - [Transport](examples/transport.ts)

## Development

This library is developed using [TypeScript](https://www.typescriptlang.org).

Use `yarn lint` to perform linting with [ESLint](https://eslint.org).

## Contributing

Bug reports and pull requests are welcome on [GitHub](https://github.com/railsware/mailtrap-nodejs). This project is intended to be a safe, welcoming space for collaboration, and contributors are expected to adhere to the [code of conduct](CODE_OF_CONDUCT.md).

## License

The package is available as open source under the terms of the [MIT License](https://opensource.org/licenses/MIT).

## Code of Conduct

Everyone interacting in the Mailtrap project's codebases, issue trackers, chat rooms and mailing lists is expected to follow the [code of conduct](CODE_OF_CONDUCT.md).
