using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
// using MySQLCore.Models;
using System;
using Dungeon.Models;
using Dungeon;

namespace Dungeon.Tests
{
  [TestClass]
  public class RoomTest : IDisposable
  {
    public RoomTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=dungeon_test;";
    }
    public void Dispose()
    {
      Room.DeleteAll();
      // Room.DeleteAll();
    }

    [TestMethod]
    public void GetAll_DatabaseEmptyAtFirst_0()
    {
      //Arrange, Act
      int result = Room.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Equals_TrueForSameDescription_Room()
    {
      //Arrange, Act
      Room firstRoom = new Room("Entryway");
      Room secondRoom = new Room("Entryway");

      //Assert
      Assert.AreEqual(firstRoom, secondRoom);
    }

    [TestMethod]
    public void Save_RoomSavesToDatabase_RoomList()
    {
      //Arrange
      Room testRoom = new Room("Entryway");
      testRoom.Save();

      //Act
      List<Room> result = Room.GetAll();
      List<Room> testList = new List<Room>{testRoom};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

  }
}
