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
      List<Item> allItems = Item.GetAll();
      return View("ItemIndex", allItems);
    }

    [HttpPost("/items")]
    public ActionResult Create()
    {
      string name = Request.Form["newItemName"];
      Item newItem = new Item(name);
      newItem.Save();
      List<Item> allItems = Item.GetAll();
      return View("ItemIndex", allItems);
    }

    [HttpPost("/items/details/{id}")]
    public ActionResult Details()
    {
      Item thisItem = Item.Find(Int32.Parse(Request.Form["id"]));
      return View("ItemDetails", thisItem);
    }

    [HttpPost("/items/update/{id}")]
    public ActionResult Details(int id)
    {
      Item thisItem = Item.Find(id);
      thisItem.Update(Request.Form["updatedItemName"]);
      return View("ItemDetails", thisItem);
    }

  }
}
