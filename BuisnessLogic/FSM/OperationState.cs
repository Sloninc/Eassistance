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
    public class OperationState : BaseState
    {
        public OperationState(IUserService userService, TelegramBot telegramBot, IUnitService unitService, IOperationService operationService, IEquipmentService equipmentService, IStepService stepService, Equipment equipment) : base(userService, telegramBot, unitService, operationService, equipmentService, stepService)
        {
            Name = "Operation";
            _equipment = equipment;
        }
        public override string Name { get; }
        private Equipment _equipment;

        bool _isAddOperation = false;
        List<Operation> _operations = null;
        bool _isRemovedOperation = false;
        Operation _removedOperation = null;
        bool _isOperationCreate = false;
        bool _isOperationDelete = false;
        Operation _addedOperation = null;
        public async override Task Handle(Update update)
        {
            try
            {
                switch (update.Message.Text)
                {
                    case "/start":
                        _fsmcontext.TransitionTo(new StartState(_userService, _botClient, _unitService, _operationService, _equipmentService, _stepService));
                        FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                        _fsmcontext.Request(update);
                        break;
                    case "Список операций":
                        _operations = await _operationService.GetAllOperations(_equipment);
                        if (_operations.Count == 0)
                        {
                            var inlineKeyboard = new ReplyKeyboardMarkup(new[] { new[] { new KeyboardButton("OK") } });
                            inlineKeyboard.ResizeKeyboard = true;
                            _fsmcontext.TransitionTo(new OperationChoiceState(_userService, _botClient, _unitService, _operationService, _equipmentService, _stepService, _equipment));
                            FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                            await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, $"список операций оборудования {_equipment.Name} пуст", replyMarkup: inlineKeyboard); ;
                        }
                        else
                        {
                            KeyboardButton[][] keyboardButtons = new KeyboardButton[_operations.Count][];
                            for (int i = 0; i < _operations.Count; i++)
                                keyboardButtons[i] = new KeyboardButton[1] { new KeyboardButton(_operations[i].Name) };
                            _fsmcontext.TransitionTo(new StepChoiceState(_userService, _botClient, _unitService, _operationService, _equipmentService, _stepService));
                            FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                            var inlineKeyboard = new ReplyKeyboardMarkup(keyboardButtons);
                            string message = $"Выберите тип операции на оборудовании {_equipment.Name}";
                            await _botClient.GetBot().Result.SendTextMessageAsync(  update.Message.Chat.Id, message, replyMarkup: inlineKeyboard);
                        }
                        break;
                    case "Добавить операцию":
                        _isOperationCreate = true;
                        _fsmcontext.TransitionTo(this);
                        FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                        await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, "Введите наименование операции");
                        break;
                    case "Удалить операцию":
                        _isOperationDelete = true;
                        _operations = await _operationService.GetAllOperations(_equipment);
                        if (_operations.Count == 0)
                        {
                            var inlineKeyboard = new ReplyKeyboardMarkup(new[] { new[] { new KeyboardButton("OK") } });
                            inlineKeyboard.ResizeKeyboard = false;
                            _fsmcontext.TransitionTo(new OperationChoiceState(_userService, _botClient, _unitService, _operationService, _equipmentService, _stepService, _equipment));
                            FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                            await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, $"список оперций на оборудовании {_equipment.Name} пуст", replyMarkup: inlineKeyboard); ;
                        }
                        else
                        {
                            KeyboardButton[][] keyboardButtonsDelete = new KeyboardButton[_operations.Count][];
                            for (int i = 0; i < _operations.Count; i++)
                                keyboardButtonsDelete[i] = new KeyboardButton[1] { new KeyboardButton(_operations[i].Name) };
                            var inlineKeyboardDelete = new ReplyKeyboardMarkup(keyboardButtonsDelete);
                            _fsmcontext.TransitionTo(this);
                            FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                            const string messageDelete = "Выберите операцию из списка для удаления";
                            await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, messageDelete, replyMarkup: inlineKeyboardDelete);
                        }
                        break;
                    default:
                        if (_isOperationCreate)
                        {
                            _addedOperation = new Operation { Name = update.Message.Text, EquipmentId = _equipment.Id };
                            _isAddOperation = await _operationService.CreateOperation(_addedOperation);
                            var inlineKeyboard = new ReplyKeyboardMarkup(new[] { new[] { new KeyboardButton("OK") } });
                            if (_isAddOperation)
                            {
                                _fsmcontext.TransitionTo(new OperationChoiceState(_userService, _botClient, _unitService, _operationService, _equipmentService, _stepService, _equipment));
                                FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                                await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, $"{_addedOperation.Name} добавлен", replyMarkup: inlineKeyboard);
                            }
                            else
                            {
                                _fsmcontext.TransitionTo(new OperationChoiceState(_userService, _botClient, _unitService, _operationService, _equipmentService, _stepService));
                                FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                                await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, $"{_addedOperation.Name} не удалось добавить", replyMarkup: inlineKeyboard);
                            }
                        }
                        if (_isOperationDelete)
                        {
                            var inlineKeyboard = new ReplyKeyboardMarkup(new[] { new[] { new KeyboardButton("OK") } });
                            inlineKeyboard.ResizeKeyboard = true;
                            _removedOperation = await _operationService.GetOperationByName(update.Message.Text);
                            if (_removedOperation != null)
                            {
                                _isRemovedOperation = await _operationService.DeleteOperation(_removedOperation);
                                if (_isRemovedOperation)
                                {
                                    _fsmcontext.TransitionTo(new OperationChoiceState(_userService, _botClient, _unitService, _operationService, _equipmentService, _stepService, _equipment));
                                    FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                                    await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, $"{_removedOperation.Name} удален", replyMarkup: inlineKeyboard);
                                }
                                else
                                {
                                    _fsmcontext.TransitionTo(new OperationChoiceState(_userService, _botClient, _unitService, _operationService, _equipmentService, _stepService, _equipment));
                                    FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                                    await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, $"{_removedOperation.Name} не удалось удалить", replyMarkup: inlineKeyboard);
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

