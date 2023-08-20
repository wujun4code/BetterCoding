using BetterCoding.Patterns.StateMachine;
using UsageDemo.Entities;

namespace UsageDemo.Patterns.StateMachine
{
    public abstract class JiraTicketStateBase<TEntity, TAction> : StateBase<TEntity, TAction>
    {
        public JiraTicketStateBase(
            StateContext<TEntity, TAction> context,
            TEntity entity) : base(context, entity)
        {

        }
    }

    public enum JiraTicketAction
    {
        Add,
        Delete,
        Start,
        ResetToNotStarted,
        Done,
        Close,
        Discard,
    }

    public class EditingState : JiraTicketStateBase<JiraTicket, JiraTicketAction>
    {
        public EditingState(
            StateContext<JiraTicket, JiraTicketAction> context,
            JiraTicket jiraTikect) : base(context, jiraTikect)
        {

        }

        public override JiraTicketAction[] AcceptedActions => new JiraTicketAction[]
        {
            JiraTicketAction.Add,
            JiraTicketAction.Delete,
            JiraTicketAction.Discard,
        };

        public override void Execute(JiraTicketAction action)
        {
            base.Execute(action);
            switch (action)
            {
                case JiraTicketAction.Add:
                    // you can save to database here `datbaseRepository.Save(_entity);`, but this is not an expected practise.
                    // you'd better call asynchoronus action out of here, state machine pattern is suggested to control workflow,
                    // We isolate workflow control and persistent storage.
                    // Persistent storage should not be put together with business processes.
                    // We should perform persistent storage operations between each state transition
                    _entity.Status = JiraTicketStatus.New;
                    _context.MoveState(new NotStartedState(_context, _entity));
                    break;

                case JiraTicketAction.Delete:
                    _entity.Status = JiraTicketStatus.Deleted;
                    _context.MoveState(new DeletedState(_context, _entity));
                    break;
            }
        }
    }

    public class NotStartedState : JiraTicketStateBase<JiraTicket, JiraTicketAction>
    {
        public NotStartedState(
            StateContext<JiraTicket, JiraTicketAction> context,
            JiraTicket jiraTicket) : base(context, jiraTicket)
        {

        }
        
        public override JiraTicketAction[] AcceptedActions => new JiraTicketAction[]
        {
            JiraTicketAction.Delete,
            JiraTicketAction.Start,
            JiraTicketAction.Done,
            JiraTicketAction.Close
        };

        public override void Execute(JiraTicketAction action)
        {
            base.Execute(action);
            switch (action)
            {
                case JiraTicketAction.Start:
                    _entity.Status = JiraTicketStatus.InProgress;
                    _context.MoveState(new InProgressState(_context, _entity));
                    break;

                case JiraTicketAction.Delete:
                    _entity.Status = JiraTicketStatus.Deleted;
                    _context.MoveState(new DeletedState(_context, _entity));
                    break;

                case JiraTicketAction.Done:
                    _entity.Status = JiraTicketStatus.Done;
                    _context.MoveState(new DoneState(_context, _entity));
                    break;
                case JiraTicketAction.Close:
                    _entity.Status = JiraTicketStatus.Done;
                    _context.MoveState(new ClosedState(_context, _entity));
                    break;
            }
        }
    }

    public class InProgressState : JiraTicketStateBase<JiraTicket, JiraTicketAction>
    {
        public InProgressState(
            StateContext<JiraTicket, JiraTicketAction> context,
            JiraTicket jiraTicket) : base(context, jiraTicket)
        {

        }

        public override JiraTicketAction[] AcceptedActions => new JiraTicketAction[]
        {
            JiraTicketAction.ResetToNotStarted,
            JiraTicketAction.Delete,
            JiraTicketAction.Done,
            JiraTicketAction.Close
        };

        public override void Execute(JiraTicketAction action)
        {
            base.Execute(action);
            switch (action)
            {
                case JiraTicketAction.ResetToNotStarted:
                    _entity.Status = JiraTicketStatus.New;
                    _context.MoveState(new NotStartedState(_context, _entity));
                    break;

                case JiraTicketAction.Delete:
                    _entity.Status = JiraTicketStatus.Deleted;
                    _context.MoveState(new DeletedState(_context, _entity));
                    break;

                case JiraTicketAction.Done:
                    _entity.Status = JiraTicketStatus.Done;
                    _context.MoveState(new DoneState(_context, _entity));
                    break;
                case JiraTicketAction.Close:
                    _entity.Status = JiraTicketStatus.Done;
                    _context.MoveState(new ClosedState(_context, _entity));
                    break;
            }
        }
    }

    public class DoneState : JiraTicketStateBase<JiraTicket, JiraTicketAction>
    {
        public DoneState(
            StateContext<JiraTicket, JiraTicketAction> context,
            JiraTicket jiraTicket) : base(context, jiraTicket)
        {

        }

        public override JiraTicketAction[] AcceptedActions => new JiraTicketAction[]
        {
            JiraTicketAction.ResetToNotStarted,
            JiraTicketAction.Delete,
        };

        public override void Execute(JiraTicketAction action)
        {
            base.Execute(action);
            switch (action)
            {
                case JiraTicketAction.ResetToNotStarted:
                    _entity.Status = JiraTicketStatus.New;
                    _context.MoveState(new NotStartedState(_context, _entity));
                    break;

                case JiraTicketAction.Delete:
                    _entity.Status = JiraTicketStatus.Deleted;
                    _context.MoveState(new DeletedState(_context, _entity));
                    break;
            }
        }
    }

    public class DeletedState : JiraTicketStateBase<JiraTicket, JiraTicketAction>
    {
        public DeletedState(
            StateContext<JiraTicket, JiraTicketAction> context,
            JiraTicket jiraTicket) : base(context, jiraTicket)
        {

        }

        public override JiraTicketAction[] AcceptedActions => new JiraTicketAction[]
        {
            JiraTicketAction.ResetToNotStarted,
        };
    }

    public class ClosedState : JiraTicketStateBase<JiraTicket, JiraTicketAction>
    {
        public ClosedState(
            StateContext<JiraTicket, JiraTicketAction> context,
            JiraTicket jiraTicket) : base(context, jiraTicket)
        {

        }

        public override JiraTicketAction[] AcceptedActions => new JiraTicketAction[]
        {
            JiraTicketAction.ResetToNotStarted,
            JiraTicketAction.Delete,
        };
    }

    public class JiraTicketStateContext : StateContext<JiraTicket, JiraTicketAction>
    {
        public JiraTicketStateContext(JiraTicket jiraTicket)
        {
            switch (jiraTicket.Status)
            {
                case JiraTicketStatus.Draft:
                    _state = new EditingState(this, jiraTicket);
                    break;
                case JiraTicketStatus.New:
                    _state = new NotStartedState(this, jiraTicket);
                    break;
                case JiraTicketStatus.InProgress:
                    _state = new InProgressState(this, jiraTicket);
                    break;
                case JiraTicketStatus.Done:
                    _state = new DoneState(this, jiraTicket);
                    break;
                case JiraTicketStatus.Deleted:
                    _state = new ClosedState(this, jiraTicket);
                    break;
                case JiraTicketStatus.Closed:
                    _state = new DeletedState(this, jiraTicket);
                    break;
            }
        }

        public JiraTicketStateContext(StateBase<JiraTicket, JiraTicketAction> state) : base(state)
        {

        }
    }
}
