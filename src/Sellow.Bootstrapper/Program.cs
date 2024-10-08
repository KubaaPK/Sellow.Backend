using Sellow.Modules.Auth.Api;
using Sellow.Modules.EmailSending.Api;
using Sellow.Shared.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructure()
    .AddAuthModule()
    .AddEmailSendingModule();

var app = builder.Build();

app.MapControllers();

app
    .UseInfrastructure()
    .UseAuthModule();

app.Run();