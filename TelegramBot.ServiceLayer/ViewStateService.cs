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
    public class ViewStateService 
    {
        private readonly ViewStateContext _vsContext;
        public ViewStateService()
        {
            _vsContext = new ViewStateContext();
        }

        public void AddViewState(Message msg, States current, States parent = States.Empty)
        {
            var state = new ViewState()
            {
                UserId = msg.From.Id,
                ChatId = msg.Chat.Id,
                CurrentState = current,
                ParentState = parent,
                CreatedOn = DateTime.Now
            };
            _vsContext.AddViewState(state);
        }
        public ViewState GetViewState(Message msg)
        {
            return _vsContext.GetViewState(msg.From.Id);
        }
    }
}
