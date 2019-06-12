using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Models
{
    /// <summary>
    /// این مدل برای ذخیره کاربران ثبت نام شده در سیستم می‌باشد
    /// </summary>
    public class TelegramUser
    {
        public int Id { set; get; }
        public string FullName { set; get; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string TelegramId { get; set; }
        public string Mobile { get; set; }
        public DateTime RegisterDate { set; get; }
        public DateTime LastLogin { set; get; }
        public bool IsActive { get; set; }
    }
}
