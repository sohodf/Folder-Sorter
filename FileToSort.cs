using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Folder_Sorter
{
    class FileToSort
    {
        public DateTime timeAdded { get; set; }
        public string path { get; set; }
        public string name { get; set; }
        public int timeToLive { get; set; } //based and set upon the matching filter - in minutes
        public DateTime timeMoved { get; set; }

        public FileToSort(DateTime timeAdded, string path)
        {
            this.timeAdded = timeAdded;
            this.path = path;
            this.name = Path.GetFileName(path);
        }
    }
}
