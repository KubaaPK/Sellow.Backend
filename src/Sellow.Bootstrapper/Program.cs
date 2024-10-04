using Sellow.Shared.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure();

var app = builder.Build();

app.MapControllers();
app.UseInfrastructure();

app.Run();