using Mailtrap.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddMailtrap("fc93772517a75afa21bb3c49ce7bdf75");
//builder.Services.AddMailtrap();
builder.Services.AddMailtrap(options =>
{
    options.Token = "1ca2a4a7-fc0a-441f-b9f9-6633f2a246ef";
    options.SendingEnpoint = "https://stoplight.io/mocks/railsware/mailtrap-api-docs/93404133/api/send";
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
