using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Folder_Sorter
{
    class cls_FileToSort
    {
        public DateTime timeAdded { get; set; }
        public string path { get; set; }
        public string name { get; set; }
        public int timeToLive { get; set; } //based and set upon the matching filter - in minutes
        public DateTime timeMoved { get; set; }

        public cls_FileToSort(DateTime timeAdded, string path)
        {
            this.timeAdded = timeAdded;
            this.path = path;
            this.name = Path.GetFileName(path);
        }

        public cls_FileToSort(DateTime timeAdded, string path, string name, int timeToLive)
        {
            this.timeAdded = timeAdded;
            this.path = path;
            this.name = name;
            this.timeToLive = timeToLive;
        }

        //returns whether the file exceeded it's TTL yet or not.
        public bool ShouldBeMoved()
        {
            TimeSpan span = DateTime.Now.Subtract(this.timeAdded);
            if (span.Minutes < timeToLive)
                return false;
            else
                return true;          
        }
    }
}
