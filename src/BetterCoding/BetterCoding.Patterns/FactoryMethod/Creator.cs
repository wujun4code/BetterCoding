namespace BetterCoding.Patterns.FactoryMethod
{
    public interface IProcessor<T>
    {
        T Execute();
    }

    public abstract class Creator<T>
        where T : IProcessor<T>
    {
        public abstract T Create();

        public T Execute()
        {
            var processor = Create();
            return processor.Execute();
        }
    }
}
