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

        public static List<Room> GetAll()
        {
            List<Room> allRooms = new List<Room> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM rooms;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
              int roomId = rdr.GetInt32(0);
              string roomDescription = rdr.GetString(1);
              Room newRoom = new Room(roomDescription, roomId);
              allRooms.Add(newRoom);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allRooms;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO rooms (name) VALUES (@name);";

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
