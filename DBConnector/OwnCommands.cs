using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql;
using MySql.Data.MySqlClient;

namespace DBConnector
{
    public class OwnCommands : MySQL
    {
        public void MarkCall()
        {
            string query = "INSERT INTO calls (date) VALUES(now())";

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        public int getCallerID()
        {
            string query = "SELECT `id` FROM `calls` ORDER BY id DESC";
            int id = -1;

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                id = int.Parse(cmd.ExecuteScalar() + "");
                this.CloseConnection();
                return id;
            }
            else
            {
                return id;
            }
        }

        public int Count()
        {
            string query = "SELECT Count(*) FROM calls";
            int Count = -1;

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);        
                Count = int.Parse(cmd.ExecuteScalar() + "");
                this.CloseConnection();
                return Count;
            }
            else
            {
                return Count;
            }
        }

        public void SaveActivity(int caller_id, string question, string answer)
        {
            string query = "INSERT INTO activity (caller_id, question, answer) VALUES("+caller_id+",'"+question+"','"+answer+"')";

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

    }
}
