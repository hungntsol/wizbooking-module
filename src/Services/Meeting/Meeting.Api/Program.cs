using Meeting.Api.ServicesExtension;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Setup(builder.Configuration);

var app = builder.Build();
app.Configure();

app.Run();
