namespace BetterCoding.Patterns.FactoryMethod
{
    public interface IProcessor<S, T>
    {
        T Execute(S source);
    }

    public abstract class Factory<P, S, T>
        where P : IProcessor<S, T>
    {
        public abstract P Create();

        public T Execute(S source)
        {
            var processor = Create();
            return processor.Execute(source);
        }
    }
}
