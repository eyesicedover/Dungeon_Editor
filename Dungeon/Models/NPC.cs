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

    }
}
