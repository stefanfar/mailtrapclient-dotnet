using Mailtrap.Configuration;
using Mailtrap.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Option 1
// The options must be set in the application settings
//builder.Services.AddMailtrap();

// Option 2
builder.Services.AddMailtrap(options =>
{
    options.Token = "<YOUR-TOKEN-HERE>"; // required
    options.SendingEnpoint = "<SENDING-ENDPOINT>";
    options.AuthorizationType = AuthorizationType.BearerAuth;
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
