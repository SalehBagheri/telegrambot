using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using TelegramBot.Models;
using TelegramBot.ServiceLayer;
using TelegramBot.ServiceLayer.Telegram;

namespace TelegramBot.Application
{
    class Program
    {
        private static UserService _userService;
        private static ViewStateService _vsService;
        private static TelegramService _telegramService;
        static void Main(string[] args)
        {
            _userService = new UserService();
            _vsService = new ViewStateService();
            _telegramService = new TelegramService();

            MyBot.Api.OnCallbackQuery += BotOnCallbackQueryReceived;
            MyBot.Api.OnMessage += BotOnMessageReceived;
            MyBot.Api.OnMessageEdited += BotOnMessageReceived;
            MyBot.Api.OnReceiveError += BotOnReceiveError;

            MyBot.Api.SetWebhookAsync().Wait();
            var me = MyBot.Api.GetMeAsync().Result;

            Console.Title = me.Username;

            MyBot.Api.StartReceiving();
            Console.ReadLine();
            MyBot.Api.StopReceiving();
        }


        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            Debugger.Break();
        }



        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;

            if(message.Type == MessageType.ContactMessage)
            {
                await _telegramService.PublicMessages.SendTextMessage(message, "مرسی بابت ارسال اطلاعات تماس");
                await _telegramService.PrivateMessages.HomePageMessage(message);
            }
            else if(message.Type == MessageType.LocationMessage)
            {
                await _telegramService.PublicMessages.SendTextMessage(message, "مرسی بابت ارسال اطلاعات مکانی");
                await _telegramService.PrivateMessages.HomePageMessage(message);
            }
            else if (message.Type == MessageType.TextMessage)
            {
                #region TextMessageProcess
                var currentState = _vsService.GetViewState(message)?.CurrentState;

                if (currentState == States.Home) // user loged in
                {
                    if (message.Text == "🖼 تصویر نمونه")
                    {
                        await _telegramService.PrivateMessages.SendSampleImage(message);
                    }
                    else if (message.Text == "📙 فایل نمونه")
                    {
                        await _telegramService.PrivateMessages.SendSamplePdf(message);
                    }
                    else if (message.Text == "🎥 ویدئوی نمونه")
                    {
                        await _telegramService.PrivateMessages.SendSampleVideo(message);
                    }
                    else if (message.Text == "🎶 آهنگ نمونه")
                    {
                        await _telegramService.PrivateMessages.SendSampleAudio(message);
                    }
                    else if (message.Text == "🌐")
                    {
                        //await _telegramService.PrivateMessages;
                    }
                    else if (message.Text == "📞")
                    {
                        //await _telegramService.PrivateMessages;
                    }
                    else if (message.Text == "✅ درباره ما")
                    {
                        await _telegramService.PublicMessages.AboutUs(message);
                    }
                    else if (message.Text == "💌 تماس با ما")
                    {
                        await _telegramService.PublicMessages.ContactUs(message);
                    }
                    else if (message.Text == "❌ خروج")
                    {
                        await _telegramService.PrivateMessages.SignOut(message);
                    }
                    else if (message.Text == "📚 راهنما")
                    {
                        await _telegramService.PrivateMessages.HelpMessage(message);
                    }
                    else
                    {
                        await _telegramService.PrivateMessages.HomePageMessage(message);
                    }
                }

                else if (currentState == States.Username)
                {
                    if (message.Text == "🔙")
                    {
                        await _telegramService.PublicMessages.WelcomeMessage(message);
                    }
                    else
                    {
                        await _telegramService.PublicMessages.UsernameRecieved(message);
                        await Task.Delay(500);
                        await _telegramService.PublicMessages.InsertPassword(message);
                    }
                }
                else if (currentState == States.Password)
                {
                    if (message.Text == "🔙")
                    {
                        await _telegramService.PublicMessages.InsertUserName(message);
                    }
                    else
                    {
                        await _telegramService.PublicMessages.PasswordRecieved(message);
                        await Task.Delay(500);
                        if (_userService.SignIn(message))
                        {
                            await _telegramService.PublicMessages.LoginMessage(message);
                            await Task.Delay(500);
                            await _telegramService.PrivateMessages.HomePageMessage(message); //successfully loged in
                        }
                        else
                        {
                            await _telegramService.PublicMessages.UnAuthorized(message); // login unsuccessful
                            await Task.Delay(500);
                            await _telegramService.PublicMessages.WelcomeMessage(message);
                        }
                    }
                }

                else if (currentState == null) // user not loged in
                {
                    await _telegramService.PublicMessages.FirstMessage(message);
                    await _telegramService.PublicMessages.WelcomeMessage(message);
                }

                else // Welcome state
                {
                    if (message.Text == "🔑 ورود به سامانه")
                    {
                        await _telegramService.PublicMessages.InsertUserName(message);
                    }
                    else if (message.Text == "📂 ثبت‌نام")
                    {
                        await _telegramService.PublicMessages.HelpMessage(message);
                    }
                    else if (message.Text == "📚 راهنما")
                    {
                        await _telegramService.PublicMessages.HelpMessage(message);
                    }
                    else if (message.Text == "✅ درباره ما")
                    {
                        await _telegramService.PublicMessages.AboutUs(message);
                    }
                    else if (message.Text == "💌 تماس با ما")
                    {
                        await _telegramService.PublicMessages.ContactUs(message);
                    }
                    else
                    {
                        await _telegramService.PublicMessages.WelcomeMessage(message);
                    }
                }
                #endregion
            }
            else if (message == null || message.Text == string.Empty)
            {
                await _telegramService.PublicMessages.UnknownMessage(message);
                return;
            }
        }

        private static async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            await MyBot.Api.AnswerCallbackQueryAsync(callbackQueryEventArgs.CallbackQuery.Id,
                $"Received {callbackQueryEventArgs.CallbackQuery.Data}");
        }
    }
}
