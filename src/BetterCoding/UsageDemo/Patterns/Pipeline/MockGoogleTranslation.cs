using BetterCoding.Patterns;

namespace UsageDemo.Patterns.Pipeline
{
    public class MockGoogleTranslation : AsynchronousPipeline<string>
    {
        public override Task<string> ProcessAsync(string input)
        {
            return Task.FromResult($"{input}-with-google-translated-text");
        }
    }
}
