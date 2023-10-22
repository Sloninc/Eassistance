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
    public class OperationChoiceState : BaseState
    {
        Equipment _currentEquipment;
        public OperationChoiceState(IUserService userService, TelegramBot telegramBot, IUnitService unitService, IOperationService operationService, IEquipmentService equipmentService, IStepService stepService, Equipment currentEquipment = null) : base(userService, telegramBot, unitService, operationService, equipmentService, stepService)
        {
            Name = "OperationChoice";
            _currentEquipment = currentEquipment;
        }
        public override string Name { get; }

        public async override Task Handle(Update update)
        {
            try
            {
                Equipment equipment = await _equipmentService.GetEquipmentByName(update.Message.Text);
                var inlineKeyboard = GetTrheeKeyboard("Список операций", "Добавить операцию", "Удалить операцию");
                if (update.Message.Text == "OK")
                {
                    equipment = _currentEquipment;
                    _fsmcontext.TransitionTo(new OperationState(_userService, _botClient, _unitService, _operationService, _equipmentService, _stepService, equipment));
                    FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                    await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, $"Оборудование {equipment.Name}", replyMarkup: inlineKeyboard);
                }
                else if (update.Message.Text == "/start")
                {
                    _fsmcontext.TransitionTo(new StartState(_userService, _botClient, _unitService, _operationService, _equipmentService, _stepService));
                    FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                    _fsmcontext.Request(update);
                }
                else
                {
                    _fsmcontext.TransitionTo(new OperationState(_userService, _botClient, _unitService, _operationService, _equipmentService, _stepService, equipment));
                    FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                    await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, $"оборудование {equipment.Name}", replyMarkup: inlineKeyboard);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
