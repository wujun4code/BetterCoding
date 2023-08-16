using BetterCoding.Patterns;

namespace UsageDemo.Patterns.Pipeline
{
    public class LanguageDetect : Pipeline<string>
    {
        public override string Process(string input)
        {
            if (input.Contains("hello") || input.Contains("world"))
                return input;

            return string.Empty;
        }
    }
}
