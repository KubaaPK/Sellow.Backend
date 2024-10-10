using Sellow.Modules.Auth.Api;
using Sellow.Modules.EmailSending.Api;
using Sellow.Modules.Sales.Api;
using Sellow.Shared.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructure()
    .AddAuthModule()
    .AddEmailSendingModule()
    .AddSalesModule();

var app = builder.Build();

app.MapControllers();

app
    .UseInfrastructure()
    .UseAuthModule()
    .UseSalesModule();

app.Run();