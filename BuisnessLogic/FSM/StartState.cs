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
        public StartState(IDbContextFactory<DataContext> contextFactory, IUserService userService, TelegramBot telegramBot, IUnitService unitService, IOperationService operationService, IEquipmentService equipmentService, IStepService stepService) : base(contextFactory, userService, telegramBot, unitService, operationService, equipmentService, stepService)
        {
            Name = "start";
        }
        public override string Name { get; }
        public async override Task Handle(Update update)
        {
            EAUser user = null;
            using (var _context = _contextFactory.CreateDbContext())
            {
                user = await _context.Users.FirstOrDefaultAsync(x => x.ChatId == update.Message.Chat.Id);
            }
            if (user == null)
            {
                _fsmcontext.TransitionTo(new RegistrationState(_contextFactory, _userService, _botClient, _unitService,_operationService,_equipmentService,_stepService));
                _fsmcontext.Request(update);
            }
            else
            {
                //_fsmcontext.TransitionTo(new RegistrationState(_context, _userService, _botClient));
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
