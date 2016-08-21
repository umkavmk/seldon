using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using file_handling.code;

namespace file_handling
{
    class Program
    {
        private const String PATH_FILE_CONFIG = "config.cfg";
        private const String DEFAULT_DB_PATH = "my_database.accdb";
        private const Int32 DEFAULT_MAX_THREADS = 10;

        private class ConfigParameters{
            public Int32 MaxThreads;
            public String DBPath;
            public ConfigParameters()
            {
                MaxThreads = DEFAULT_MAX_THREADS;
                DBPath = DEFAULT_DB_PATH;
            }
        }

        static void ReadCfgFile(ConfigParameters cfgParameters)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try { xmlDoc.Load(PATH_FILE_CONFIG); }
            catch { return; }

            foreach (XmlNode xmlNode in xmlDoc.DocumentElement)
            {
                if (xmlNode.LocalName == "MaxThreads")
                {
                    try
                    {
                        Convert.ToInt32(xmlNode.FirstChild.Value);
                        cfgParameters.MaxThreads = Convert.ToInt32(xmlNode.FirstChild.Value);
                    } catch { }
                }
                else if (xmlNode.LocalName == "PathDB")
                {
                    try { cfgParameters.DBPath = xmlNode.FirstChild.Value; }
                    catch { }
                }
            }
        }
        static void Main(string[] args)
        {
            Console.Write("Reading config file...");
            ConfigParameters cfgParameters = new ConfigParameters();
            ReadCfgFile(cfgParameters);

            Console.Write("\nReading data base... ");
            DataBase db = DataBase.Open(cfgParameters.DBPath);
            if(db != null)
            {
                Console.Write("Succes!");
            }
            else
            {
                Console.Write("Error!");
                return;
            }

            Boolean isOk = true;
            ThreadQueue threadQueue = new ThreadQueue(cfgParameters.MaxThreads);
            //запускаем поток, который исполняет задачи в соответствии с их приоритетами
            threadQueue.Start();

            Console.Write("\n\n");
            while(isOk)
            {
                //получаем новые задачи из БД
                List<Task> newTasks = db.GetTasks();
                //добавляем полученные задачи в очередь
                for (int i = 0; i < newTasks.Count; i++)
                {
                    if (!threadQueue.isExistTask(newTasks[i]))
                    {
                        if (!newTasks[i].HighPriority)
                        { threadQueue.EnqueueLowPriorityTask(newTasks[i]); }
                        else
                        { threadQueue.EnqueueHighPriorityTask(newTasks[i]); }
                    }
                }
                System.Threading.Thread.Sleep(10000);

                List<Task> finishedTasks = threadQueue.GetFinishedTasks();
                db.WriteResult(finishedTasks);
                threadQueue.DeleteFinishedTasks(finishedTasks);
            }
            threadQueue.Stop();
        }
    }
}
