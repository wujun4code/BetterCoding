using BetterCoding.Patterns.Pipeline;

namespace UsageDemo.Patterns.Pipeline
{
    public class MockOpenAITranslation : AutomicTransactionPipeline<string>
    {
        public override Task<string> ProcessAsync(string input)
        {
            throw new NotImplementedException();
        }

        public override Task<string> RevertAsync(string input)
        {
            return Task.FromResult(input);
        }
    }
}
