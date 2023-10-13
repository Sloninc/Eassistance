using Telegram.Bot;
using Telegram.Bot.Types;

namespace Eassistance.Controllers.Commands
{
    //public class RegisterCommand : ICommand, IListener
    //{
    //    public TelegramBotClient Client => Bot.GetTelegramBot();
    //    public string Name => "Регистрация";
    //    public CommandExecutor Executor { get; }
    //    public RegisterCommand(CommandExecutor executor)
    //    {
    //        Executor = executor;
    //    }
    //    private string? phone = null;
    //    private string? name = null;
    //    public async Task Execute(Update update)
    //    {
    //        long chatId = update.Message.Chat.Id;
    //        Executor.StartListen(this); //говорим, что теперь нам надо отправлять апдейты
    //        await Client.SendTextMessageAsync(chatId, "Введите номер!");
    //    }
    //    public async Task GetUpdate(Update update)
    //    {
    //        long chatId = update.Message.Chat.Id;
    //        if (update.Message.Text == null) 
    //            return;

    //        if (phone == null) //Получаем номер, просим имя
    //        {
    //            phone = update.Message.Text;
    //            await Client.SendTextMessageAsync(chatId, "Введите имя!");
    //        }
    //        else //Получаем имя, говорим, что больше нам апдейты не нужны
    //        {
    //            name = update.Message.Text;
    //            await Client.SendTextMessageAsync(chatId, "Поздравляем с регистарцией!");
    //            Executor.StopListen();
    //        }
    //    }
    //}
}
