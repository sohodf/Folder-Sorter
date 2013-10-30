using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
//each file added to the system creates an object that will be managed.
//
namespace Folder_Sorter
{
    public class cls_FileToSort
    {
        public DateTime timeAdded { get; set; }
        public string path { get; set; }
        public string name { get; set; }
        public long timeToLive { get; set; } //based and set upon the matching filter - in minutes
        public DateTime timeMoved { get; set; } 
        //Basic constructor for a file object. these fields are mandatory.
        public cls_FileToSort(DateTime timeAdded, string path)
        {
            this.timeAdded = timeAdded;
            this.path = path;
            this.name = Path.GetFileName(path);
        }
        //3 part constructor for a file object
        public cls_FileToSort(DateTime timeAdded, string path, string name)
        {
            this.timeAdded = timeAdded;
            this.path = path;
            this.name = name;
        }

        //full constructor for a file object
        public cls_FileToSort(DateTime timeAdded, string path, string name, long timeToLive)
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

        //checks if the file still exists in the source directory
        public bool FileStillExists()
        {
            if (File.Exists(this.path + this.name))
                return true;
            return false;
        }


    }
}
