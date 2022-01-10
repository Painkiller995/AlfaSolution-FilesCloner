using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FilesCloner.Core
{
    class DirectoryManager
    {
        public string DirectoryFinder(string Title = "Please Select directory:" , Boolean NewFolderBTN = false)
        {
            try {
            FolderBrowserDialog DirectorySelector = new FolderBrowserDialog();
            DirectorySelector.Description = Title;
            DirectorySelector.ShowNewFolderButton = NewFolderBTN;
            DirectorySelector.ShowDialog();
            return DirectorySelector.SelectedPath;
            }
            catch
            {
                MessageBox.Show("Error while trying to find the directory");
                return string.Empty;
            }
        }

        public List<string> SubFoldersList(string Source)
        {
            List<string> Sub = new List<string>();
            try
            {
                if (Directory.Exists(Source)) { 
                string[] dirs = Directory.GetDirectories(Source);
                foreach (string dir in dirs)
                {
                    var d = new DirectoryInfo(dir);
                    Sub.Add(d.Name);
                }
                }
                else
                {
                    // MessageBox.Show(String.Format("The Directory {0} is not exist.", Source));
                    Console.WriteLine(String.Format("The Directory {0} is not exist.", Source));
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
            return Sub;
        }

    }

}
