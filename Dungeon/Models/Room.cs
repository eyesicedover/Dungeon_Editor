using System.Collections.Generic;
using MySql.Data.MySqlClient;
// using MySQLCore.Models;
using System;
using Dungeon.Models;
using Dungeon;

namespace Dungeon.Models
{

    public class Room
    {
        private string _name;
        private int _id;
        // We no longer declare _RoomId here

        public Room(string name, int id = 0)
        {
            _name = name;
            _id = id;
           // RoomId is removed from the constructor
        }

        public override bool Equals(System.Object otherRoom)
        {
          if (!(otherRoom is Room))
          {
            return false;
          }
          else
          {
             Room newRoom = (Room) otherRoom;
             bool idEquality = this.GetId() == newRoom.GetId();
             bool nameEquality = this.GetName() == newRoom.GetName();
             // We no longer compare Items' RoomIds in a RoomEquality bool here.
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

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM rooms;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

    }
}
