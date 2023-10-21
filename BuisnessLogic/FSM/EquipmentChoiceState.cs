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
    public class EquipmentChoiceState:BaseState
    {
        Unit _currentUnit;
        public EquipmentChoiceState(IDbContextFactory<DataContext> contextFactory, IUserService userService, TelegramBot telegramBot, IUnitService unitService, IOperationService operationService, IEquipmentService equipmentService, IStepService stepService, Unit currentunit=null) : base(contextFactory, userService, telegramBot, unitService, operationService, equipmentService, stepService)
        {
            Name = "EqipmentChoice";
            _currentUnit = currentunit;
        }
        public override string Name { get; }
 
        public async override Task Handle(Update update)
        {
            try
            {
                Unit unit = null;
                var inlineKeyboard = GetTrheeKeyboard("Список оборудования", "Добавить оборудование", "Удалить оборудование");
                if (update.Message.Text == "OK")
                {
                    unit = _currentUnit;
                    _fsmcontext.TransitionTo(new EquipmentState(_contextFactory, _userService, _botClient, _unitService, _operationService, _equipmentService, _stepService, unit));
                    FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                    await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, $"Узел связи {unit.Name}", replyMarkup: inlineKeyboard);
                }
                else
                {
                    unit = await _unitService.GetUnitByName(update.Message.Text);
                    _fsmcontext.TransitionTo(new EquipmentState(_contextFactory, _userService, _botClient, _unitService, _operationService, _equipmentService, _stepService, unit));
                    FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                    await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, $"Узел связи {unit.Name}", replyMarkup: inlineKeyboard);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
