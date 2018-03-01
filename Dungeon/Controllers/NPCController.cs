using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;
using Dungeon.Models;

namespace Dungeon.Controllers
{
  public class NPCController : Controller
  {
      [HttpGet("/npcs")]
      public ActionResult Index()
      {
        List<NPC> allNPCs = NPC.GetAll();
        return View("NPCIndex", allNPCs);
      }

      [HttpPost("/npcs")]
      public ActionResult Create()
      {
        string name = Request.Form["newNPCName"];
        NPC newNPC = new NPC(name);
        newNPC.Save();
        List<NPC> allNPCs = NPC.GetAll();
        return View("NPCIndex", allNPCs);
      }

      [HttpPost("/npcs/details/{id}")]
      public ActionResult Details()
      {
        NPC thisNPC = NPC.Find(Int32.Parse(Request.Form["id"]));
        return View("NPCDetails", thisNPC);
      }

      [HttpPost("/npcs/update/{id}")]
      public ActionResult Details(int id)
      {
        NPC thisNPC = NPC.Find(id);
        thisNPC.Update(Request.Form["updatedNPCName"]);
        return View("NPCDetails", thisNPC);
      }
  }
}
