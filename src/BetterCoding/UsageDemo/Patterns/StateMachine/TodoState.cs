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
    }

    public enum TodoAction
    {
        Add,
        Delete,
        Start,
        ResetToNotStarted,
        Done,
        Close,
        Discard,
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
            TodoAction.Discard,
        };

        public override void Execute(TodoAction action)
        {
            base.Execute(action);
        }
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

    public class InProgressState : TodoStateBase<TodoItem, TodoAction>
    {
        public InProgressState(
            StateContext<TodoItem, TodoAction> context,
            TodoItem todoItem) : base(context, todoItem)
        {

        }

        public override TodoAction[] AcceptedActions => new TodoAction[]
        {
            TodoAction.ResetToNotStarted,
            TodoAction.Delete,
            TodoAction.Done,
            TodoAction.Close
        };
    }

    public class DoneState : TodoStateBase<TodoItem, TodoAction>
    {
        public DoneState(
            StateContext<TodoItem, TodoAction> context,
            TodoItem todoItem) : base(context, todoItem)
        {

        }

        public override TodoAction[] AcceptedActions => new TodoAction[]
        {
            TodoAction.ResetToNotStarted,
            TodoAction.Delete,
        };
    }

    public class DeletedState : TodoStateBase<TodoItem, TodoAction>
    {
        public DeletedState(
            StateContext<TodoItem, TodoAction> context,
            TodoItem todoItem) : base(context, todoItem)
        {

        }

        public override TodoAction[] AcceptedActions => new TodoAction[]
        {
            TodoAction.ResetToNotStarted,
        };
    }

    public class ClosedState : TodoStateBase<TodoItem, TodoAction>
    {
        public ClosedState(
            StateContext<TodoItem, TodoAction> context,
            TodoItem todoItem) : base(context, todoItem)
        {

        }

        public override TodoAction[] AcceptedActions => new TodoAction[]
        {
            TodoAction.ResetToNotStarted,
            TodoAction.Delete,
        };
    }

    public class TodoStateContext : StateContext<TodoItem, TodoAction>
    {
        public TodoStateContext(TodoItem todoItem)
        {
            switch (todoItem.Status)
            {
                case TodoItemStatus.Draft:
                    _state = new EditingState(this, todoItem);
                    break;
                case TodoItemStatus.New:
                    _state = new NotStartedState(this, todoItem);
                    break;
                case TodoItemStatus.InProgress:
                    _state = new InProgressState(this, todoItem);
                    break;
                case TodoItemStatus.Done:
                    _state = new DoneState(this, todoItem);
                    break;
                case TodoItemStatus.Deleted:
                    _state = new ClosedState(this, todoItem);
                    break;
                case TodoItemStatus.Closed:
                    _state = new DeletedState(this, todoItem);
                    break;
            }
        }

        public TodoStateContext(StateBase<TodoItem, TodoAction> state) : base(state)
        {

        }
    }
}
