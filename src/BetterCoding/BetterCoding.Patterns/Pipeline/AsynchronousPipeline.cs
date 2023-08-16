namespace BetterCoding.Patterns
{
    public interface IAsynchronousPipeline<S>
    {
        Task<S> ProcessAsync(S input);
        Task<S> ExecuteAsync(S input);
        IAsynchronousPipeline<S> Next(IAsynchronousPipeline<S> nextNode);
        IAsynchronousPipeline<S> Next(Func<S, Task<S>> nextProcessor);
    }

    public abstract class AsynchronousPipeline<S> : IAsynchronousPipeline<S>
    {
        public IAsynchronousPipeline<S>? NextAsynchronousNode { get; set; }

        public abstract Task<S> ProcessAsync(S input);

        public virtual IAsynchronousPipeline<S> Next(IAsynchronousPipeline<S> nextNode)
        {
            NextAsynchronousNode = nextNode;
            return nextNode;
        }

        public virtual IAsynchronousPipeline<S> Next(Func<S, Task<S>> nextProcessor)
        {
            var funcPipeline = new AsynchronousFuncPipeline<S>(nextProcessor);
            return Next(funcPipeline);
        }

        public virtual async Task<S> ExecuteAsync(S input)
        {
            var s = await ProcessAsync(input);
            if (NextAsynchronousNode != null)
            {
                s = await NextAsynchronousNode.ExecuteAsync(s);
            }
            return s;
        }
    }

    public class AsynchronousFuncPipeline<S> : AsynchronousPipeline<S>
    {
        private readonly Func<S, Task<S>>? _processor;
        public AsynchronousFuncPipeline(Func<S, Task<S>> processor)
        {
            _processor = processor;
        }

        public override Task<S> ProcessAsync(S input)
        {
            if (_processor == null) throw new InvalidOperationException();
            return _processor(input);
        }
    }
}
