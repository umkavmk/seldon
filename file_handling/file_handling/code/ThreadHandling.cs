using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
namespace file_handling
{
    class ThreadHandling
    {
        Thread thread;
        Task task;

        private void GetLengthOfFile()
        {
            try
            {
                if (!File.Exists(task.Path))
                    { throw new IOException(); }
                 FileInfo info = new FileInfo(task.Path);
                 task.Value = info.Length.ToString();
                 task.Status = 1;
            }
            catch { task.Status = 2; }

            task.HistoryDateTime = DateTime.Now;
            task.Finish();
        }
        private void GetCreationTimeOfFile()
        {
            try
            {
                if(!File.Exists(task.Path))
                    { throw new IOException(); }
                FileInfo info = new FileInfo(task.Path);
                task.Value = info.CreationTime.ToString();
                task.Status = 1;
            }
            catch { task.Status = 2; }

            task.HistoryDateTime = DateTime.Now;
            task.Finish();
        }
        private void GetLastWriteTimeOfFile()
        {
            try
            {
                if (!File.Exists(task.Path))
                    { throw new IOException(); }
                FileInfo info = new FileInfo(task.Path);
                task.Value = info.LastWriteTime.ToString();
                task.Status = 1;
            }
            catch { task.Status = 2; }

            task.HistoryDateTime = DateTime.Now;
            task.Finish();
        }
        public ThreadHandling(Task task, Int32 id)
        {
            this.task = task;
            task.ThreadID = id;

            ThreadStart action;
            switch (task.Type)
            {
                case 2:
                    {
                        action = new ThreadStart(this.GetCreationTimeOfFile);
                        break;
                    }
                case 3:
                    {
                        action = new ThreadStart(this.GetLastWriteTimeOfFile);
                        break;
                    }
                default:
                    {
                        action = new ThreadStart(this.GetLengthOfFile);
                        break;
                    }
            }
            thread = new Thread(action);
            thread.Start();
        }
        public Boolean isComplete()
        {
            return task.Complete;
        }
        ~ThreadHandling()
        {
            try { thread.Abort(); }
            catch { }
            thread = null;
        }
    }
}
