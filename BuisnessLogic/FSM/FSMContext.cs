namespace Eassistance.BuisnessLogic.FSM
{
    public class FSMContext
    {
        // Ссылка на текущее состояние Контекста.
        private BaseState _state = null;

        public FSMContext(BaseState state)
        {
            this.TransitionTo(state);
        }

        // Контекст позволяет изменять объект Состояния во время выполнения.
        public void TransitionTo(BaseState state)
        {
            Console.WriteLine($"Context: Transition to {state.GetType().Name}.");
            this._state = state;
            this._state.SetContext(this);
        }

        // Контекст делегирует часть своего поведения текущему объекту
        // Состояния.
        public void Request1()
        {
            this._state.Handle1();
        }

        public void Request2()
        {
            this._state.Handle2();
        }
    }
}
