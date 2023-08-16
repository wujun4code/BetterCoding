namespace BetterCoding.Patterns
{
    public interface IPipeline<S>
    {
        S Process(S input);
        S Execute(S input);
        ISynchronousPipeline<S> Next(ISynchronousPipeline<S> nextNode);
        ISynchronousPipeline<S> Next(Func<S, S> nextProcessor);

        Task<S> ProcessAsync(S input);
        Task<S> ExecuteAsync(S input);
        IAsynchronousPipeline<S> Next(IAsynchronousPipeline<S> nextNode);
        IAsynchronousPipeline<S> Next(Func<S, Task<S>> nextProcessor);
    }

    public abstract class Pipeline<S> : IPipeline<S>
    {
        public ISynchronousPipeline<S>? NextSynchronousNode { get; set; }
        public IAsynchronousPipeline<S>? NextAsynchronousNode { get; set; }
        public abstract S Process(S input);

        public abstract Task<S> ProcessAsync(S input);

        public S Execute(S input)
        {
            var s = Process(input);
            if (NextSynchronousNode != null)
                s = NextSynchronousNode.Execute(s);
            return s;
        }

        public async Task<S> ExecuteAsync(S input)
        {
            var s = await ProcessAsync(input);
            if (NextAsynchronousNode != null)
            {
                s = await NextAsynchronousNode.ExecuteAsync(s);
            }
            return s;
        }

        public virtual ISynchronousPipeline<S> Next(ISynchronousPipeline<S> nextNode)
        {
            NextSynchronousNode = nextNode;
            return nextNode;
        }

        public virtual ISynchronousPipeline<S> Next(Func<S, S> nextProcessor)
        {
            var funcPipeline = new SynchronousFuncPipeline<S>(nextProcessor);
            return Next(funcPipeline);
        }

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
    }
}
