using Eassistance.Controllers.Commands;
using Telegram.Bot.Types;

namespace Eassistance.Controllers
{
    public class CommandExecutor : ITelegramUpdateListener
    {
        private List<ICommand> commands;
        private IListener? listener = null;
        public CommandExecutor()
        {
            commands = new List<ICommand>();
            {
                new StartCommand();
            }
        }
        public async Task GetUpdate(Update update)
        {
            if (listener == null)
            {
                await ExecuteCommand(update);
            }
            else
            {
                await listener.GetUpdate(update);
            }
        }
        private async Task ExecuteCommand(Update update)
        {
            Message msg = update.Message;
            foreach (var command in commands)
            {
                if (command.Name == msg.Text)
                {
                    await command.Execute(update);
                }
            }
        }
        public void StartListen(IListener newListener)
        {
            listener = newListener;
        }
        public void StopListen()
        {
            listener = null;
        }
    }
}
