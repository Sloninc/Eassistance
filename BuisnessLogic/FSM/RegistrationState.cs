using Eassistance.Infrastructure;
using Eassistance.Domain;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Eassistance.Services;
using Eassistance.Services.Abstract;
using Telegram.Bot;

namespace Eassistance.BuisnessLogic.FSM
{
    public class RegistrationState:BaseState
    {
        public RegistrationState(DataContext context, IUserService userService, TelegramBot telegramBot) : base(context, userService, telegramBot)
        {
            Name = "registration";
        }
        public override string Name { get; }
        //public abstract Task ExecuteAsync(Update update);
        public async override Task Handle(Update update)
        {
            try
            {
                switch (update.Message.Text)
                {
                    case "/start":
                        const string message = "введите имя и фамилию через пробел";
                        await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, message);
                        break;
                    default:
                        if (update.Message.Text == null)
                            await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, "вы ничего не ввели");
                        else if (update.Message.Text.Split(' ').Count() != 2)
                            await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, "Введите имя и фамилию через пробел!");
                        else
                        {
                            var user = new EAUser
                            {
                                Username = update.Message.Chat.Username,
                                ChatId = update.Message.Chat.Id,
                                FirstName = update.Message.Text.Split(' ')[0],
                                LastName = update.Message.Text.Split(' ')[1]
                            };
                            await _context.Users.AddAsync(user);
                            await _context.SaveChangesAsync();
                            await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, $"{update.Message.Text} зарегистрирован");
                            //_fsmcontext.TransitionTo(new RegistrationState(_context, _userService, _botClient));
                        }
                        break;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
