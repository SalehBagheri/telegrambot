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
    public class ViewStateContext : BaseContext
    {
        public ViewStateContext() { }

        public void AddViewState(ViewState viewState)
        {
            try
            {
                conn.Open();
                var command = "INSERT INTO [dbo].[ViewStates] ([UserId], [ChatId], [ParentState], [CurrentState], [CreatedOn]) VALUES (@UserId, @ChatId, @ParentState, @CurrentState, @CreatedOn)";
                conn.Execute(command, viewState, commandType: CommandType.Text);
                conn.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ViewState GetViewState(int userId)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@UserId", userId);
                conn.Open();
                var command = "SELECT * FROM [dbo].[ViewStates] WHERE [UserId] = @UserId ORDER BY [CreatedOn] DESC";
                ViewState State = SqlMapper.Query<ViewState>(conn, command, param).FirstOrDefault();
                conn.Close();
                return State;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
