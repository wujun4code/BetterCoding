using BetterCoding.Patterns.StateMachine;
using UsageDemo.Entities;

namespace UsageDemo.Patterns.StateMachine
{
    public abstract class TodoStateBase<TEntity, TAction> : StateBase<TEntity, TAction>
    {
        public TodoStateBase(
          StateContext<TEntity, TAction> context,
          TEntity entity) : base(context, entity)
        {

        }

        public override TAction[] AcceptedActions => new TAction[0];

        public virtual string StateFriendlyName
        {
            get
            {
                return GetType().Name;
            }
        }
    }

    public enum TodoAction
    {
        Add,
        Delete,
        Start
    }

    public class NotStartedState : TodoStateBase<TodoItem, TodoAction> 
    {
        public NotStartedState(StateContext<TodoItem, TodoAction> context, TodoItem todoItem) : base(context, todoItem)
        {

        }

        public override bool Accept(TodoAction action)
        {
            throw new NotImplementedException();
        }

        public override void Execute(TodoAction action)
        {
            throw new NotImplementedException();
        }
    }
}
