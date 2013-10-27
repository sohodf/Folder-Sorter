using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Folder_Sorter
{
    public class cls_Filter
    {

        public string sourceDir { get; set; }
        public string targetDir { get; set; }
        public string filter { get; set; }
        public Int64 TTL { get; set; }
        
        //class constructor
        public cls_Filter(string sourceDir, string TargetDir, string filter, Int64 TTL)
        {
            this.sourceDir = sourceDir;
            this.targetDir = TargetDir;
            this.filter = filter;
            this.TTL = TTL;
        }

        //checks if the file fits the current filter. return true if it fits.
        public bool CompareFileToFilter(cls_FileToSort file)
        {
            if (Regex.IsMatch(filter, file.name))
                return true;
            return false;
        }

        //Moves the file to the targe directory
        public bool MoveFile(cls_FileToSort file)
        {

        }

       
    }
}
