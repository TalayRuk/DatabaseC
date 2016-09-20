using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using ToDoList.Database;

namespace ToDoList
{
  public class Task
  {
    private int _id;
    private string _description;

    public Task(string Description, int Id = 0)
    {
      _id = Id;
      _description = Description;
    }

    // write public override after write the test
    public override bool Equals(System.Object otherTask)
    {
      if (!(otherTask is Task))
      {
        return false;
      }
      else
      {
        Task newTask = (Task) otherTask; //not making new task but tell comp a copy of the task from otherTask
        //this way the computer don't think that it's exactly the same exact copy
        bool idEquality = (this.GetId() == newTask.GetId());
        bool descriptionEquality = (this.GetDescription() == newTask.GetDescription());
        return (idEquality && descriptionEquality);
      }
    }

    public override int GetHashCode()
    {
         return this.GetDescription().GetHashCode();
    }

    public int GetId()
    {
      return _id;
    }
    public string GetDescription()
    {
      return _description;
    }
    public void SetDescription(string newDescription)
    {
      _description = newDescription;
    }
    public static List<Task> GetAll()
    {
      List<Task> allTasks = new List<Task>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM tasks;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        // from TaskTest; Task savedTask = Task.GetAll()[0];
        // GetInt32(0) meant to get column index 0 which mean 1st column = id & not the row
        // the GetAll()[0] is get the whole first row which would be 1st id & 1st description
        // DataReader provides a series of methods that allow you
        // to access column values in their native data types
        // (GetDateTime, GetDouble, GetGuid, GetInt32,
        // and see https://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqldatareader(v=vs.110).aspx)
        //GetString(1) meant as id = column index 0, description = column index 1
        //GetString(1) mean get string from column index 1 which = description column = get string from description
        int taskId = rdr.GetInt32(0);
        string taskDescription = rdr.GetString(1);
        Task newTask = new Task(taskDescription, taskId);
        allTasks.Add(newTask);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allTasks;
    }
    // write save method after the override & test in testTask
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO tasks (description) OUTPUT INSERTED.id VALUES (@taskDescription);", conn);
      // add Sql variable
      SqlParameter descriptionParameter = new SqlParameter();
      descriptionParameter.ParameterName = "@taskDescription";
      descriptionParameter.Value = this.GetDescription();
      cmd.Parameters.Add(descriptionParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static Task Find(int id)
  {
    SqlConnection conn = DB.Connection();
    conn.Open();

    SqlCommand cmd = new SqlCommand("SELECT * FROM tasks WHERE id = @TaskId;", conn);
    SqlParameter taskIdParameter = new SqlParameter();
    taskIdParameter.ParameterName = "@TaskId";
    taskIdParameter.Value = id.ToString();
    cmd.Parameters.Add(taskIdParameter);
    SqlDataReader rdr = cmd.ExecuteReader();

    int foundTaskId = 0;
    string foundTaskDescription = null;
    while(rdr.Read())
    {
      foundTaskId = rdr.GetInt32(0);
      foundTaskDescription = rdr.GetString(1);
    }
    Task foundTask = new Task(foundTaskDescription, foundTaskId);

    if (rdr != null)
    {
      rdr.Close();
    }
    if (conn != null)
    {
      conn.Close();
    }

    return foundTask;
  }

  
    // once the Delete method is declare here. Then, the Test will pass w/DeleteAll() in the TaskTest.cs for the first Test
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM tasks;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

  }
}
