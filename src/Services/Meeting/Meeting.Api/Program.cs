using Meeting.Api.ServicesExtension;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Setup(CreateProgramLogger(), builder.Configuration);

var app = builder.Build();
app.Configure();

app.Run();

static ILogger CreateProgramLogger()
{
    using var loggerFactory = LoggerFactory.Create(builder =>
    {
        builder.SetMinimumLevel(LogLevel.Information);
        builder.AddConsole();
        builder.AddEventSourceLogger();
    });

    return loggerFactory.CreateLogger<Program>();
}