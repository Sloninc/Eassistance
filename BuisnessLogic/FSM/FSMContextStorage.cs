using Telegram.Bot.Types;

namespace Eassistance.BuisnessLogic.FSM
{
    public static class FSMContextStorage
    {
        private static Dictionary<ChatId,FSMContext> _storage = new Dictionary<ChatId,FSMContext>();
        public static void Set(ChatId chatId, FSMContext context)
        {
            if(_storage.ContainsKey(chatId))
                _storage[chatId] = context;
            else
                _storage.Add(chatId, context); 
        }
        public static FSMContext Get(ChatId chatId)
        {
            if (_storage.ContainsKey(chatId))
                return _storage[chatId];
            else
                return null;
        }
    }
}
