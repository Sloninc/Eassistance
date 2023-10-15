using Telegram.Bot.Types;
namespace Eassistance.BuisnessLogic.FSM
{
    public abstract class BaseState
    {
        protected FSMContext _context;

        public void SetContext(FSMContext context)
        {
            this._context = context;
        }
        public abstract string Name { get; }
        public abstract Task ExecuteAsync(Update update);
        public abstract void Handle1();

        public abstract void Handle2();
    }
}
