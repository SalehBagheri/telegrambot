using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramBot.DataLayer;
using TelegramBot.Models;

namespace TelegramBot.ServiceLayer
{
    public class UserService
    {
        private readonly ViewStateService _vsService;
        public UserService()
        {
            _vsService = new ViewStateService();
        }

        public void AddUser(Message msg)
        {
            var fullname = GetLastMessage(msg, States.FullName).MessageText;
            var username = GetLastMessage(msg, States.Username).MessageText;
            var password = GetLastMessage(msg, States.Password).MessageText;
            var mobile = GetLastMessage(msg, States.Mobile).MessageText;
            var telegramId = GetLastMessage(msg, States.TelegramId).MessageText;
            var newUser = new TelegramUser()
            {
                FullName = fullname,
                Username = username,
                Password = password,
                Mobile = mobile,
                TelegramId = telegramId,
                RegisterDate = DateTime.Now,
                IsActive = true
            };
            using (var db = new UserContext())
            {
                db.AddUser(newUser);
            }
        }

        public TelegramUser GetUser(Message msg)
        {
            using (var db = new UserContext())
            {
                return db.GetUser(msg.From.Id);
            }
        }

        public bool IsUsernameExist(string username)
        {
            using (var db = new UserContext())
            {
                return db.CheckUsername(username);
            }
        }

        public bool SignIn(Message msg)
        {
            var username = GetLastMessage(msg, States.Username).MessageText;
            var password = GetLastMessage(msg, States.Password).MessageText;
            using (var db = new UserContext())
            {
                return (db.GetUser(username, password) != null);
            }
        }

        public void AddMessage(Message msg, States current)
        {
            var userMsg = new UserMessage()
            {
                UserId = msg.From.Id,
                ChatId = msg.Chat.Id,
                MessageId = msg.MessageId,
                MessageText = msg.Text,
                CurrentState = current,
                CreatedOn = DateTime.Now
            };
            using (var db = new UserContext())
            {
                db.AddMessage(userMsg);
            }
        }
        public UserMessage GetLastMessage(Message msg, States current)
        {
            using (var db = new UserContext())
            {
                return db.GetMessage(msg.From.Id, current);
            }
        }


    }
}
