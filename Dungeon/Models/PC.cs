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

        public void Update(string newName)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE pcs SET name = @newName WHERE id = @searchId;";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = _id;
            cmd.Parameters.Add(searchId);

            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@newName";
            name.Value = newName;
            cmd.Parameters.Add(name);

            cmd.ExecuteNonQuery();
            _name = newName;
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

        public void AddItemToPC(Item newItem)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO inventory (pcs, items) VALUES (@PCId, @ItemId);";

            MySqlParameter pcs = new MySqlParameter();
            pcs.ParameterName = "@PCId";
            pcs.Value = _id;
            cmd.Parameters.Add(pcs);

            MySqlParameter items = new MySqlParameter();
            items.ParameterName = "@ItemId";
            items.Value = newItem.GetId();
            cmd.Parameters.Add(items);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Item> GetItems()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT items.* FROM pcs
              JOIN inventory ON (pcs.id = inventory.pcs)
              JOIN items ON (inventory.items = items.id)
              WHERE pcs.id = @PCId;";

            MySqlParameter pcIdParameter = new MySqlParameter();
            pcIdParameter.ParameterName = "@PCId";
            pcIdParameter.Value = _id;
            cmd.Parameters.Add(pcIdParameter);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Item> items = new List<Item>{};

            while(rdr.Read())
            {
                int itemId = rdr.GetInt32(0);
                string itemName = rdr.GetString(1);
                Item newItem = new Item(itemName, itemId);
                items.Add(newItem);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        return items;
        }

    }
}
