namespace BetterCoding.Patterns
{
    public interface ISynchronousPipeline<S>
    {
        S Process(S input);
        S Execute(S input);
        ISynchronousPipeline<S> Next(ISynchronousPipeline<S> nextNode);
        ISynchronousPipeline<S> Next(Func<S, S> nextProcessor);
    }

    public abstract class SynchronousPipeline<S> : ISynchronousPipeline<S>
    {
        public ISynchronousPipeline<S>? NextNode { get; set; }

        public abstract S Process(S input);

        public virtual ISynchronousPipeline<S> Next(ISynchronousPipeline<S> nextNode)
        {
            NextNode = nextNode;
            return nextNode;
        }

        public virtual ISynchronousPipeline<S> Next(Func<S, S> nextProcessor)
        {
            var funcPipeline = new SynchronousFuncPipeline<S>(nextProcessor);
            return Next(funcPipeline);
        }

        public virtual S Execute(S input)
        {
            var s = Process(input);
            if (NextNode != null)
                s = NextNode.Execute(s);
            return s;
        }
    }

    public class SynchronousFuncPipeline<S> : SynchronousPipeline<S>
    {
        private readonly Func<S, S>? _processor;
        public SynchronousFuncPipeline(Func<S, S> processor)
        {
            _processor = processor;
        }

        public override S Process(S input)
        {
            if (_processor == null) throw new InvalidOperationException();
            return _processor(input);
        }
    }

    public class PipelineSupervisor<S>
    {
        private ISynchronousPipeline<S>? _start;
        public PipelineSupervisor(params ISynchronousPipeline<S>[] pipelines)
        {
            if (pipelines == null || !pipelines.Any()) throw new ArgumentNullException();

            ISynchronousPipeline<S>? current = null;
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

        public S Execute(S input)
        {
            if (_start == null) return input;
            return _start.Execute(input);
        }
    }
}