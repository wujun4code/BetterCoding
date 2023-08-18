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

    public class AsynchronousPipelineSupervisor<S>
    {
        private IAsynchronousPipeline<S>? _start;
        public AsynchronousPipelineSupervisor(params IAsynchronousPipeline<S>[] pipelines)
        {
            if (pipelines == null || !pipelines.Any()) throw new ArgumentNullException();

            IAsynchronousPipeline<S>? current = null;
            for (var i = 0; i < pipelines.Length; i++)
            {
                if (_start == null)
                {
                    _start = pipelines[i];
                }

                if (current == null)
                {
                    current = pipelines[i];
                }
                else
                    current = current.Next(pipelines[i]);
            }
        }

        public async Task<S> Execute(S input)
        {
            if (_start == null) return input;
            return await _start.ExecuteAsync(input);
        }
    }
}
