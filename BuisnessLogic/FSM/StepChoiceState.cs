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
    public class StepChoiceState : BaseState
    {
        Operation _currentOperation;
        public StepChoiceState(IUserService userService, TelegramBot telegramBot, IUnitService unitService, IOperationService operationService, IEquipmentService equipmentService, IStepService stepService, Operation currentOperation = null) : base(userService, telegramBot, unitService, operationService, equipmentService, stepService)
        {
            Name = "StepChoice";
            _currentOperation = currentOperation;
        }
        public override string Name { get; }

        public async override Task Handle(Update update)
        {
            try
            {
                Operation operation = null;
                var inlineKeyboard = GetTrheeKeyboard("Проведение операции", "Добавить шаг операции", "Удалить шаг операции");
                if (update.Message.Text == "OK")
                {
                    operation = _currentOperation;
                    _fsmcontext.TransitionTo(new StepState(_userService, _botClient, _unitService, _operationService, _equipmentService, _stepService, operation));
                    FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                    await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, $"Операция {operation.Name}", replyMarkup: inlineKeyboard);
                }
                else if (update.Message.Text == "/start")
                {
                    _fsmcontext.TransitionTo(new StartState(_userService, _botClient, _unitService, _operationService, _equipmentService, _stepService));
                    FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                    _fsmcontext.Request(update);
                }
                else
                {
                    operation = await _operationService.GetOperationByName(update.Message.Text);
                    _fsmcontext.TransitionTo(new StepState(_userService, _botClient, _unitService, _operationService, _equipmentService, _stepService, operation));
                    FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                    await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, $"Операция {operation.Name}", replyMarkup: inlineKeyboard);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

