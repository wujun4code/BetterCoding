using BetterCoding.Patterns.StateMachine;
using System.Linq;
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
        public override bool Accept(TAction action)
        {
            return AcceptedActions.Contains(action);
        }

        public override void Execute(TAction action)
        {
            if (!Accept(action)) throw new InvalidOperationException($"can not execute {action?.GetType()} on state {GetType()}");
        }

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
        Start,
        Done,
        Close,
    }

    public class EditingState : TodoStateBase<TodoItem, TodoAction>
    {
        public EditingState(
            StateContext<TodoItem, TodoAction> context,
            TodoItem todoItem) : base(context, todoItem)
        {

        }

        public override TodoAction[] AcceptedActions => new TodoAction[]
        {
            TodoAction.Add,
            TodoAction.Delete,
        };
    }

    public class NotStartedState : TodoStateBase<TodoItem, TodoAction>
    {
        public NotStartedState(
            StateContext<TodoItem, TodoAction> context,
            TodoItem todoItem) : base(context, todoItem)
        {

        }

        public override TodoAction[] AcceptedActions => new TodoAction[]
        {
            TodoAction.Delete,
            TodoAction.Start,
            TodoAction.Done,
            TodoAction.Close
        };
    }

    public class TodoStateContext : StateContext<TodoItem, TodoAction>
    {
        public TodoStateContext(TodoItem todoItem)
        {
            if (todoItem.Id < 1)
                _state = new EditingState(this, todoItem);
            else if (todoItem.Status == 1)
            {
                _state = new NotStartedState(this, todoItem);
            }
        }

        public TodoStateContext(StateBase<TodoItem, TodoAction> state) : base(state)
        {

        }
    }
}
