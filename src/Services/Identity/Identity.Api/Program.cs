using Identity.Api.ServiceExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Setup(builder.Configuration);

var app = builder.Build();
app.Configure();

app.Run();
