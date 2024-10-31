using iPipelinePStepWebService;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;

var builder = Host.CreateApplicationBuilder(args);

/**************************************
 * Configure as a Windows Service
 ******************************************/
builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "iPipeline";
});

LoggerProviderOptions.RegisterProviderOptions<EventLogSettings, EventLogLoggerProvider>(builder.Services);

builder.Services.AddHostedService<Worker>();

builder.Logging.AddConfiguration(
    builder.Configuration.GetSection("Settings"));

var host = builder.Build();
host.Run();
