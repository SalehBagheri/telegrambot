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
            string connectionString = "Data Source=.;Initial Catalog=TelegramBot;User ID=saleh; Password=1234";// WebConfigurationManager.ConnectionStrings["SBPMSConnectionString"].ToString();
            conn = new SqlConnection(connectionString);
        }
        public void Dispose()
        {
        }
    }
}
