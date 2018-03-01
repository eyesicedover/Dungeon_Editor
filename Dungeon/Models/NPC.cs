using System.Collections.Generic;
using MySql.Data.MySqlClient;
// using MySQLCore.Models;
using System;
using Dungeon.Models;
using Dungeon;

namespace Dungeon.Models
{
    public class NPC
    {
        private string _name;
        // private string _type;
        // private int _hp;
        // private int _ac;
        // private int _damage;
        // private int _lvl;
        // private int _roomId;
        private int _id;

        public NPC(string name, int id = 0)
        {
            _name = name;
            _id = id;
        }

        public override bool Equals(System.Object otherNPC)
        {
          if (!(otherNPC is NPC))
          {
            return false;
          }
          else
          {
             NPC newNPC = (NPC) otherNPC;
             bool idEquality = this.GetId() == newNPC.GetId();
             bool nameEquality = this.GetName() == newNPC.GetName();
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

        // public string GetType()
        // {
        //   return _type;
        // }
        //
        // public int GetHP()
        // {
        //     return _hp;
        // }
        //
        // public int GetAC()
        // {
        //     return _ac;
        // }
        //
        // public int GetDamage()
        // {
        //     return _damage;
        // }
        //
        // public int GetLVL()
        // {
        //     return _lvl;
        // }
        //
        // public int GetRoomId()
        // {
        //     return _roomId;
        // }

        public int GetId()
        {
            return _id;
        }

        public static string GetString()
        {
            return "this is a string from the model";
        }

        public static List<NPC> GetAll()
        {
            List<NPC> allNPCs = new List<NPC> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM npcs;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
              int npcId = rdr.GetInt32(0);
              string npcDescription = rdr.GetString(1);
              NPC newNPC = new NPC(npcDescription, npcId);
              allNPCs.Add(newNPC);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allNPCs;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO npcs (name) VALUES (@name);";

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
            cmd.CommandText = @"UPDATE npcs SET name = @newName WHERE id = @searchId;";

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

        public static NPC Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM npcs WHERE id = (@searchId);";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int npcId = 0;
            string npcName = "";

            while(rdr.Read())
            {
              npcId = rdr.GetInt32(0);
              npcName = rdr.GetString(1);
            }

            NPC newNPC = new NPC(npcName, npcId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return newNPC;
        }

        public void Delete()
        {
            // Delete NPC entirely
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = new MySqlCommand("DELETE FROM npcs WHERE id = @NPCId; DELETE FROM loot WHERE npc_id = @NPCId;", conn);
            MySqlParameter npcIdParameter = new MySqlParameter();
            npcIdParameter.ParameterName = "@NPCId";
            npcIdParameter.Value = this.GetId();

            cmd.Parameters.Add(npcIdParameter);
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
            cmd.CommandText = @"DELETE FROM NPCs;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void AddItemToNPC(Item newItem)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO loot (npcs, items) VALUES (@NPCId, @ItemId);";

            MySqlParameter npcs = new MySqlParameter();
            npcs.ParameterName = "@NPCId";
            npcs.Value = _id;
            cmd.Parameters.Add(npcs);

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
            cmd.CommandText = @"SELECT items.* FROM npcs
              JOIN loot ON (npcs.id = loot.npcs)
              JOIN items ON (loot.items = items.id)
              WHERE npcs.id = @NPCId;";

            MySqlParameter npcIdParameter = new MySqlParameter();
            npcIdParameter.ParameterName = "@NPCId";
            npcIdParameter.Value = _id;
            cmd.Parameters.Add(npcIdParameter);

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
