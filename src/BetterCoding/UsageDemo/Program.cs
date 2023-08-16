// See https://aka.ms/new-console-template for more information
using UsageDemo.Patterns.Pipeline;
using BetterCoding.Patterns;


var startPipeline = new LanguageDetect();

startPipeline.Next(new Translation()).Next(new EndLine());

var processedText = startPipeline.Execute("hello");
Console.WriteLine(processedText);

var pipelineSupervisor = new PipelineSupervisor<string>(
    new LanguageDetect(),
    new Translation(),
    new EndLine());
processedText = pipelineSupervisor.Execute("world");

Console.WriteLine(processedText);
Console.ReadLine();