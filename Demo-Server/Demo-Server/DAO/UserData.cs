using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using SocketGameProtocol;

namespace Demo_Server.DAO
{
    class UserData
    {
    
     
        bool CheckHasUser(string username,MySqlConnection conn)
        {bool hasUser = false;
            string sql = string.Format("select * from user where username='{0}' ", username);
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                   MySqlDataReader reader = cmd.ExecuteReader();     
                   if (reader.Read())     
                   {
                        hasUser = true;
                   }
                    reader.Close();
                }
            }
            catch
            {

            }
            return hasUser;
        }
        public bool Logon(MainPack pack,MySqlConnection conn)
        {
            string username = pack.LoginPack.Username;
            string passwork = pack.LoginPack.Password;
            if (CheckHasUser(username, conn))
            {
                return false;
            }      
            // {0}改为@username 不需要格式函数
            string sql = string.Format("insert into user(username,password) values('{0}','{1}')",username,passwork);
            bool isSuccess = false;
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                         Console.WriteLine("注册成功");
                    isSuccess = true;
                }                   
            }
            return isSuccess;
        }
        public bool Login(MainPack pack, MySqlConnection conn)
        {
            string username = pack.LoginPack.Username;
            string password = pack.LoginPack.Password;
            bool isSuccess = false;
            string sql = string.Format("select * from user where username='{0}' and password='{1}' ", username,password);
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        isSuccess = true;
                    }
                    reader.Close();
                }
            }
            catch
            {
            }
            return isSuccess;
        }
    }
}
