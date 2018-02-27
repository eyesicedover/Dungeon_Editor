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

    [TestMethod]
    public void Equals_TrueForSameDescription_NPC()
    {
      //Arrange, Act
      NPC firstNPC = new NPC("Orc");
      NPC secondNPC = new NPC("Orc");

      //Assert
      Assert.AreEqual(firstNPC, secondNPC);
    }

    [TestMethod]
    public void Save_NPCSavesToDatabase_NPCList()
    {
      //Arrange
      NPC testNPC = new NPC("Orc");
      testNPC.Save();

      //Act
      List<NPC> result = NPC.GetAll();
      List<NPC> testList = new List<NPC>{testNPC};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

  }
}
