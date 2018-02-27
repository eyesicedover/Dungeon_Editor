using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
// using MySQLCore.Models;
using System;
using Dungeon.Models;
using Dungeon;

namespace Dungeon.Tests
{
  [TestClass]
  public class PCTest : IDisposable
  {
    public PCTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=dungeon_test;";
    }
    public void Dispose()
    {
      PC.DeleteAll();
      // PC.DeleteAll();
    }

    [TestMethod]
    public void GetAll_DatabaseEmptyAtFirst_0()
    {
      //Arrange, Act
      int result = PC.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Equals_TrueForSameDescription_PC()
    {
      //Arrange, Act
      PC firstPC = new PC("Crom");
      PC secondPC = new PC("Crom");

      //Assert
      Assert.AreEqual(firstPC, secondPC);
    }

    [TestMethod]
    public void Save_PCSavesToDatabase_PCList()
    {
      //Arrange
      PC testPC = new PC("Crom");
      testPC.Save();

      //Act
      List<PC> result = PC.GetAll();
      List<PC> testList = new List<PC>{testPC};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

  }
}
