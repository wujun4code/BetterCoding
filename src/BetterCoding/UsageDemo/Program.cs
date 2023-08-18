// See https://aka.ms/new-console-template for more information
using BetterCoding.Patterns;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UsageDemo.Entities;
using UsageDemo.Patterns.Pipeline;
using UsageDemo.Patterns.StateMachine;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .AddFilter("Microsoft", LogLevel.Warning)
        .AddFilter("System", LogLevel.Warning)
        .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
        .AddConsole();
});
ILogger logger = loggerFactory.CreateLogger<Program>();

logger.LogInformation("Hello, BetterCoding");

ServiceCollection serviceCollection = new ServiceCollection();
serviceCollection.AddLogging();

#region Pipeline 
var startPipeline = new LanguageDetect();

startPipeline.Next(new Translation()).Next(s => $"{s}, ya").Next(new EndLine());

var processedText = startPipeline.Execute("hello");
Console.WriteLine(processedText);

var pipelineSupervisor = new SynchronousPipelineSupervisor<string>(
    new LanguageDetect(),
    new Translation(),
    new EndLine());

processedText = pipelineSupervisor.Execute("world");

Console.WriteLine(processedText);
#endregion

#region State Machine

var ticket = new JiraTicket
{
    Status = JiraTicketStatus.Draft,
    Title = "learn something abount Kubernetes Ingress",
    Id = new Random().Next(100),
    Created = DateTime.UtcNow,
    Updated = DateTime.UtcNow
};

try
{
    var jiraTicketStateContext = new JiraTicketStateContext(ticket);
    Console.WriteLine(jiraTicketStateContext.State?.StateFriendlyName);

    // do persisten storage action here.
    // something like dbRepository.Save(ticket);
    jiraTicketStateContext.Execute(JiraTicketAction.Add);
    Console.WriteLine(jiraTicketStateContext.State?.StateFriendlyName);

    jiraTicketStateContext.Execute(JiraTicketAction.Start);
    Console.WriteLine(jiraTicketStateContext.State?.StateFriendlyName);

    jiraTicketStateContext.Execute(JiraTicketAction.Done);
    Console.WriteLine(jiraTicketStateContext.State?.StateFriendlyName);

    jiraTicketStateContext.Execute(JiraTicketAction.ResetToNotStarted);
    Console.WriteLine(jiraTicketStateContext.State?.StateFriendlyName);

    jiraTicketStateContext.Execute(JiraTicketAction.Delete);
    Console.WriteLine(jiraTicketStateContext.State?.StateFriendlyName);
}
catch (Exception ex)
{
    logger.LogError(ex, ex.Message);
}
#endregion

Console.ReadLine();