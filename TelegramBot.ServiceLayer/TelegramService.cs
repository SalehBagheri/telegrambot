using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.ServiceLayer.Telegram;

namespace TelegramBot.ServiceLayer
{
    public class TelegramService
    {
        public TelegramService()
        {
            PublicMessages = new PublicState();
            PrivateMessages = new PrivateState();
        }

        public PublicState PublicMessages { get; set; }
        public PrivateState PrivateMessages { get; set; }
    }
}
