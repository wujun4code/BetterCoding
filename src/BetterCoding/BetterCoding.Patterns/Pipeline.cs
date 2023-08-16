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
}