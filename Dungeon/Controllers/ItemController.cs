using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;
using Dungeon.Models;

namespace Dungeon.Controllers
{
  public class ItemController : Controller
  {
      [HttpGet("/items")]
      public ActionResult Index()
      {
        return View("ItemIndex");
      }

  }
}
