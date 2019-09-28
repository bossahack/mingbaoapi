using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Book.Dal
{
    public class SqlHelper
    {
        static SqlHelper()
        {
            Dapper.SimpleCRUD.SetDialect(Dapper.SimpleCRUD.Dialect.MySQL);
        }

        public static MySqlConnection GetInstance()
        {
            var conn = new MySqlConnection(WebConfigurationManager.AppSettings["mysqlConnection"]);            
            return conn;
        }
    }
}
