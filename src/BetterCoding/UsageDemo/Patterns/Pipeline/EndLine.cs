using BetterCoding.Patterns;

namespace UsageDemo.Patterns.Pipeline
{
    public class EndLine: SynchronousPipeline<string>
    {
        public override string Process(string input)
        {
            return $"{input}.";
        }
    }
}
