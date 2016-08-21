using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace file_handling.code
{
    class DataBase
    {
        private OleDbConnection connection;

        private DataBase(OleDbConnection connection)
        {
            this.connection = connection;
        }
        ~DataBase()
        {
            try { connection.Close(); }
            catch { }
        }
        static public DataBase Open(String path)
        {
            DataBase newDB = null;
            OleDbConnection connection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source="
    + path + "; Persist Security Info=False;");

            try { connection.Open(); }
            catch { }

            if (connection.State == ConnectionState.Open)
            { newDB = new DataBase(connection); }
            return newDB;
        }
        public List<Task> GetTasks()
        {
            String Sql = "SELECT TaskTable.ID, TaskTable.Path, TaskTable.TaskID, StatusTable.StatusID "
                + "FROM TaskTable LEFT JOIN StatusTable ON taskTable.ID=StatusTable.TaskID "
                + "WHERE (StatusTable.TaskID IS NULL OR StatusTable.StatusID=2) "
                + "GROUP BY TaskTable.ID, TaskTable.Path, TaskTable.TaskID, StatusTable.StatusID "
                + "HAVING count(*)<10";
            OleDbCommand command = new OleDbCommand(Sql, connection);

            OleDbDataReader dataReader = command.ExecuteReader();

            List<Task> tasks = new List<Task>();
            while(dataReader.Read())
            {
                Task tmpTask = new Task();
                try
                {
                    Convert.ToInt32(dataReader["ID"]);
                    Convert.ToString(dataReader["Path"]);
                    Convert.ToByte(dataReader["TaskID"]);
                    Convert.ToString(dataReader["StatusID"]);

                    tmpTask.ID = Convert.ToInt32(dataReader["ID"]);
                    tmpTask.Path = Convert.ToString(dataReader["Path"]);
                    tmpTask.Type = Convert.ToByte(dataReader["TaskID"]);
                    
                    String tmpStr = Convert.ToString(dataReader["StatusID"]);
                    if (tmpStr == "2")
                        { tmpTask.HighPriority = false; }
                    else
                        { tmpTask.HighPriority = true; }

                    tasks.Add(tmpTask);
                }
                catch { continue; }
            }
            dataReader.Close();

            return tasks;
        }
        private void WriteInStatusTable(List<Task> tasks)
        {
            String Sql;
            OleDbCommand command;

            Sql = "SELECT TOP 1 ID FROM StatusTable ORDER BY ID DESC";
            command = new OleDbCommand(Sql, connection);

            OleDbDataReader dataReader = command.ExecuteReader();
            Int32 id_last;
            if (!dataReader.Read())
            { id_last = 0; }
            else
            {
                try
                {
                    Convert.ToInt32(dataReader["ID"]);
                    id_last = Convert.ToInt32(dataReader["ID"]);
                } catch { id_last = 0; }
            }

            for (int i = 0; i < tasks.Count; i++)
            {
                Sql = "INSERT INTO StatusTable VALUES (" + (id_last + i + 1).ToString() + ", " +
                    tasks[i].ID + ", " + tasks[i].Status + ", \"" + tasks[i].HistoryDateTime.ToString() + "\")";
                command = new OleDbCommand(Sql, connection);
                command.ExecuteNonQuery();
            }
            dataReader.Close();
        }
        private void WriteInResultTable(List<Task> tasks)
        {
            String Sql;
            OleDbCommand command;

            Sql = "SELECT TOP 1 ID FROM ResultTable ORDER BY ID DESC";
            command = new OleDbCommand(Sql, connection);

            OleDbDataReader dataReader = command.ExecuteReader();
            Int32 id_last;
            if (!dataReader.Read())
            { id_last = 0; }
            else
            {
                try
                {
                    Convert.ToInt32(dataReader["ID"]);
                    id_last = Convert.ToInt32(dataReader["ID"]);
                }
                catch { id_last = 0; }
            }

            for (int i = 0; i < tasks.Count; i++)
            {
                if (tasks[i].Status == 1)
                {
                    Sql = "INSERT INTO ResultTable VALUES (" + (id_last + i + 1).ToString() + ", " +
                        tasks[i].ID + ", \"" + tasks[i].Value + "\", " + tasks[i].ThreadID + ")";
                    command = new OleDbCommand(Sql, connection);
                    command.ExecuteNonQuery();
                }
            }
            dataReader.Close();
        }
        public void WriteResult(List<Task> tasks)
        {
            WriteInStatusTable(tasks);
            WriteInResultTable(tasks);
        }
    }
}
