using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ToDoList
{
  public class ToDoList : IDisposable
  {
    public ToDoTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=todo_test;Integrated Security=SSPI;";
    }
    // add this first test when DataBase is first empty
    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      // Arrange, Act
      int result = Task.GetAll().Count;
      // Assert
      Assert.Equal(0, result);
    }

    public void Dispose()
    {
      Task.DeleteAll();
    }


  }
}
