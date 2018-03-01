using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;
using Dungeon.Models;

namespace Dungeon.Controllers
{
  public class PCController : Controller
  {
      [HttpGet("/pcs")]
      public ActionResult Index()
      {
        List<PC> allPCs = PC.GetAll();
        return View("PCIndex", allPCs);
      }

      [HttpPost("/pcs")]
      public ActionResult Create()
      {
        string name = Request.Form["newPCName"];
        PC newPC = new PC(name);
        newPC.Save();
        List<PC> allPCs = PC.GetAll();
        return View("PCIndex", allPCs);
      }

      [HttpPost("/pcs/details/{id}")]
      public ActionResult Details()
      {
        PC thisPC = PC.Find(Int32.Parse(Request.Form["id"]));
        return View("PCDetails", thisPC);
      }

      [HttpPost("/pcs/update/{id}")]
      public ActionResult Details(int id)
      {
        // PC TempPC = PC.Find(id);
        //
        // temp_Name = TempPC.GetName();
        // temp_Type = TempPC.GetType();
        // temp_HP = TempPC.GetHP();
        // temp_AC = TempPC.GetAC();
        // temp_Damage = TempPC.GetDamage();
        // temp_Lvl = TempPC.GetLVL();
        // temp_Exp = TempPC.GetEXP();
        // temp_RoomId = TempPC.GetRoomId();
        // // _id = id;
        PC thisPC = PC.Find(id);

        string temp_Name = Request.Form["updatedPCName"];
        string temp_Type = Request.Form["updatedPCType"];
        int temp_HP = Int32.Parse(Request.Form["updatedPCHP"]);
        int temp_AC = Int32.Parse(Request.Form["updatedPCAC"]);
        int temp_Damage = Int32.Parse(Request.Form["updatedPCDamage"]);
        int temp_LVL = Int32.Parse(Request.Form["updatedPCLVL"]);
        int temp_EXP = Int32.Parse(Request.Form["updatedPCEXP"]);
        int temp_RoomId = Int32.Parse(Request.Form["updatedPCRoomId"]);

        thisPC.Update(temp_Name, temp_Type, temp_HP, temp_AC, temp_Damage, temp_LVL, temp_EXP, temp_RoomId);

        PC thisUpdatedPC = PC.Find(id);

        return View("PCDetails", thisUpdatedPC);
      }
  }
}
