using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
namespace file_handling
{
    class Task
    {
        private Int32 __id;
        public Int32 ID
        {
            get { return __id; }
            set { __id = value; }
        }
        private String __path;
        public String Path
        {
            get { return __path; }
            set { __path = value; }
        }
        private Byte __type;
        public Byte Type
        {
            get { return __type; }
            set
            {
                if (value < 1 || value > 3)
                    { __type = 1; }
                else
                    { __type = value; }
            }
        }
        private Byte __status;
        public Byte Status
        {
            get { return __status; }
            set
            {
                if (value == 1)
                    { __status = value; }
                else
                    { __status = 2; }
            }
        }
        private DateTime __dateTime;
        public DateTime HistoryDateTime
        {
            get { return __dateTime; }
            set { __dateTime = value; }
        }
        private String __value;
        public String Value
        {
            get { return __value; }
            set { __value = value; }
        }
        private Int32 __thread_id;
        public Int32 ThreadID
        {
            get { return __thread_id; }
            set
            {
                if (value < 0 || value >= 1024)
                    { __thread_id = 0; }
                else
                    { __thread_id = value;  }
            }
        }
        private Boolean __complete;
        public Boolean Complete
        {
            get { return __complete; }
        }
        private Boolean __high_priority;
        public Boolean HighPriority
        {
            get { return __high_priority; }
            set { __high_priority = value; }
        }
        public Task()
        {
            __complete = false;
        }
        public void Finish()
        {
            __complete = true;
        }
    }
}
