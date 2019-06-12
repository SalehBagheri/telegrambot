using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Models;

namespace TelegramBot.DataLayer
{
    public class UserContext : BaseContext
    {
        public UserContext() { }

        public void AddUser(TelegramUser user)
        {
            try
            {
                conn.Open();
                var command = "INSERT INTO [dbo].[TelegramUsers] ([FullName], [Username], [Password], [TelegramId], [Mobile], [RegisterDate], [LastLogin], [IsActive]) VALUES (@FullName, @Username, @Password, @TelegramId, @Mobile, @RegisterDate, @LastLogin, @IsActive)";
                conn.Execute(command, user, commandType: CommandType.Text);
                conn.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public TelegramUser GetUser(int userId)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Id", userId);
                conn.Open();
                var command = "SELECT * FROM [dbo].[TelegramUsers] WHERE [Id] = @Id";
                TelegramUser User = SqlMapper.Query<TelegramUser>(conn, command, param).FirstOrDefault();
                conn.Close();
                return User;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public TelegramUser GetUser(string userName, string password)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Username", userName);
                param.Add("@Password", password);
                conn.Open();
                var command = "SELECT TOP (1) * FROM [dbo].[TelegramUsers] WHERE [Username] = @Username AND [Password] = @Password AND [IsActive] = 1";
                TelegramUser User = SqlMapper.Query<TelegramUser>(conn, command, param).FirstOrDefault();
                conn.Close();
                return User;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// تابعی برای چک کردن عدم ورود تکراری نام کاربری
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool CheckUsername(string username)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Username", username);
                conn.Open();
                var command = "SELECT TOP (1) * FROM [dbo].[TelegramUsers] WHERE [Username] = @Username";
                TelegramUser User = SqlMapper.Query<TelegramUser>(conn, command, param).FirstOrDefault();
                conn.Close();
                return (User != null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddMessage(UserMessage message)
        {
            try
            {
                conn.Open();
                var command = "INSERT INTO [dbo].[UserMessages] ([UserId], [ChatId], [MessageId], [MessageText], [CurrentState], [CreatedOn]) VALUES (@UserId, @ChatId, @MessageId, @MessageText, @CurrentState, @CreatedOn)";
                conn.Execute(command, message, commandType: CommandType.Text);
                conn.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public UserMessage GetMessage(int userId, States viewState)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@UserId", userId);
                param.Add("@CurrentState", viewState);
                conn.Open();
                var command = "SELECT TOP (1) * FROM [dbo].[UserMessages] WHERE [UserId] = @UserId AND [CurrentState] = @CurrentState ORDER BY [CreatedOn] DESC";
                UserMessage Message = SqlMapper.Query<UserMessage>(conn, command, param).FirstOrDefault();
                conn.Close();
                return Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
