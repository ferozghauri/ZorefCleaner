using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZorefCleaner
{
    public partial class DuplicatePrompt : Form
    {
        public DuplicatePrompt()
        {
            InitializeComponent();
            label2.Text = Form1.Duplicate_Files.Count().ToString() + " Duplicate files Found";
        }
        Random random = new Random();
        private void Delete_Button(object sender, EventArgs e)
        {
            Form1.Duplicate_Files.ForEach(File.Delete);
            this.Close();
        }

        private void Move_Files_Button(object sender, EventArgs e)
        {
           
            FolderBrowserDialog duplicate_folder_Path = new FolderBrowserDialog();
            if (duplicate_folder_Path.ShowDialog() == DialogResult.OK)
            {
                foreach (var d_f in Form1.Duplicate_Files)
                {
                    try
                    {
                        File.Move(d_f, duplicate_folder_Path.SelectedPath + "\\" + Path.GetFileName(d_f));
                    }
                    catch
                    {
                        File.Move(d_f, duplicate_folder_Path.SelectedPath + "\\" + random.Next(1000, 99999) + Path.GetFileName(d_f));
                    }
                }
            }
            
            this.Close();
        }
    }
}
