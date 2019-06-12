using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Models
{
    /// <summary>
    /// این مدل برای ذخیره پیام های ارسالی کاربران می‌باشد
    /// </summary>
    public class UserMessage
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public long ChatId { get; set; }
        public int MessageId { get; set; }
        public string MessageText { get; set; }
        public States CurrentState { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
