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
        return View("NPCIndex");
      }

  }
}
