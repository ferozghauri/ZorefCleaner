using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

namespace ZorefCleaner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static string Chosen_Directory;
        public List<string> All_Directories = new List<string>();
        public List<string> All_Files = new List<string>();
        public static List<string> Duplicate_Files = new List<string>();
        public HashSet<string> File_hashes = new HashSet<string>();
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog getDirectory = new FolderBrowserDialog();
            getDirectory.Description = "Choose yo path ";
            if (getDirectory.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("you have selected: " + getDirectory.SelectedPath);
                Chosen_Directory = getDirectory.SelectedPath;
                All_Directories.Add(Chosen_Directory);
            }
            get_directory_list(Chosen_Directory);
            progressBar1.Maximum = All_Directories.Count();
            progressBar1.Step = 1;
            File.WriteAllLines(@"C:\Users\muhammad.feroz\Desktop\directories.txt", All_Directories,Encoding.UTF8);
            foreach(var all_dir in All_Directories)
            {
                progressBar1.PerformStep();
                label1.Text = all_dir;
                var all_files = Directory.GetFiles(all_dir);
                foreach(var file in all_files)
                {
                    All_Files.Add(file);
                    if (!File_hashes.Add(CalculateMD5(file)))
                    {
                        //File.Delete(file);
                        Duplicate_Files.Add(file);
                        //if (!Duplicate_Files.Add(file))
                        //{
                        //    File.Move(file,file+"")
                        //}
                    }
                }
            }
            DuplicatePrompt dp = new DuplicatePrompt();
            if (Duplicate_Files.Count() != 0)
            {
                dp.ShowDialog();
            }
            else
            {
                MessageBox.Show("No duplicates found");
            }
            label1.Text = "Complete";
            File.WriteAllLines(@"C:\Users\muhammad.feroz\Desktop\files.txt", All_Files, Encoding.UTF8);
            File.WriteAllLines(@"C:\Users\muhammad.feroz\Desktop\hashes.txt", File_hashes, Encoding.UTF8);



        }
        public List<string> check_sub_directories(string path)
        {
            List<string> sub_dir = new List<string>(Directory.GetDirectories(path));
            return sub_dir;
        }
        public void get_directory_list(string path)
        {
            if (check_sub_directories(path).Count() != 0)
            {
                var sub_directories = check_sub_directories(path);
                foreach (string sd in sub_directories)
                {
                    //MessageBox.Show(sd);
                    get_directory_list(sd);
                    All_Directories.Add(sd);
                }

            }
            else
            {
                //MessageBox.Show("has no sub dir");
            }
        }
        public string CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    //return BitConverter.ToString(hash);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
    }
}
