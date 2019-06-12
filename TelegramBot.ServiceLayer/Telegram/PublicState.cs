using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Models;

namespace TelegramBot.ServiceLayer.Telegram
{
    public class PublicState
    {
        private readonly UserService _userService;
        private readonly ViewStateService _vsService;
        public PublicState()
        {
            _userService = new UserService();
            _vsService = new ViewStateService();
        }


        /// <summary>
        /// برای اولین بار یک تصویر تبلیغاتی به کاربر ارسال می‌شود
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<Message> FirstMessage(Message message)
        {
            await MyBot.Api.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);

            var PromptMessage = "به ربات پیشرفته تلگرام بات خوش آمدید!";

            string file = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\img\\first.jpg";

            var fileName = file.Split('\\').Last();

            using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var fts = new FileToSend(fileName, fileStream);
                return await MyBot.Api.SendPhotoAsync(message.Chat.Id, fts, PromptMessage);
            }
        }

        public async Task<Message> WelcomeMessage(Message message)
        {
            var PromptMessage = "🔘 چه عملیاتی انجام شود؟";

            _vsService.AddViewState(message, States.Welcome);

            var replyKeyboard = new ReplyKeyboardMarkup(new[]
                {
                    new [] // first row
                    {
                        new KeyboardButton("🔑 ورود به سامانه")
                    },
                    new [] // second row
                    {
                        new KeyboardButton("📂 ثبت‌نام")
                    },
                    new[] // last row
                    {
                        new KeyboardButton("✅ درباره ما"),
                        new KeyboardButton("📚 راهنما"),
                        new KeyboardButton("💌 تماس با ما")
                    }
                });

            return await MyBot.Api.SendTextMessageAsync(message.Chat.Id, PromptMessage, false, false, 0, replyKeyboard);
        }
        public async Task<Message> AboutUs(Message message)
        {
            var PromptMessage = "پیام درباره ما";

            var replyKeyboard = new ReplyKeyboardMarkup(new[]
                {
                    new KeyboardButton("🔙")
                });

            return await MyBot.Api.SendTextMessageAsync(message.Chat.Id, PromptMessage, false, false, 0, replyKeyboard);
        }
        public async Task<Message> ContactUs(Message message)
        {
            var PromptMessage = "پیام اطلاعات تماس با ما";

            var replyKeyboard = new ReplyKeyboardMarkup(new[]
                {
                    new KeyboardButton("🔙")
                });

            return await MyBot.Api.SendTextMessageAsync(message.Chat.Id, PromptMessage, false, false, 0, replyKeyboard);
        }
        public async Task<Message> HelpMessage(Message message)
        {
            var PromptMessage = "راهنمای کلی";

            var replyKeyboard = new ReplyKeyboardMarkup(new[]
                {
                    new KeyboardButton("🔙")
                });

            return await MyBot.Api.SendTextMessageAsync(message.Chat.Id, PromptMessage, false, false, 0, replyKeyboard);
        }
        public async Task<Message> InsertUserName(Message message)
        {
            var PromptMessage = "نام کاربری خود را وارد نمایید:";

            _vsService.AddViewState(message, States.Username, States.Welcome);

            var replyKeyboard = new ReplyKeyboardMarkup(new[]
                {
                    new KeyboardButton("🔙")
                });

            return await MyBot.Api.SendTextMessageAsync(message.Chat.Id, PromptMessage, false, false, 0, replyKeyboard);
        }
        public async Task<Message> UsernameRecieved(Message message)
        {
            var PromptMessage = "نام کاربری شما ثبت شد!";

            _userService.AddMessage(message, States.Username);

            return await MyBot.Api.SendTextMessageAsync(message.Chat.Id, PromptMessage, false, false, message.MessageId, new ReplyKeyboardHide());
        }
        public async Task<Message> InsertPassword(Message message)
        {
            var PromptMessage = "رمز ورود خود را وارد نمایید:";

            _vsService.AddViewState(message, States.Password);

            var replyKeyboard = new ReplyKeyboardMarkup(new[]
                {
                    new KeyboardButton("🔙")
                });

            return await MyBot.Api.SendTextMessageAsync(message.Chat.Id, PromptMessage, false, false, 0, replyKeyboard);
        }
        public async Task<Message> PasswordRecieved(Message message)
        {
            var PromptMessage = "رمز ورود ثبت شد!";

            _userService.AddMessage(message, States.Password);

            return await MyBot.Api.SendTextMessageAsync(message.Chat.Id, PromptMessage, false, false, message.MessageId, new ReplyKeyboardHide());
        }
        public async Task<Message> UnAuthorized(Message message)
        {
            var PromptMessage = "نام کاربری و رمز ورود شما در سیستم ثبت نشده و یا هنوز فعال نشده‌است.";

            return await MyBot.Api.SendTextMessageAsync(message.Chat.Id, PromptMessage, false, false, 0, new ReplyKeyboardHide());
        }
        public async Task<Message> LoginMessage(Message message)
        {
            var PromptMessage = "ورود موفقیت آمیز! خوش آمدید";

            return await MyBot.Api.SendTextMessageAsync(message.Chat.Id, PromptMessage, false, false, 0, new ReplyKeyboardHide());
        }
        public async Task<Message> UnknownMessage(Message message)
        {
            var PromptMessage = "پیام ارسال شده تعریف نشده است!";

            return await MyBot.Api.SendTextMessageAsync(message.Chat.Id, PromptMessage, false, false, message.MessageId);
        }
        public async Task<Message> SendTextMessage(Message message, string PromptMessage)
        {
            return await MyBot.Api.SendTextMessageAsync(message.Chat.Id, PromptMessage);
        }
    }
}
