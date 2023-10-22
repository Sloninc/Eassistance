using Eassistance.Infrastructure;
using Eassistance.Domain;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Eassistance.Services;
using Eassistance.Services.Abstract;
using Telegram.Bot;

namespace Eassistance.BuisnessLogic.FSM
{
    public class StartState : BaseState
    {
        public StartState(IUserService userService, TelegramBot telegramBot, IUnitService unitService, IOperationService operationService, IEquipmentService equipmentService, IStepService stepService) : base(userService, telegramBot, unitService, operationService, equipmentService, stepService)
        {
            Name = "start";
        }
        public override string Name { get; }
        public async override Task Handle(Update update)
        {
            EAUser user = null;
            user = await _userService.GetUserByChatId(update.Message.Chat.Id);
            if (user == null)
            {
                _fsmcontext.TransitionTo(new RegistrationState(_userService, _botClient, _unitService,_operationService,_equipmentService,_stepService));
                FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                _fsmcontext.Request(update);
            }
            else
            {
                _fsmcontext.TransitionTo(new UnitState(_userService, _botClient, _unitService, _operationService, _equipmentService, _stepService));
                FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                var inlineKeyboard = GetTrheeKeyboard("Список узлов", "Добавить узел", "Удалить узел");
                await _botClient.GetBot().Result.SendTextMessageAsync(user.ChatId, "Выберите действие", replyMarkup: inlineKeyboard);
            }
        }
    }
}
