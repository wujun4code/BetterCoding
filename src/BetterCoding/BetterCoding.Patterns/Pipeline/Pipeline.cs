namespace BetterCoding.Patterns
{
    public interface IPipeline<S>
    {
        S Process(S input);
        IPipeline<S> Next(IPipeline<S> nextNode);
        S Execute(S input);
    }

    public abstract class Pipeline<S> : IPipeline<S>
    {
        public IPipeline<S>? NextNode { get; set; }

        public abstract S Process(S input);

        public virtual IPipeline<S> Next(IPipeline<S> nextNode)
        {
            NextNode = nextNode;
            return nextNode;
        }

        public virtual S Execute(S input)
        {
            var s = Process(input);
            if (NextNode != null)
                s = NextNode.Execute(s);
            return s;
        }
    }

    public class PipelineSupervisor<S>
    {
        private IPipeline<S> _start;
        public PipelineSupervisor(params IPipeline<S>[] pipelines)
        {
            if (pipelines == null || !pipelines.Any()) throw new ArgumentNullException();

            IPipeline<S> current = null;
            for ( var i = 0; i < pipelines.Length; i++) 
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
            return _start.Execute(input);
        }
    }
}