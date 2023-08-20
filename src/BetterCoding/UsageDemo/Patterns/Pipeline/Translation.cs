using BetterCoding.Patterns.Pipeline;
namespace UsageDemo.Patterns.Pipeline
{
    public class Translation : SynchronousPipeline<string>
    {
        public override string Process(string input)
        {
            switch (input)
            {
                case "hello":
                    return "你好";
                case "world":
                    return "世界";
            }
            return input;
        }
    }
}
