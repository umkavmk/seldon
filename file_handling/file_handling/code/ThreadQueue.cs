using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace file_handling.code
{
    class ThreadQueue
    {
        private ThreadHandling[] threads;
        private Boolean Running;
        private Queue<Task> qTasksLowPriority;
        private Queue<Task> qTasksHighPriority;
        private List<Task> tasksProcessing;
        private Thread threadQueue;

        private void __Start()
        {
            while (Running)
            {
                for (int i = 0; i < threads.Length; i++)
                {
                    if (threads[i] != null)
                    {
                        if (threads[i].isComplete())
                        {
                            threads[i] = null;
                            Console.Write("Thread " + (i + 1).ToString() + " is free\n");
                        }
                    }
                    if (threads[i] == null)
                    {
                        Task tmp;
                        if(qTasksHighPriority.Count > 0)
                        {
                            tmp = qTasksHighPriority.Dequeue();
                            Console.Write("Thread " + (i + 1).ToString()
                                + " running task ID=" + tmp.ID + " (high priority)\n");
                        }
                        else if (qTasksLowPriority.Count > 0)
                        {
                            tmp = qTasksLowPriority.Dequeue();
                            Console.Write("Thread " + (i + 1).ToString()
                                + " running task ID=" + tmp.ID + " (low priority)\n");
                        }
                        else
                            { tmp = null; }

                        if (tmp != null)
                            { threads[i] = new ThreadHandling(tmp, i + 1); }
                    }
                }
                Thread.Sleep(1000);
            }
        }
        public void Start()
        {
            Running = true;
            threadQueue.Start();
        }
        public void Stop()
        {
            Running = false;
        }
        public Boolean isExistTask(Task task)
        {
            for (int i = 0; i < tasksProcessing.Count; i++)
            {
                if (tasksProcessing[i].ID == task.ID)
                { return true; }
            }
            return false;
        }
        public void EnqueueHighPriorityTask(Task task)
        {
            tasksProcessing.Add(task);
            qTasksHighPriority.Enqueue(task);
        }
        public void EnqueueLowPriorityTask(Task task)
        {
            tasksProcessing.Add(task);
            qTasksLowPriority.Enqueue(task);
        }
        public List<Task> GetFinishedTasks()
        {
            List<Task> finishedTasks = new List<Task>();
            for (int i = 0; i < tasksProcessing.Count; i++)
            {
                if (tasksProcessing[i].Complete)
                { finishedTasks.Add(tasksProcessing[i]); }
            }
            return finishedTasks;
        }
        public void DeleteFinishedTasks(List<Task> finishedTasks)
        {
            for (int i = 0; i < finishedTasks.Count; i++)
            { tasksProcessing.Remove(finishedTasks[i]); }
        }
        public ThreadQueue(Int32 numThreads)
        {
            Running = false;
            qTasksLowPriority = new Queue<Task>();
            qTasksHighPriority = new Queue<Task>();
            tasksProcessing = new List<Task>();
            threads = new ThreadHandling[numThreads];
            threadQueue = new Thread(__Start);
        }
        ~ThreadQueue()
        {
            qTasksLowPriority = null;
            qTasksHighPriority = null;
            tasksProcessing = null;

            for (int i = 0; i < threads.Length; i++)
            { threads[i] = null; }
            threads = null;

            try { threadQueue.Abort(); }
            catch { }
            threadQueue = null;
        }
    }
}
