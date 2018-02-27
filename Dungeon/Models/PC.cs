using System.Collections.Generic;
using MySql.Data.MySqlClient;
// using MySQLCore.Models;
using System;
using Dungeon.Models;
using Dungeon;

namespace Dungeon.Models
{
    public class PC
    {
        private string _name;
        private int _id;

        public PC(string Name, int id = 0)
        {
            _name = Name;
            _id = id;
        }

        public override bool Equals(System.Object otherPC)
        {
          if (!(otherPC is PC))
          {
            return false;
          }
          else
          {
             PC newPC = (PC) otherPC;
             bool idEquality = this.GetId() == newPC.GetId();
             bool nameEquality = this.GetName() == newPC.GetName();
             // We no longer compare Items' categoryIds in a categoryEquality bool here.
             return (idEquality && nameEquality);
           }
        }
        public override int GetHashCode()
        {
             return this.GetName().GetHashCode();
        }

        public string GetName()
        {
            return _name;
        }

        public int GetId()
        {
            return _id;
        }

        public static string GetString()
        {
            return "this is a string from the model";
        }

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM PCs;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

    }
}
