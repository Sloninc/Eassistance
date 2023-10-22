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
    public class EquipmentState:BaseState
    {
        public EquipmentState(IUserService userService, TelegramBot telegramBot, IUnitService unitService, IOperationService operationService, IEquipmentService equipmentService, IStepService stepService, Unit unit) : base(userService, telegramBot, unitService, operationService, equipmentService, stepService)
        {
            Name = "Equipments";
            _unit = unit;
        }
        public override string Name { get; }
        private Unit _unit;

        bool _isAddEquipment = false;
        List<Equipment> _equipments = null;
        bool _isRemovedEquipment = false;
        Equipment _removedEquipment = null;
        bool _isOperationCreate = false;
        bool _isOperationDelete = false;
        Equipment _addedEquipment = null;
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
                    case "Список оборудования":
                        _equipments = await _equipmentService.GetAllEquipments(_unit);
                        if (_equipments.Count == 0)
                        {
                            var inlineKeyboard = new ReplyKeyboardMarkup(new[] { new[] { new KeyboardButton("OK") } });
                            inlineKeyboard.ResizeKeyboard = false;
                            _fsmcontext.TransitionTo(new EquipmentChoiceState(_userService, _botClient, _unitService, _operationService, _equipmentService, _stepService, _unit));
                            FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                            await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, $"список оборудования узла {_unit.Name} пуст", replyMarkup: inlineKeyboard);;
                        }
                        else
                        {
                            KeyboardButton[][] keyboardButtons = new KeyboardButton[_equipments.Count][];
                            for (int i = 0; i < _equipments.Count; i++)
                                keyboardButtons[i] = new KeyboardButton[1] { new KeyboardButton(_equipments[i].Name) };
                            _fsmcontext.TransitionTo(new OperationChoiceState(_userService, _botClient, _unitService, _operationService, _equipmentService, _stepService));
                            FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                            var inlineKeyboard = new ReplyKeyboardMarkup(keyboardButtons);
                            const string message = "Выберите оборудование узла из списка";
                            await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, message, replyMarkup: inlineKeyboard);
                        }
                        break;
                    case "Добавить оборудование":
                        _isOperationCreate = true;
                        _fsmcontext.TransitionTo(this);
                        FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                        await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, "Введите название оборудования и через @ его заводской номер. Например \"DW7000@34FV67CBV67B\"");
                        break;
                    case "Удалить оборудование":
                        _isOperationDelete = true;
                        _equipments = await _equipmentService.GetAllEquipments(_unit);
                        if (_equipments.Count == 0)
                        {
                            var inlineKeyboard = new ReplyKeyboardMarkup(new[] { new[] { new KeyboardButton("OK") } });
                            inlineKeyboard.ResizeKeyboard = false;
                            _fsmcontext.TransitionTo(new EquipmentChoiceState(_userService, _botClient, _unitService, _operationService, _equipmentService, _stepService, _unit));
                            FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                            await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, $"список оборудования узла {_unit.Name} пуст", replyMarkup: inlineKeyboard); ;
                        }
                        else
                        {
                            KeyboardButton[][] keyboardButtonsDelete = new KeyboardButton[_equipments.Count][];
                            for (int i = 0; i < _equipments.Count; i++)
                                keyboardButtonsDelete[i] = new KeyboardButton[1] { new KeyboardButton(_equipments[i].Name) };
                            var inlineKeyboardDelete = new ReplyKeyboardMarkup(keyboardButtonsDelete);
                            _fsmcontext.TransitionTo(this);
                            FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                            const string messageDelete = "Выберите оборудование из списка для удаления";
                            await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, messageDelete, replyMarkup: inlineKeyboardDelete);
                        }
                        break;
                    default:
                        if (_isOperationCreate)
                        {
                            string[] newEqipment = update.Message.Text.Split('@');
                            _addedEquipment = new Equipment { Name = newEqipment[0], SerialNumber = newEqipment[1], UnitId = _unit.Id };
                            _isAddEquipment = await _equipmentService.CreateEquipment(_addedEquipment);
                            var inlineKeyboard = new ReplyKeyboardMarkup(new[] { new[] { new KeyboardButton("OK") } });
                            if (_isAddEquipment)
                            {
                                _fsmcontext.TransitionTo(new EquipmentChoiceState(_userService, _botClient, _unitService, _operationService, _equipmentService, _stepService, _unit));
                                FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                                await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, $"{_addedEquipment.Name} добавлен", replyMarkup: inlineKeyboard);
                            }
                            else
                            {
                                _fsmcontext.TransitionTo(new EquipmentChoiceState(_userService, _botClient, _unitService, _operationService, _equipmentService, _stepService));
                                FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                                await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, $"{_addedEquipment.Name} не удалось добавить", replyMarkup: inlineKeyboard);
                            }
                        }
                        if (_isOperationDelete)
                        {
                            var inlineKeyboard = new ReplyKeyboardMarkup(new[] { new[] { new KeyboardButton("OK") } });
                            inlineKeyboard.ResizeKeyboard = true;
                            _removedEquipment = await _equipmentService.GetEquipmentByName(update.Message.Text);
                            if (_removedEquipment != null)
                            {
                                _isRemovedEquipment = await _equipmentService.DeleteEquipment(_removedEquipment);
                                if (_isRemovedEquipment)
                                {
                                    _fsmcontext.TransitionTo(new EquipmentChoiceState(_userService, _botClient, _unitService, _operationService, _equipmentService, _stepService, _unit));
                                    FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                                    await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, $"{_removedEquipment.Name} удален", replyMarkup: inlineKeyboard);
                                }
                                else
                                {
                                    _fsmcontext.TransitionTo(new EquipmentChoiceState(_userService, _botClient, _unitService, _operationService, _equipmentService, _stepService, _unit));
                                    FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                                    await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, $"{_removedEquipment.Name} не удалось удалить", replyMarkup: inlineKeyboard);
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
