﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBot.ServiceLayer.Telegram
{
    public static class MyBot
    {
        /// <summary>
        /// توکن ربات خود را اینجا وارد نمایید
        /// </summary>
        public static readonly TelegramBotClient Api = new TelegramBotClient("Enter your token key here");
    }
}
