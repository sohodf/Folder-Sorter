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

        public MainWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //on form load, try to read all filters. if none exist, create empty filter file.
        private void Form1_Load(object sender, EventArgs e)
        {

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
            listBox1.Items.Add(textToLog);
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //clears selected filter
        }

    }
}
