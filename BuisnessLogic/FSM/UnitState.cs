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
    public class UnitState: BaseState
    {
        public UnitState(IUserService userService, TelegramBot telegramBot, IUnitService unitService, IOperationService operationService, IEquipmentService equipmentService, IStepService stepService) : base(userService, telegramBot, unitService, operationService, equipmentService, stepService)
        {
            Name = "Units";
        }
        public override string Name { get; }
        bool _isAddUnit = false;
        List<Unit> _units = null;
        bool _isRemovedUnit = false;
        Unit _removedUnit = null;
        bool _isOperationCreate= false;
        bool _isOperationDelete= false;
        Unit _addedUnit = null;
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
                    case "Список узлов":
                        _units = await _unitService.GetAllUnits();
                        if (_units.Count==0)
                        {
                            var inlineKeyboard = new ReplyKeyboardMarkup(new[] { new[] { new KeyboardButton("OK") } });
                            inlineKeyboard.ResizeKeyboard = false;
                            _fsmcontext.TransitionTo(new StartState(_userService, _botClient, _unitService, _operationService, _equipmentService, _stepService));
                            FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                            await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, "список узлов пуст", replyMarkup: inlineKeyboard);
                        }
                        else
                        {
                            KeyboardButton[][] keyboardButtons = new KeyboardButton[_units.Count][];
                            for (int i = 0; i < _units.Count; i++)
                                keyboardButtons[i] = new KeyboardButton[1] { new KeyboardButton(_units[i].Name) };
                            _fsmcontext.TransitionTo(new EquipmentChoiceState(_userService, _botClient, _unitService, _operationService, _equipmentService, _stepService));
                            FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                            var inlineKeyboard = new ReplyKeyboardMarkup(keyboardButtons);
                            const string message = "Выберите узел из списка";
                            await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, message, replyMarkup: inlineKeyboard);
                        }
                        break;
                    case "Добавить узел":
                        _isOperationCreate = true;
                        await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, "Введите название узла");
                        break;
                    case "Удалить узел":
                        _isOperationDelete = true;
                        _units = await _unitService.GetAllUnits();
                        KeyboardButton[][] keyboardButtonsDelete = new KeyboardButton[_units.Count][];
                        for (int i = 0; i < _units.Count; i++)
                            keyboardButtonsDelete[i] = new KeyboardButton[1] { new KeyboardButton(_units[i].Name) };
                        var inlineKeyboardDelete = new ReplyKeyboardMarkup(keyboardButtonsDelete);
                        const string messageDelete = "Выберите узел из списка";
                        await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, messageDelete, replyMarkup: inlineKeyboardDelete);
                        break;
                    default:
                        if (_isOperationCreate)
                        {
                            _addedUnit = new Unit() { Name = update.Message.Text };
                            _isAddUnit = await _unitService.CreateUnit(_addedUnit);
                            var inlineKeyboard = new ReplyKeyboardMarkup(new[] { new[] { new KeyboardButton("OK") } });
                            inlineKeyboard.ResizeKeyboard = true;
                            if (_isAddUnit)
                            {
                                _fsmcontext.TransitionTo(new StartState(_userService, _botClient, _unitService, _operationService, _equipmentService, _stepService));
                                FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                                await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, $"{_addedUnit.Name} добавлен", replyMarkup: inlineKeyboard);
                            }
                            else
                            {
                                _fsmcontext.TransitionTo(new StartState(_userService, _botClient, _unitService, _operationService, _equipmentService, _stepService));
                                FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                                await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, $"{_addedUnit.Name} не удалось добавить", replyMarkup: inlineKeyboard);
                            }
                            _addedUnit = null;
                            _isAddUnit = false;
                            _isOperationCreate = false;
                        }
                        if (_isOperationDelete)
                        {
                            var inlineKeyboard = new ReplyKeyboardMarkup(new[] { new[] { new KeyboardButton("OK") } });
                            inlineKeyboard.ResizeKeyboard = true;
                            _removedUnit = await _unitService.GetUnitByName(update.Message.Text);
                            if (_removedUnit != null)
                            {
                                _isRemovedUnit = await _unitService.DeleteUnit(_removedUnit);
                                if (_isRemovedUnit)
                                {
                                    _fsmcontext.TransitionTo(new StartState(_userService, _botClient, _unitService, _operationService, _equipmentService, _stepService));
                                    FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                                    await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, $"{_removedUnit.Name} удален", replyMarkup: inlineKeyboard);
                                }
                                else
                                {
                                    _fsmcontext.TransitionTo(new StartState(_userService, _botClient, _unitService, _operationService, _equipmentService, _stepService));
                                    FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                                    await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, $"{_removedUnit.Name} не удалось удалить", replyMarkup: inlineKeyboard);
                                }
                            }
                            _isOperationDelete = false;
                            _isRemovedUnit = false;
                            _removedUnit = null;
                            _fsmcontext.TransitionTo(new StartState(_userService, _botClient, _unitService, _operationService, _equipmentService, _stepService));
                            FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
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
