using Sellow.Modules.Auth.Api;
using Sellow.Shared.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructure()
    .AddAuthModule();

var app = builder.Build();

app.MapControllers();

app
    .UseInfrastructure()
    .UseAuthModule();

app.Run();