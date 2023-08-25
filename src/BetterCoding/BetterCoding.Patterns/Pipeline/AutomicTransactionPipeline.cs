namespace BetterCoding.Patterns.Pipeline
{
    public abstract class AutomicTransactionPipeline<S> : AsynchronousPipeline<S>
    {
        public abstract Task<S> RevertAsync(S input);

        public async override Task<S> ExecuteAsync(S input)
        {
            var success = false;
            S result = default(S);
            try
            {
                result = await ProcessAsync(input);
                success = true;
            }
            catch
            {
                result = await RevertAsync(input);
            }
            if (success && NextAsynchronousNode != null)
            {
                result = await NextAsynchronousNode.ExecuteAsync(result);
            }
            return result;
        }
    }
}
