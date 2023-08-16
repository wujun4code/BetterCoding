// See https://aka.ms/new-console-template for more information
using UsageDemo.Patterns.Pipeline;


var startPipeline = new LanguageDetect();

startPipeline.Next(new Translation()).Next(new EndLine());

var processedText = startPipeline.Execute("hello");
Console.WriteLine(processedText);
Console.ReadLine();