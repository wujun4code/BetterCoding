// See https://aka.ms/new-console-template for more information
using UsageDemo.Patterns.Pipeline;
using BetterCoding.Patterns;
using UsageDemo.Entities;
using UsageDemo.Patterns.StateMachine;

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

var todo = new TodoItem { Status = TodoItemStatus.Draft };
var todoStateContext = new TodoStateContext(todo);
todoStateContext.Execute(TodoAction.Add);
Console.WriteLine(todoStateContext.State?.StateFriendlyName);
#endregion

Console.ReadLine();
