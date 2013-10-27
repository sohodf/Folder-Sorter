using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;


namespace Folder_Sorter
{


    public partial class MainWindow : Form
    {
        public string watchingDir { get; set; }
        public string targetDir { get; set; }
        public ArrayList filters = new ArrayList();
        public ArrayList files = new ArrayList();
        //path for filters file
        public static string filterPath = Application.StartupPath + "\\FilterList.csv";
        //path for files file
        public static string filesPath = Application.StartupPath + "\\Files.csv";
        //path for log file
        public string LogFileName = "";
        System.Windows.Forms.Timer folderWatchTimer = new System.Windows.Forms.Timer();
        public static int watchInterval = 5000;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //load both filter and files files upon program start.
        //initialize log file
        private void Form1_Load(object sender, EventArgs e)
        {

            LogFileName = @"Log " + DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss") + ".txt";
            //create log file
            File.Create(Application.StartupPath + LogFileName);
            
            if (File.Exists(filterPath))
            {
                LoadFilters();
            }
            else
                File.Create(filterPath);

            if (File.Exists(filesPath))
            {
                LoadFiles();
            }
            else
                File.Create(filesPath);

        }

        //read file and load filters
        public ArrayList LoadFilters()
        {
            filters.Clear();

            if (File.ReadAllText(filterPath) == "")
            {
                //empty file - do nothing
                return filters;
            }
            else
            {
                //load the filters to an object
                string[] allLines = File.ReadAllLines(filterPath);
                foreach (string line in allLines)
                {
                    string[] items = line.Split(',');
                    cls_Filter filter = new cls_Filter(items[0], items[1], items[2], int.Parse(items[3]));
                    filters.Add(filter);
                }
                Log(filters.Count.ToString() + " filters loaded in total");
                return filters;
            }

        }

        //read file and load active files
        public ArrayList LoadFiles()
        {
            files.Clear();

            if (File.ReadAllText(filesPath) == "")
            {
                //empty file - do nothing
                return files;
            }
            else
            {
                //load the filters to an object
                string[] allLines = File.ReadAllLines(filesPath);
                foreach (string line in allLines)
                {
                    string[] items = line.Split(',');
                    cls_FileToSort file = new cls_FileToSort(DateTime.Parse(items[0]), items[1], items[2], int.Parse(items[3]));
                    files.Add(files);
                }
                Log(files.Count.ToString() + " files loaded in total");
                return files;
            }

        }



        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            if (result == DialogResult.OK)
            {
                label2.Text = fbd.SelectedPath;
                watchingDir = (char)34 + fbd.SelectedPath + (char)34;

            }
       }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            label5.Text = textBox1.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            if (result == DialogResult.OK)
            {
                label7.Text = fbd.SelectedPath;
                targetDir = (char)34 + fbd.SelectedPath + (char)34;

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label10.Text = comboBox1.Text;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (label2.Text.Contains("No folder selected") || (label5.Text == "")
                || label7.Text.Contains("No folder selected") || label10.Text == "")
            {
                Log("Not all fields selected. Filter not Saved");
                Log("Select all fields and try again");
            }
            else
            {
                File.AppendAllText(filterPath, watchingDir + "," + targetDir + "," + label5.Text + "," + comboBox1.Text + Environment.NewLine);
                LoadFilters();
                Log("1 filter added");
                
            }
            
        }

        //adds a line to the log
        public void Log(string textToLog)
        {
            //add the log to the listbox
            listBox1.Items.Add(textToLog);
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
            //write log to file
            File.AppendAllText(Application.StartupPath + "\\" + LogFileName, textToLog + Environment.NewLine);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //clears selected filter
        }

        private void button6_Click(object sender, EventArgs e)
        {
            button6.Enabled = false;
            button7.Enabled = true;
            button1.Enabled = false;

            folderWatchTimer.Interval = watchInterval;
            folderWatchTimer.Tick += new EventHandler(timerTick);
            folderWatchTimer.Enabled = true;

            Log(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " started watching folders - " + filters.Count.ToString() + " filters active");

        }

        //timer ticks on "watchInterval" interval (in ms). if something's happenning - do nothing. else, start the tick.
        void timerTick(object sender, EventArgs e)
        {
            Log("tick");
            if (RunOverFolder.IsBusy)
                return;
            else
                RunOverFolder.RunWorkerAsync();
        }

        //stops the time. if there's an action in the background, it continues until it is finished.
        private void button7_Click(object sender, EventArgs e)
        {
            Log("Stopping...");
            folderWatchTimer.Stop();
            //Wait for the undelying thread to die
            while (RunOverFolder.IsBusy)
            {
                Log("Waiting for all actions to finish");
                System.Threading.Thread.Sleep(5000);
            }
            Log("Actions finished. Stopping watch");

            button1.Enabled = true;
            button7.Enabled = false;
            button6.Enabled = true;

            Log(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " stopped watching folders ");
        }

        //checking the actual folders for new files
        private void RunOverFolder_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke(new Action(() => Log("Checking started at " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"))));
            //iterate over the existing paths and check if the files are already in the files list. 
            //if not, add them with the appropriate TTL according to the matching filter
            foreach (cls_Filter path in filters)
            {
                string[] filePaths = Directory.GetFiles(@path.sourceDir);
                foreach (string curretnPath in filePaths)
                {
                    //check if the file already exists in the files list
                    bool exists = false;
                    foreach (cls_FileToSort f in files)
                    {
                        string fullPath = Path.Combine(f.path, f.name);
                        if (curretnPath == fullPath)
                        {
                            exists = true;
                            break;
                        }
                        
                    }
                    //file does not exist in the lists - new file detected
                    if (!exists)
                    {
                        this.Invoke(new Action(() => Log("New file detected at " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"))));
                        this.Invoke(new Action(() => Log("New file path is: " + curretnPath)));
                        cls_FileToSort newFile = new cls_FileToSort(DateTime.Now, curretnPath, Path.GetFileName(curretnPath));
                        //find the appropriate filter
                        bool filterFound = false;
                        foreach (cls_Filter filter in filters)
                        {
                            //filter found
                            if (filter.CompareFileToFilter(newFile))
                            {
                                filterFound = true;
                                newFile.timeToLive = filter.TTL;
                                files.Add(newFile);
                                break;
                            }
                            //filter not found - do nothing
                            if (!filterFound)
                                this.Invoke(new Action(() => Log("No filter found for file " + curretnPath)));
                        }
                    }

                }
            }
        }

    }
}
