using Telegram.Bot;

namespace Eassistance
{
    public class Bot
    {
        private static TelegramBotClient client { get; set; }
        public static TelegramBotClient GetTelegramBot()
        {
            if (client != null)
            {
                return client;
            }
            client = new TelegramBotClient("6255156472:AAEigi9Xd7avGjd2cuJPCfmSETLwrGQLRRo");
            return client;
        }
    }
}
