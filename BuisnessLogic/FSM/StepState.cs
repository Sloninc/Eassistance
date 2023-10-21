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
    public class StepState : BaseState
    {
        public StepState(IDbContextFactory<DataContext> contextFactory, IUserService userService, TelegramBot telegramBot, IUnitService unitService, IOperationService operationService, IEquipmentService equipmentService, IStepService stepService, Operation operation) : base(contextFactory, userService, telegramBot, unitService, operationService, equipmentService, stepService)
        {
            Name = "Step";
            _operation = operation;
        }
        public override string Name { get; }
        private Operation _operation;

        bool _isAddStep = false;
        List<Step> _steps = null;
        bool _isRemovedStep = false;
        Step _removedStep = null;
        bool _isOperationCreate = false;
        bool _isOperationDelete = false;
        Step _addedStep = null;
        bool _isReadingBodySteps = false;
        bool _isbody = false;
        string _body = null;

        public async override Task Handle(Update update)
        {
            try
            {
                switch (update.Message.Text)
                {
                    case "/start":
                        _fsmcontext.TransitionTo(new StartState(_contextFactory, _userService, _botClient, _unitService, _operationService, _equipmentService, _stepService));
                        FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                        _fsmcontext.Request(update);
                        break;
                    case "Проведение операции":
                        _steps = await _stepService.GetAllSteps(_operation);
                        if (_steps.Count == 0)
                        {
                            var inlineKeyboard = new ReplyKeyboardMarkup(new[] { new[] { new KeyboardButton("OK") } });
                            inlineKeyboard.ResizeKeyboard = true;
                            _fsmcontext.TransitionTo(new StepChoiceState(_contextFactory, _userService, _botClient, _unitService, _operationService, _equipmentService, _stepService, _operation));
                            FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                            await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, $"шаги операций {_operation.Name} отсутствуют", replyMarkup: inlineKeyboard); ;
                        }
                        else
                        {
                            KeyboardButton[][] keyboardButtons = new KeyboardButton[_steps.Count][];
                            for (int i = 0; i < _steps.Count; i++)
                                keyboardButtons[i] = new KeyboardButton[1] { new KeyboardButton(_steps[i].Name) };
                            _fsmcontext.TransitionTo(new StepRunState(_contextFactory, _userService, _botClient, _unitService, _operationService, _equipmentService, _stepService, _operation));
                            FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                            var inlineKeyboard = new ReplyKeyboardMarkup(keyboardButtons);
                            string message = $"Выберите шаг операции {_operation.Name}";
                            await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, message, replyMarkup: inlineKeyboard);
                        }
                        break;
                    case "Добавить шаг операции":
                        _isOperationCreate = true;
                        _fsmcontext.TransitionTo(this);
                        FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                        await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, "Введите наименование шага операции и его порядковый номер через @. Например: \"Настройка TFTP@5\"");
                        break;
                    case "Удалить шаг операции":
                        _isOperationDelete = true;
                        _steps = await _stepService.GetAllSteps(_operation);
                        if (_steps.Count == 0)
                        {
                            var inlineKeyboard = new ReplyKeyboardMarkup(new[] { new[] { new KeyboardButton("OK") } });
                            inlineKeyboard.ResizeKeyboard = false;
                            _fsmcontext.TransitionTo(new StepChoiceState(_contextFactory, _userService, _botClient, _unitService, _operationService, _equipmentService, _stepService, _operation));
                            FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                            await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, $"список шагов оперции {_operation.Name} пуст", replyMarkup: inlineKeyboard); ;
                        }
                        else
                        {
                            KeyboardButton[][] keyboardButtonsDelete = new KeyboardButton[_steps.Count][];
                            for (int i = 0; i < _steps.Count; i++)
                                keyboardButtonsDelete[i] = new KeyboardButton[1] { new KeyboardButton(_steps[i].Name) };
                            var inlineKeyboardDelete = new ReplyKeyboardMarkup(keyboardButtonsDelete);
                            _fsmcontext.TransitionTo(this);
                            FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                            const string messageDelete = "Выберите шаг операции из списка для удаления";
                            await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, messageDelete, replyMarkup: inlineKeyboardDelete);
                        }
                        break;
                    default:
                        if (_isOperationCreate&_isbody)
                        {
                            _addedStep.Body = $"Шаг#{_addedStep.SerialNumber}. {_addedStep.Name}\n {update.Message.Text}";
                            _isAddStep = await _stepService.CreateStep(_addedStep);
                            var inlineKeyboard = new ReplyKeyboardMarkup(new[] { new[] { new KeyboardButton("OK") } });
                            if (_isAddStep)
                            {
                                _fsmcontext.TransitionTo(new StepChoiceState(_contextFactory, _userService, _botClient, _unitService, _operationService, _equipmentService, _stepService, _operation));
                                FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                                await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, $"шаг операции {_addedStep.Name} добавлен", replyMarkup: inlineKeyboard);
                            }
                            else
                            {
                                _fsmcontext.TransitionTo(new StepChoiceState(_contextFactory, _userService, _botClient, _unitService, _operationService, _equipmentService, _stepService, _operation));
                                FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                                await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, $"{_addedStep.Name} не удалось добавить", replyMarkup: inlineKeyboard);
                            }
                        }
                        if (_isOperationCreate & !_isbody)
                        {
                            string[] newStep = update.Message.Text.Split('@');
                            _addedStep = new Step { Name = newStep[0], SerialNumber = int.Parse(newStep[1]), OperationId = _operation.Id };
                            _fsmcontext.TransitionTo(this);
                            _isbody = true;
                            FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                            await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, $"Поместите инструкцию в шаг: {_addedStep.Name}");
                        }
                        if (_isOperationDelete)
                        {
                            var inlineKeyboard = new ReplyKeyboardMarkup(new[] { new[] { new KeyboardButton("OK") } });
                            inlineKeyboard.ResizeKeyboard = true;
                            _removedStep = await _stepService.GetStepByName(update.Message.Text);
                            if (_removedStep != null)
                            {
                                _isRemovedStep = await _stepService.DeleteStep(_removedStep);
                                if (_isRemovedStep)
                                {
                                    _fsmcontext.TransitionTo(new StepChoiceState(_contextFactory, _userService, _botClient, _unitService, _operationService, _equipmentService, _stepService, _operation));
                                    FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                                    await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, $"шаг операции {_removedStep.Name} удален", replyMarkup: inlineKeyboard);
                                }
                                else
                                {
                                    _fsmcontext.TransitionTo(new StepChoiceState(_contextFactory, _userService, _botClient, _unitService, _operationService, _equipmentService, _stepService, _operation));
                                    FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                                    await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, $"{_removedStep.Name} не удалось удалить", replyMarkup: inlineKeyboard);
                                }
                            }
                        }
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


