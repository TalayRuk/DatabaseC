using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ToDoList
{
  public class ToDoTest : IDisposable
  {
    public ToDoTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=todo_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Task.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_REturnsTrueIfDescriptionAreSame()
    {
      //Arrange,Act
      Task firstTask = new Task("Mow my cat");
      Task secondTask = new Task("Mow my cat");

      //Assert
      Assert.Equal(firstTask, secondTask);
    }

    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      // Arrange
      Task testTask = new Task("Mow my cat");

      // Act
      testTask.Save();
      List<Task> result = Task.GetAll();
      List<Task> testList = new List<Task>{testTask};

      // Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToObject()
    {
      // Arrange
      Task testTask = new Task("Mow my cat");
      // Act
      testTask.Save();
      Task savedTask = Task.GetAll()[0];
      // =
      // List<Task> allTasks = Task.GetAll();
      // Task myTask = allTasks[0];
      // Array [0] b/c List<Task> is an array.
      // Get [0] in this case meant get the whole row of that first id index
      // which include description and other columns(if there's more)

      int result = savedTask.GetId();
    }

    [Fact]
    public void Test_Find_FindsTaskInDatabase()
    {
      // Arrange
      Task testTask = new Task("Mow my cat");
      testTask.Save();

      // Act
      Task foundTask = Task.Find(testTask.GetId());

      // Assert
      Assert.Equal(testTask, foundTask);
    }


    // To run the 1st empty test -don't need Dispose() & :IDisposable @public class ToDoTest()
    public void Dispose()
    {
      Task.DeleteAll();
    }
  }
}
