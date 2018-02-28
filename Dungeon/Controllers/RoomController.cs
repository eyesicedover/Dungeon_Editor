using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;
using Dungeon.Models;

namespace Dungeon.Controllers
{
  public class RoomController : Controller
  {
      [HttpGet("/rooms")]
      public ActionResult Index()
      {
        List<Room> allRooms = Room.GetAll();
        return View("RoomIndex", allRooms);
      }

      [HttpPost("/rooms")]
      public ActionResult Create()
      {
        string name = Request.Form["newRoomName"];
        Room newRoom = new Room(name);
        newRoom.Save();
        List<Room> allRooms = Room.GetAll();
        return View("RoomIndex", allRooms);
      }

      [HttpPost("/rooms/details/{id}")]
      public ActionResult Details()
      {
        Room thisRoom = Room.Find(Int32.Parse(Request.Form["id"]));
        return View("RoomDetails", thisRoom);
      }

      [HttpPost("/rooms/update/{id}")]
      public ActionResult Details(int id)
      {
        Room thisRoom = Room.Find(id);
        thisRoom.Update(Request.Form["updatedRoomName"]);
        return View("RoomDetails", thisRoom);
      }

  }
}
