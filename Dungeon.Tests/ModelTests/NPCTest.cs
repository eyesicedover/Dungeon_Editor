using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
// using MySQLCore.Models;
using System;
using Dungeon.Models;
using Dungeon;

namespace Dungeon.Tests
{
  [TestClass]
  public class NPCTest : IDisposable
  {
    public NPCTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=dungeon_test;";
    }
    public void Dispose()
    {
      NPC.DeleteAll();
    }

    [TestMethod]
    public void GetAll_DatabaseEmptyAtFirst_0()
    {
      //Arrange, Act
      int result = NPC.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
    }

  }
}
