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
    public class StepRunState : BaseState
    {
        public StepRunState(IUserService userService, TelegramBot telegramBot, IUnitService unitService, IOperationService operationService, IEquipmentService equipmentService, IStepService stepService, Operation operation) : base(userService, telegramBot, unitService, operationService, equipmentService, stepService)
        {
            Name = "StepRun";
            _operation = operation;
        }
        public override string Name { get; }
        private Operation _operation;
        List<Step> _steps = null;
        Step _currentStep = null;
        public async override Task Handle(Update update)
        {
            try
            {
                var inlineKeyboard = new ReplyKeyboardMarkup(new[] { 
                    new[] { new KeyboardButton("Предыдущий шаг операции"), new KeyboardButton("Следующий шаг операции") },
                    new[] { new KeyboardButton("Вернуться к списку шагов операции")} });
                inlineKeyboard.ResizeKeyboard = true;
                _steps = await _stepService.GetAllSteps(_operation);
                switch (update.Message.Text)
                {
                    case "/start":
                        _fsmcontext.TransitionTo(new StartState(_userService, _botClient, _unitService, _operationService, _equipmentService, _stepService));
                        FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                        _fsmcontext.Request(update);
                        break;
                    case "Вернуться к списку шагов операции":
                        _fsmcontext.TransitionTo(new StepState(_userService, _botClient, _unitService, _operationService, _equipmentService, _stepService, _operation));
                        FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                        update.Message.Text = "Проведение операции";
                        _fsmcontext.Request(update);
                        break;
                    case "Предыдущий шаг операции":
                        _fsmcontext.TransitionTo(this);
                        FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                        if (_currentStep.SerialNumber > 1)
                        {
                            _currentStep = _steps[_currentStep.SerialNumber - 2];
                            await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, _currentStep.Body, replyMarkup: inlineKeyboard);
                        }
                        else
                        {
                            await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, _currentStep.Body, replyMarkup: inlineKeyboard);
                        }
                        break;
                    case "Следующий шаг операции":
                        _fsmcontext.TransitionTo(this);
                        FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                        if (_currentStep.SerialNumber < _steps.Count)
                        {
                            _currentStep = _steps[_currentStep.SerialNumber];
                            await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, _currentStep.Body, replyMarkup: inlineKeyboard);
                        }
                        else
                        {
                            await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, _currentStep.Body, replyMarkup: inlineKeyboard);
                        }
                        break;
                    default:
                        _fsmcontext.TransitionTo(this);
                        FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                        _currentStep = _steps.First(x => x.Name == update.Message.Text);
                        await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, _currentStep.Body, replyMarkup: inlineKeyboard);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
