namespace BetterCoding.Patterns.StateMachine
{
    public abstract class StateBase<E, A>
    {
        protected StateContext<E, A> _context;
        protected E _entity;

        public StateBase(StateContext<E, A> context, E entity)
        {
            _context = context;
            _entity = entity;
        }

        public abstract A[] AcceptedActions { get; }
        public abstract bool Accept(A action);
        public abstract void Execute(A action);
    }

    public abstract class StateContext<E, A>
    {
        protected StateBase<E, A> _state;

        public StateBase<E, A> State
        {
            get
            {
                return _state;
            }
        }

        public StateContext(StateBase<E, A> state)
        {
            _state = state;
        }

        public virtual void Execute(A action)
        {
            _state.Execute(action);
        }

        public virtual void MoveState(StateBase<E, A> newState)
        {
            _state = newState;
        }
    }
}
