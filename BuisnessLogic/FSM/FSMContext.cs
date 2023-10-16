using Eassistance.Infrastructure;
using Eassistance.Domain;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace Eassistance.BuisnessLogic.FSM
{
    public class FSMContext
    {
        // Ссылка на текущее состояние Контекста.
        private BaseState _state = null;

        public FSMContext(BaseState state)
        {
            TransitionTo(state);
        }

        // Контекст позволяет изменять объект Состояния во время выполнения.
        public void TransitionTo(BaseState state)
        {
            //Console.WriteLine($"Context: Transition to {state.GetType().Name}.");
            _state = state;
            _state.SetContext(this);
        }

        // Контекст делегирует часть своего поведения текущему объекту
        // Состояния.
        public async Task Request(Update update)
        {
            await _state.Handle(update);
        }

        //public void Request2()
        //{
        //    this._state.Handle2();
        //}
    }
}
