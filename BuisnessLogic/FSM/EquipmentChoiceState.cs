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
        public EquipmentChoiceState(IDbContextFactory<DataContext> contextFactory, IUserService userService, TelegramBot telegramBot, IUnitService unitService, IOperationService operationService, IEquipmentService equipmentService, IStepService stepService) : base(contextFactory, userService, telegramBot, unitService, operationService, equipmentService, stepService)
        {
            Name = "EqipmentChoice";
        }
        public override string Name { get; }
 
        public async override Task Handle(Update update)
        {
            try
            {

                Unit unit = null;
                using (var _context = _contextFactory.CreateDbContext())
                {
                    unit = await _context.Units.FirstOrDefaultAsync(x => x.Name == update.Message.Text);
                }
                if (unit == null)
                {
                    _fsmcontext.TransitionTo(new UnitState(_contextFactory, _userService, _botClient, _unitService, _operationService, _equipmentService, _stepService));
                    FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                    _fsmcontext.Request(update);
                }
                else
                {
                    _fsmcontext.TransitionTo(new EquipmentState(_contextFactory, _userService, _botClient, _unitService, _operationService, _equipmentService, _stepService, unit));
                    FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                    KeyboardButton[][] keyboardButtons = new KeyboardButton[3][];
                    keyboardButtons[0] = new KeyboardButton[1] { new KeyboardButton($"Список оборудования") };
                    keyboardButtons[1] = new KeyboardButton[1] { new KeyboardButton($"Добавить оборудование") };
                    keyboardButtons[2] = new KeyboardButton[1] { new KeyboardButton($"Удалить оборудования") };
                    var inlineKeyboard = new ReplyKeyboardMarkup(keyboardButtons);
                    inlineKeyboard.ResizeKeyboard = true;
                    await _botClient.GetBot().Result.SendTextMessageAsync(update.Message.Chat.Id, $"Узел связи {unit.Name}", replyMarkup: inlineKeyboard);;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
