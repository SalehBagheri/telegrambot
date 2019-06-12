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
    public class PrivateState
    {
        private readonly UserService _userService;
        private readonly ViewStateService _vsService;
        public PrivateState()
        {
            _userService = new UserService();
            _vsService = new ViewStateService();
        }
        public async Task<Message> HomePageMessage(Message message)
        {
            var PromptMessage = "🏛 چه عملیاتی انجام شود؟";

            _vsService.AddViewState(message, States.Home);

            var replyKeyboard = new ReplyKeyboardMarkup(new[]
                {
                    new []
                    {
                        new KeyboardButton("🖼 تصویر نمونه"),
                        new KeyboardButton("📙 فایل نمونه"),
                    },
                    new []
                    {
                        new KeyboardButton("🎥 ویدئوی نمونه"),
                        new KeyboardButton("🎶 آهنگ نمونه"),
                    },
                    new []
                    {
                        new KeyboardButton("🌐") { RequestLocation = true },
                        new KeyboardButton("📞") {RequestContact = true },
                    },
                    new[]
                    {
                        new KeyboardButton("✅ درباره ما"),
                        new KeyboardButton("💌 تماس با ما")
                    },
                    new[] // last row
                    {
                        new KeyboardButton("❌ خروج"),
                        new KeyboardButton("📚 راهنما")
                    }
                });
            
            return await MyBot.Api.SendTextMessageAsync(message.Chat.Id, PromptMessage, false, false, 0, replyKeyboard);
        }
        public async Task<Message> SignOut(Message message)
        {
            var PromptMessage = "✅ خروج موفقیت آمیز";

            _vsService.AddViewState(message, States.Welcome);

            var replyKeyboard = new ReplyKeyboardMarkup(new[]
                {
                    new KeyboardButton("صفحه نخست")
                });

            return await MyBot.Api.SendTextMessageAsync(message.Chat.Id, PromptMessage, false, false, 0, replyKeyboard);
        }
        
        public async Task<Message> HelpMessage(Message message)
        {
            var PromptMessage = "پیام راهنمای داخلی سیستم";

            var replyKeyboard = new ReplyKeyboardMarkup(new[]
                {
                    new KeyboardButton("🔙")
                });

            return await MyBot.Api.SendTextMessageAsync(message.Chat.Id, PromptMessage, false, false, 0, replyKeyboard);
        }

        public async Task<Message> SendSamplePdf(Message message)
        {
            await MyBot.Api.SendChatActionAsync(message.Chat.Id, ChatAction.UploadDocument);
            MyBot.Api.UploadTimeout = new TimeSpan(0, 15, 0);

            var PromptMessage = "PDF نمونه";

            string file = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\pdf\\sample.pdf";

            var fileName = file.Split('\\').Last();

            using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var fts = new FileToSend(fileName, fileStream);
                return await MyBot.Api.SendDocumentAsync(message.Chat.Id, fts, PromptMessage);
            }
        }

        public async Task<Message> SendSampleImage(Message message)
        {
            await MyBot.Api.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);

            var PromptMessage = "نمونه تصویر ارسالی";

            string file = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\img\\sample.jpg";

            var fileName = file.Split('\\').Last();

            using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var fts = new FileToSend(fileName, fileStream);
                return await MyBot.Api.SendPhotoAsync(message.Chat.Id, fts, PromptMessage);
            }
        }

        public async Task<Message> SendSampleVideo(Message message)
        {
            await MyBot.Api.SendChatActionAsync(message.Chat.Id, ChatAction.UploadVideo);
            MyBot.Api.UploadTimeout = new TimeSpan(0, 15, 0);

            var PromptMessage = "نمونه ویدئوی ارسالی";

            string file = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\video\\sample.mov";

            var fileName = file.Split('\\').Last();

            using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var fts = new FileToSend(fileName, fileStream);
                return await MyBot.Api.SendVideoAsync(message.Chat.Id, fts, 0, PromptMessage);
            }
        }

        public async Task<Message> SendSampleAudio(Message message)
        {
            await MyBot.Api.SendChatActionAsync(message.Chat.Id, ChatAction.UploadAudio);
            MyBot.Api.UploadTimeout = new TimeSpan(0, 15, 0);

            var PromptMessage = "نمونه آهنگ ارسالی";

            string file = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\audio\\sample.mp3";

            var fileName = file.Split('\\').Last();

            using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var fts = new FileToSend(fileName, fileStream);
                return await MyBot.Api.SendAudioAsync(message.Chat.Id, fts, 0, "",PromptMessage);
            }
        }
    }
}
