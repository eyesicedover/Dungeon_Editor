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

        public static List<PC> GetAll()
        {
            List<PC> allPCs = new List<PC> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM pcs;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
              int pcId = rdr.GetInt32(0);
              string pcDescription = rdr.GetString(1);
              PC newPC = new PC(pcDescription, pcId);
              allPCs.Add(newPC);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allPCs;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO pcs (name) VALUES (@name);";

            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@name";
            name.Value = this._name;
            cmd.Parameters.Add(name);

            // Code to declare, set, and add values to a categoryId SQL parameters has also been removed.

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static PC Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM pcs WHERE id = (@searchId);";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int pcId = 0;
            string pcName = "";

            while(rdr.Read())
            {
              pcId = rdr.GetInt32(0);
              pcName = rdr.GetString(1);
            }

            PC newPC = new PC(pcName, pcId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return newPC;
        }

        public void Delete()
        {
            // Delete PC entirely
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = new MySqlCommand("DELETE FROM pcs WHERE id = @PCId; DELETE FROM inventory WHERE pc_id = @PCId;", conn);
            MySqlParameter pcIdParameter = new MySqlParameter();
            pcIdParameter.ParameterName = "@PCId";
            pcIdParameter.Value = this.GetId();

            cmd.Parameters.Add(pcIdParameter);
            cmd.ExecuteNonQuery();

            if (conn != null)
            {
                conn.Close();
            }
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
