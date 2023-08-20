using BetterCoding.Patterns.FactoryMethod;

namespace UsageDemo.Patterns.FactoryMethod
{
    public interface ITranslator : IProcessor<string, Task<string>>
    {

    }

    public class GoogleAPITranslatorV1 : ITranslator
    {
        public Task<string> Execute(string source)
        {
            return Task.FromResult($"{source} tansltated by Google API V1");
        }
    }

    public class GoogleAPITranslatorV2 : ITranslator
    {
        public Task<string> Execute(string source)
        {
            return Task.FromResult($"{source} tansltated by Google API V2");
        }
    }

    public class GoogleTranslationFactory : Factory<ITranslator, string, Task<string>>
    {
        public override ITranslator Create()
        {
            // switch V1 or V2 here, we can control business logic here
            return new GoogleAPITranslatorV2();
        }
    }

    public class OpenAIAPITranslator : ITranslator
    {
        public Task<string> Execute(string source)
        {
            return Task.FromResult($"{source} tansltated by OpenAI API");
        }
    }

    public class OpenAITranslatorFactory : Factory<ITranslator, string, Task<string>>
    {
        public override ITranslator Create()
        {
            return new OpenAIAPITranslator();
        }
    }
}
