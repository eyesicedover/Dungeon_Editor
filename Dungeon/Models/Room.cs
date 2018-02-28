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

            // Code to declare, set, and add values to a roomId SQL parameters has also been removed.

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static Room Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM rooms WHERE id = (@searchId);";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int roomId = 0;
            string roomName = "";
            // We remove the line setting a itemRoomId value here.

            while(rdr.Read())
            {
              roomId = rdr.GetInt32(0);
              roomName = rdr.GetString(1);
              // We no longer read the itemRoomId here, either.
            }

            // Constructor below no longer includes a itemRoomId parameter:
            Room newRoom = new Room(roomName, roomId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return newRoom;
        }

        public void Delete()
        // Delete's the room
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = new MySqlCommand("DELETE FROM rooms WHERE id = @RoomId; DELETE FROM contents WHERE room_id = @RoomId;", conn);
            MySqlParameter roomIdParameter = new MySqlParameter();
            roomIdParameter.ParameterName = "@RoomId";
            roomIdParameter.Value = this.GetId();

            cmd.Parameters.Add(roomIdParameter);
            cmd.ExecuteNonQuery();

            if (conn != null)
            {
                conn.Close();
            }
        }

        public void AddItemToRoom(Item newItem)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO contents (rooms, items) VALUES (@RoomId, @ItemId);";

            MySqlParameter rooms = new MySqlParameter();
            rooms.ParameterName = "@RoomId";
            rooms.Value = _id;
            cmd.Parameters.Add(rooms);

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
            cmd.CommandText = @"SELECT items.* FROM rooms
              JOIN contents ON (rooms.id = contents.rooms)
              JOIN items ON (contents.items = items.id)
              WHERE rooms.id = @RoomId;";

            MySqlParameter roomIdParameter = new MySqlParameter();
            roomIdParameter.ParameterName = "@RoomId";
            roomIdParameter.Value = _id;
            cmd.Parameters.Add(roomIdParameter);

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

        public static void DeleteAll()
        {
          MySqlConnection conn = DB.Connection();
          conn.Open();
          var cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"DELETE FROM rooms; DELETE FROM contents;";
          cmd.ExecuteNonQuery();
          conn.Close();
          if (conn != null)
          {
            conn.Dispose();
          }
        }

    }
}
