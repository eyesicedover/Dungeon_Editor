using System.Collections.Generic;
using MySql.Data.MySqlClient;
// using MySQLCore.Models;
using System;
using Dungeon.Models;
using Dungeon;

namespace Dungeon.Models
{
    public class Item
    {
        private string _name;
        private int _id;

        public Item(string name, int id = 0)
        {
            _name = name;
            _id = id;
        }

        public override bool Equals(System.Object otherItem)
        {
          if (!(otherItem is Item))
          {
            return false;
          }
          else
          {
             Item newItem = (Item) otherItem;
             bool idEquality = this.GetId() == newItem.GetId();
             bool nameEquality = this.GetName() == newItem.GetName();
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

        public static List<Item> GetAll()
        {
            List<Item> allItems = new List<Item> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM items;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
              int itemId = rdr.GetInt32(0);
              string itemDescription = rdr.GetString(1);
              Item newItem = new Item(itemDescription, itemId);
              allItems.Add(newItem);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allItems;
        }


        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM items;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
    }
}
