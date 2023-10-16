using Eassistance.Infrastructure;
using Eassistance.Domain;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Eassistance.Services;
using Eassistance.Services.Abstract;
using Telegram.Bot;

namespace Eassistance.BuisnessLogic.FSM
{
    public class StartState : BaseState
    {
        public StartState(DataContext context, IUserService userService, TelegramBot telegramBot) : base(context, userService, telegramBot)
        {
            Name = "start";
        }
        public override string Name { get; }
        //public abstract Task ExecuteAsync(Update update);
        public async override Task Handle(Update update)
        {
            //var user = await _userService.GetOrCreate(update);
            var user = await _context.Users.FirstOrDefaultAsync(x => x.ChatId == update.Message.Chat.Id);
            if (user == null)
            {
                _fsmcontext.TransitionTo(new RegistrationState(_context, _userService, _botClient));
                _fsmcontext.Request(update);
            }
            else
            {
                var inlineKeyboard = new ReplyKeyboardMarkup(new[]
             {
                new[]
                    {
                    new KeyboardButton("Список узлов"),
                    new KeyboardButton("Добавить узел"),
                    new KeyboardButton("Удалить узел")
                    }
            });
                inlineKeyboard.ResizeKeyboard = true;
                await _botClient.GetBot().Result.SendTextMessageAsync(user.ChatId, "Добро пожаловать! Выберите команду! ", replyMarkup: inlineKeyboard);
            }
        }
    }
}
