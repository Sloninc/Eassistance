using Eassistance.Controllers.Commands;
using Telegram.Bot.Types;

namespace Eassistance.Controllers
{
    public class CommandExecutor : ITelegramUpdateListener
    {
        private List<ICommand> commands;

        public CommandExecutor()
        {
            commands = new List<ICommand>();
            {
                new StartCommand();
            }
        }

        public async Task GetUpdate(Update update)
        {
            Message msg = update.Message;
            if (msg.Text == null)
                return;

            foreach (var command in commands)
            {
                if (command.Name == msg.Text)
                {
                    await command.Execute(update);
                }
            }
        }
    }
}
