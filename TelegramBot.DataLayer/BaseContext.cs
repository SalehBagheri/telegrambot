using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.DataLayer
{
    public class BaseContext : IDisposable
    {
        public BaseContext()
        {
            SetConnection();
        }
        public SqlConnection conn;
        //To Handle connection related activities      
        private void SetConnection()
        {
            string connectionString = "Enter your connection string here";// WebConfigurationManager.ConnectionStrings["DefaultConnectionString"].ToString();
            conn = new SqlConnection(connectionString);
        }
        public void Dispose()
        {
        }
    }
}
