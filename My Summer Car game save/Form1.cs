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
using System.Xml.Serialization;

namespace My_Summer_Car_game_save
{
    public partial class Form1 : Form
    {

        public string dirA;
        public string dirB;
        public string[] files;

        public List<DirectoryInfo> allSaveDirs;

        public string destPath;
        public string sourcePath;
        public string saveFileName;
        public string currentPath;
        public string savesDir;
        public string newSaveDir;

        public ListView savesList;

        private bool _dragging = false;
        private Point _offset;
        private Point _start_point = new Point(0,0);

        public Form1()
        {
            InitializeComponent();
           
            savesList = listView1;
            string currentPath = Directory.GetCurrentDirectory();
            if (!Directory.Exists(Path.Combine(currentPath, "Saves")))
            {
                Directory.CreateDirectory(Path.Combine(currentPath, "Saves"));
            }
            savesDir = currentPath + "\\Saves";

            string path = "C:\\Users\\" + Environment.UserName + "\\AppData\\LocalLow\\Amistech\\My Summer Car";
            if (Directory.Exists(path))
            {
                dirA = path;
                files = System.IO.Directory.GetFiles(dirA);
                button3.BackColor = Color.FromArgb(192, 255, 192);
            }

            // savesList.ListViewItemSorter = new IntegerComparer(1);
            // savesList.Sort();
            int ID = 0;

            /* while (Directory.Exists(Path.Combine(savesDir, "Save" + ID)))
             {
                 string finalSaveDir = Path.Combine(savesDir, "Save" + ID);

                 Directory.CreateDirectory(finalSaveDir);

                 DirectoryInfo dirInfo = new System.IO.DirectoryInfo(finalSaveDir);
                 */

           

            var directories = GetDirectories(savesDir);
            DirectoryInfo dirInfo;

            for (int i = 0; i < directories.Count(); i++)
            {
                dirInfo = new System.IO.DirectoryInfo(directories[i]);
                // add to list
                ListViewItem lvi = new ListViewItem(dirInfo.Name.ToString());
                lvi.SubItems.Add(dirInfo.LastAccessTime.ToString());
                savesList.Items.Add(lvi);
                //ID++;
            }
            //}



            savesList.ListViewItemSorter = new ListViewItemComparer(1, SortOrder.Descending);
            savesList.Sort();

        }

       


        //< Select the old save
        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            string path = "C:\\Users\\" + Environment.UserName + "\\AppData\\LocalLow\\Amistech\\My Summer Car";
            if (Directory.Exists(path))
            {
                fbd.SelectedPath = path;
            }
            else
            {
                fbd.SelectedPath = @"C:\";
            }

            if (fbd.ShowDialog() == DialogResult.OK)
            {

                dirA = fbd.SelectedPath;
                files = System.IO.Directory.GetFiles(dirA);
                //Array.Clear(files, 0, files.Length);

            }
           



        }

       /* //< Select the new save folder
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd2 = new FolderBrowserDialog();
            
                fbd2.SelectedPath = @"C:\";
            

            if (fbd2.ShowDialog() == DialogResult.OK)
            {

                dirB = fbd2.SelectedPath;
              

            }
            dirB = fbd2.SelectedPath;
        }*/

        //< Backup your game button
        private void button3_Click(object sender, EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(dirA);



            if (!dir.Exists)
            {
                MessageBox.Show("Select an appropriate folder of your saved game AppData first!", "Error");
            }
            else
            {

                string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Please name your save...", "Save Name?", "Save Name");
                /*  if (!Directory.Exists(dirB)) {
                      Directory.CreateDirectory(dirB);
                  }*/

                newSaveDir = savesDir;
                int ID = 0;
                string finalSaveDir;
                if (Directory.Exists(Path.Combine(savesDir, UserAnswer)))
                {
                    MessageBox.Show("Save with the same name already exists!", "Error");
                }
                else
                {
                    if (String.IsNullOrEmpty(UserAnswer))
                    {
                        while (Directory.Exists(Path.Combine(savesDir, "Save" + ID)))
                        {
                            ID++;
                        }
                        finalSaveDir = Path.Combine(savesDir, "Save" + ID);
                    }
                    else
                    {      
                        finalSaveDir = Path.Combine(savesDir, UserAnswer);
                    }
                    Directory.CreateDirectory(finalSaveDir);

                    DirectoryInfo dirInfo = new System.IO.DirectoryInfo(finalSaveDir);

                    // add to list
                    ListViewItem lvi = new ListViewItem(dirInfo.Name.ToString());
                    lvi.SubItems.Add(dirInfo.LastAccessTime.ToString());
                    savesList.Items.Add(lvi);

                    FileInfo[] allFiles = dir.GetFiles();

                    /* try
                     {
                         Information info = new Information();
                         info.FoldersData = allFiles;
                         SaveXML.SaveData(info, "data.xml");
                     }
                     catch (Exception ex)
                     {
                         MessageBox.Show(ex.Message);
                     }*/


                    foreach (FileInfo file in allFiles)
                    {


                        string pathTo = Path.Combine(finalSaveDir, file.Name);
                        string pathFrom = Path.Combine(dirA, file.Name);



                        if (File.Exists(pathTo))
                        {
                            File.Delete(pathTo);
                        }


                        FileInfo Sourcefile = new FileInfo(pathFrom);
                        Sourcefile.CopyTo(pathTo);


                    }
                    MessageBox.Show("Your game saved successfully!", "Success");

                }
            }
        }

        //< Load game button
        private void button4_Click(object sender, EventArgs e)
        {
            string item = "";
            if (savesList.SelectedItems.Count > 0)
            {
                item = savesList.SelectedItems[0].Text;
                //rest of your logic
            }
            else {
                MessageBox.Show("Select a save first!", "Error");
            }

            DirectoryInfo dir = new DirectoryInfo(Path.Combine(savesDir, item));
            string finalSaveDir = Path.Combine(savesDir, item);


            if (!dir.Exists)
            {
                MessageBox.Show("You dont have any My Summer Car Saves to backup!", "Error");
            }
            else
            {

                /* if (!Directory.Exists(dirB))
                 {
                     Directory.CreateDirectory(dirB);
                 }*/





                FileInfo[] allFiles = dir.GetFiles();

                foreach (FileInfo file in allFiles)
                {


                    string pathTo = Path.Combine(dirA, file.Name);
                    string pathFrom = Path.Combine(finalSaveDir, file.Name);



                    if (File.Exists(pathTo))
                    {
                        File.Delete(pathTo);
                    }


                    FileInfo Sourcefile = new FileInfo(pathFrom);
                    Sourcefile.CopyTo(pathTo);
                }
                MessageBox.Show("Your game loaded successfully!", "Success");
            }
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
           
        }


        //< Delete save button
        private void button1_Click(object sender, EventArgs e)
        {
            string item;
          
            if (savesList.SelectedItems.Count > 0 )
            {
                item = savesList.SelectedItems[0].Text;
                DirectoryInfo dir = new DirectoryInfo(Path.Combine(savesDir, item));
                //rest of your logic

                //DirectoryInfo dir = new DirectoryInfo(savesDir + "\\Save" + item);
                string finalSaveDir = Path.Combine(savesDir, item);


                if (!dir.Exists)
                {
                    MessageBox.Show("The directory you want to delete doesn't exist!", "Error");
                }


                DialogResult result1 = MessageBox.Show("Are you sure you want to delete " + item + "?",
        "Confirmation",
        MessageBoxButtons.YesNoCancel,
        MessageBoxIcon.Question,
        MessageBoxDefaultButton.Button2);

                if (result1 == DialogResult.Yes)
                {
                    dir.Delete(true);
                    savesList.Items.Clear();

                    var directories = GetDirectories(savesDir);
                    DirectoryInfo dirInfo;

                    for (int i = 0; i < directories.Count(); i++)
                    {
                        dirInfo = new System.IO.DirectoryInfo(directories[i]);
                        // add to list
                        ListViewItem lvi = new ListViewItem(dirInfo.Name.ToString());
                        lvi.SubItems.Add(dirInfo.LastAccessTime.ToString());
                        savesList.Items.Add(lvi);
                        //ID++;
                    }



                    savesList.ListViewItemSorter = new ListViewItemComparer(1, SortOrder.Descending);
                    savesList.Sort();
                }

            }
            else
            {
                MessageBox.Show("Select a save first!", "Error");
            }

            

        }

       


         List<string> GetDirectories(string path)
        {
            try
            {
                return Directory.GetDirectories(path).ToList();
            }
            catch (UnauthorizedAccessException)
            {
                return new List<string>();
            }
        }

       

        private void label5_Click(object sender, EventArgs e)
        {
            Application.Exit();
            
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            _dragging = true;
            _start_point = new Point(e.X, e.Y);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragging) {
                Point point = PointToScreen(e.Location);
                Location = new Point(point.X - this._start_point.X, point.Y - this._start_point.Y);
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            _dragging = false;
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            _dragging = true;
            _start_point = new Point(e.X, e.Y);
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragging)
            {
                Point point = PointToScreen(e.Location);
                Location = new Point(point.X - this._start_point.X, point.Y - this._start_point.Y);
            }
        }

        private void panel2_MouseUp(object sender, MouseEventArgs e)
        {
            _dragging = false;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
            {
                return;
            }
            else
            {
                button4.BackColor = Color.FromArgb(192, 255, 192);
                button1.BackColor = Color.FromArgb(192, 255, 192);
            }
        }
    }
}
