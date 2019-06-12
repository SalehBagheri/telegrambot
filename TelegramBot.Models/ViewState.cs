using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Models
{
    /// <summary>
    /// این مدل برای ذخیره موقعیت های صفحات برای هر کاربر می‌باشد
    /// </summary>
    public class ViewState
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public long ChatId { get; set; }
        public States ParentState { get; set; }
        public States CurrentState { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public enum States
    {        
        Empty,
        Home,
        Username,
        Password,
        Help,
        AboutUs,
        FullName,
        TelegramId,
        Mobile,
        Welcome,
    }
}
