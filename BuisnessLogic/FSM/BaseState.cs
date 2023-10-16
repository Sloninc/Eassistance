using Eassistance.Infrastructure;
using Eassistance.Domain;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot;
using Eassistance.Services;
using Eassistance.Services.Abstract;

namespace Eassistance.BuisnessLogic.FSM
{
    public abstract class BaseState
    {
        protected FSMContext _fsmcontext;
        protected readonly DataContext _context;
        protected readonly IUserService _userService;
        protected readonly TelegramBot _botClient;
        public BaseState(DataContext context, IUserService userService, TelegramBot telegramBot)
        {
            _context = context;
            _userService = userService;
            _botClient = telegramBot; ;
        }
        public void SetContext(FSMContext fsmcontext)
        {
            _fsmcontext = fsmcontext;
        }
        public abstract string Name { get; }
        //public abstract Task ExecuteAsync(Update update);
        public abstract Task Handle(Update update);
    }
}
