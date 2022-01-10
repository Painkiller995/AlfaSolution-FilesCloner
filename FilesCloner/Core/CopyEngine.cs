using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FilesCloner.Core
{
    // Welcome to the cony engine :-)
    public partial class CopyEngine  
    {
        ////////////////////////////////////////////////////////////////////////////////
        internal delegate void UpdateProgressDelegate(int ProgressPercentage); //Send Data 
        internal event UpdateProgressDelegate UpdateProgress; //Send Data 
        ////////////////////////////////////////////////////////////////////////////////

        // We need that to find subfolders of subfilders :-)
        DirectoryManager DirectoryManager = new DirectoryManager();
        ////////////////////////////////////////////////////////////////////////////////
        // The BackgroundWorker will be used to perform a long running action
        // on a background thread.  This allows the UI to be free for painting
        // as well as other actions the user may want to perform.  The background
        // thread will use the ReportProgress event to update the ProgressBar
        // on the UI thread.
        private BackgroundWorker m_AsyncWorker = new BackgroundWorker();
        int ProgressPercentageLevel = 0;
        ////////////////////////////////////////////////////////////////////////////////

        ///////////////////////// CopyEngine data provider /////////////////////////////
        private string Engine_SourceDis;
        private string Engine_TargetDis;
        private string Engine_SelectedSubFolder;
        private List<string> Engine_Subfolders;
        private List<string> Engine_ext;
        private List<string> Engine_DelExt;
        private Boolean Engine_IsItMain;
        private Boolean Engine_CloneSubfolders;
        private Boolean Engine_DeleteSource;
        private Boolean Engine_DeleteTarget;
        private Boolean Engine_Overwrite;
        private Boolean Engine_CloneAllFiles;
        private Boolean Engine_DeleteAllFiles;
        ////////////////////////////////////////////////////////////////////////////////

        public CopyEngine()
        {
            // Create a background worker thread that ReportsProgress &
            // SupportsCancellation
            // Hook up the appropriate events.
            m_AsyncWorker.WorkerReportsProgress = true;
            m_AsyncWorker.WorkerSupportsCancellation = true;
            m_AsyncWorker.ProgressChanged += new ProgressChangedEventHandler
                            (bwAsync_ProgressChanged);
            m_AsyncWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler
                            (bwAsync_RunWorkerCompleted);
            m_AsyncWorker.DoWork += new DoWorkEventHandler(bwAsync_DoWork);
        }


        public void EngineDatarovider(string SourceDis , string TargetDis, List<string>  Subfolders, string SelectedSubFolder, Boolean IsItMain = false, Boolean CloneSubfolders = false , List<string> ext = null, List<string> DelExt = null , Boolean CloneAllFiles = false, Boolean DeleteAllFiles = false , Boolean DeleteSource = false, Boolean DeleteTarget = false, Boolean Overwrite = true)
        {
            Engine_SourceDis = SourceDis;
            Engine_TargetDis = TargetDis;
            Engine_Subfolders = Subfolders;
            Engine_IsItMain = IsItMain;
            Engine_CloneSubfolders = CloneSubfolders;
            Engine_ext = ext;
            Engine_DelExt = DelExt;
            Engine_DeleteSource = DeleteSource;
            Engine_DeleteTarget = DeleteTarget;
            Engine_Overwrite = Overwrite;
            Engine_SelectedSubFolder =  SelectedSubFolder;
            Engine_CloneAllFiles = CloneAllFiles;
            Engine_DeleteAllFiles = DeleteAllFiles;
        }

        public string DisChecker(bool IsItMainDis, string SourceorTargetDis)
        {
            string LastDis = string.Empty;
            if (IsItMainDis)
            {
                LastDis = SourceorTargetDis;
                return LastDis;
            }
            else
            {
                LastDis = SourceorTargetDis + @"\" + Engine_SelectedSubFolder;
                return LastDis;
            }
        }

        #region Asynchronous BackgroundWorker Thread

        //public void bnAsync_Click(object sender, EventArgs e)
        public void bnAsync_Click()
        {
            ProgressPercentageLevel = 0;
            UpdateProgress(0);
            // If the background thread is running then clicking this
            // button causes a cancel, otherwise clicking this button
            // launches the background thread.
            if (m_AsyncWorker.IsBusy)
            {
                //bnAsync.Enabled = false;
                //lblStatus.Text = "Cancelling...";

                // Notify the worker thread that a cancel has been requested.
                // The cancel will not actually happen until the thread in the
                // DoWork checks the bwAsync.CancellationPending flag, for this
                // reason we set the label to "Cancelling...", because we haven't
                // actually cancelled yet.
                m_AsyncWorker.CancelAsync();
            }
            else
            {
                //bnAsync.Text = "Cancel";
                //lblStatus.Text = "Running...";

                // Kickoff the worker thread to begin it's DoWork function.
                m_AsyncWorker.RunWorkerAsync();
            }
        }

        private void bwAsync_DoWork(object sender, DoWorkEventArgs e)
        {

            ///////////////////////////////////////////////////////////////////////// 
            ///This will replace Ext List with *.* to copy or/and delete all types of files :-)
            if (Engine_CloneAllFiles)
            {
                string all = "*";
                Engine_ext = new List<string>();
                Engine_ext.Add(all);
                //Engine_DelExt.Add(all);
            }
            if (Engine_DeleteAllFiles)
            {
                string all = "*";
                Engine_DelExt = new List<string>();
                Engine_DelExt.Add(all);
            }
            /////////////////////////////////////////////////////////////////////////

            // The sender is the BackgroundWorker object we need it to
            // report progress and check for cancellation.
            BackgroundWorker bwAsync = sender as BackgroundWorker;

          
            //Delete all target files before cloning 
            bwAsync.ReportProgress(25);
            if (Engine_DeleteTarget)
                {
                    foreach (string enTarext in Engine_DelExt)
                    {
                        FileDeleter(DisChecker(Engine_IsItMain, Engine_TargetDis), "*." + enTarext.ToLower());
                        //Check Canceling Between Missions
                        if (bwAsync.CancellationPending)
                        {
                            // Pause for a bit to demonstrate that there is time between
                            // "Cancelling..." and "Cancel ed".
                            Thread.Sleep(1200);

                            // Set the e.Cancel flag so that the WorkerCompleted event
                            // knows that the process was cancelled.
                            e.Cancel = true;
                            return;
                        }
                    //Check Canceling Between Missions
           
                }
                bwAsync.ReportProgress(35);
                if (Engine_CloneSubfolders)
                {
                    foreach (string folder in DirectoryManager.SubFoldersList(Engine_TargetDis + @"\" + Engine_SelectedSubFolder))
                    {
                        foreach (string enTarext in Engine_DelExt)
                        {
                            FileDeleter(DisChecker(Engine_IsItMain, Engine_TargetDis) + @"\" + folder , "*." + enTarext.ToLower());
                            //Check Canceling Between Missions
                            if (bwAsync.CancellationPending)
                            {
                                // Pause for a bit to demonstrate that there is time between
                                // "Cancelling..." and "Cancel ed".
                                Thread.Sleep(1200);

                                // Set the e.Cancel flag so that the WorkerCompleted event
                                // knows that the process was cancelled.
                                e.Cancel = true;
                                return;
                            }
                            //Check Canceling Between Missions
                        }
                    }
                bwAsync.ReportProgress(45);
                }
                }
                bwAsync.ReportProgress(50);
                
                //Delete all target files before cloning 

                //The Cloning Process
                if (Engine_IsItMain == true)
                {
                    foreach (string EnExt in Engine_ext)
                    {
                        FilesCloner(Engine_SourceDis, Engine_TargetDis, "*." + EnExt.ToLower(), Engine_Overwrite);
                        //Check Canceling Between Missions
                        if (bwAsync.CancellationPending)
                        {
                            // Pause for a bit to demonstrate that there is time between
                            // "Cancelling..." and "Cancel ed".
                            Thread.Sleep(1200);

                            // Set the e.Cancel flag so that the WorkerCompleted event
                            // knows that the process was cancelled.
                            e.Cancel = true;
                            return;
                        }
                        //Check Canceling Between Missions
                    }
                bwAsync.ReportProgress(65);
                if (Engine_CloneSubfolders) {
                        foreach (string folder in Engine_Subfolders)
                        {
                            foreach (string EnExt in Engine_ext)
                            {
                                FilesCloner(Engine_SourceDis + @"\" + folder, Engine_TargetDis + @"\" + folder, "*." + EnExt.ToLower(), Engine_Overwrite);
                                //Check Canceling Between Missions
                                if (bwAsync.CancellationPending)
                                {
                                    // Pause for a bit to demonstrate that there is time between
                                    // "Cancelling..." and "Cancel ed".
                                    Thread.Sleep(1200);

                                    // Set the e.Cancel flag so that the WorkerCompleted event
                                    // knows that the process was cancelled.
                                    e.Cancel = true;
                                    return;
                                }
                                //Check Canceling Between Missions
                            }
                        }
                    }
                }
                else if (Engine_IsItMain == false)
                {
                    if (Engine_SelectedSubFolder is object)
                    {
                        foreach (string EnExt in Engine_ext)
                        {
                            FilesCloner(Engine_SourceDis + @"\" + Engine_SelectedSubFolder, Engine_TargetDis + @"\" + Engine_SelectedSubFolder, "*." + EnExt.ToLower(), Engine_Overwrite);
                            //Check Canceling Between Missions
                            if (bwAsync.CancellationPending)
                            {
                                // Pause for a bit to demonstrate that there is time between
                                // "Cancelling..." and "Cancel ed".
                                Thread.Sleep(1200);

                                // Set the e.Cancel flag so that the WorkerCompleted event
                                // knows that the process was cancelled.
                                e.Cancel = true;
                                return;
                            }
                            //Check Canceling Between Missions
                        }
                        if (Engine_CloneSubfolders) {
                            foreach (string folder in DirectoryManager.SubFoldersList(Engine_SourceDis + @"\" + Engine_SelectedSubFolder))
                            {
                                foreach (string EnExt in Engine_ext)
                                {
                                    FilesCloner(Engine_SourceDis + @"\" + folder, Engine_TargetDis + @"\" + folder, "*." + EnExt.ToLower(), Engine_Overwrite);
                                    //Check Canceling Between Missions
                                    if (bwAsync.CancellationPending)
                                    {
                                        // Pause for a bit to demonstrate that there is time between
                                        // "Cancelling..." and "Cancel ed".
                                        Thread.Sleep(1200);

                                        // Set the e.Cancel flag so that the WorkerCompleted event
                                        // knows that the process was cancelled.
                                        e.Cancel = true;
                                        return;
                                    }
                                    //Check Canceling Between Missions
                                }
                            }
                        }
                    }
                    else {
                        MessageBox.Show("Please Select Subfolder first");
                    }
                bwAsync.ReportProgress(70);
            }
          
          
            //The Cloning Process

            //Delete all source files after cloning
            bwAsync.ReportProgress(75);
            if (Engine_DeleteSource)
            {
                if (MessageBox.Show("Are you sure you want to delete files from the source folder?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    //do no stuff
                }
                else
                {
                    foreach (string enTarext in Engine_DelExt)
                    {
                        FileDeleter(DisChecker(Engine_IsItMain, Engine_SourceDis), "*." + enTarext.ToLower());
                        //Check Canceling Between Missions
                        if (bwAsync.CancellationPending)
                        {
                            // Pause for a bit to demonstrate that there is time between
                            // "Cancelling..." and "Cancel ed".
                            Thread.Sleep(1200);

                            // Set the e.Cancel flag so that the WorkerCompleted event
                            // knows that the process was cancelled.
                            e.Cancel = true;
                            return;
                        }
                        //Check Canceling Between Missions
                    }
                    if (Engine_CloneSubfolders)
                    {
                        foreach (string folder in DirectoryManager.SubFoldersList(Engine_SourceDis + @"\" + Engine_SelectedSubFolder))
                        {
                            foreach (string enTarext in Engine_DelExt)
                            {
                                FileDeleter(DisChecker(Engine_IsItMain, Engine_SourceDis) + @"\" + folder, "*." + enTarext.ToLower());
                                //Check Canceling Between Missions
                                if (bwAsync.CancellationPending)
                                {
                                    // Pause for a bit to demonstrate that there is time between
                                    // "Cancelling..." and "Cancel ed".
                                    Thread.Sleep(1200);

                                    // Set the e.Cancel flag so that the WorkerCompleted event
                                    // knows that the process was cancelled.
                                    e.Cancel = true;
                                    return;
                                }
                                //Check Canceling Between Missions
                            }
                        }
                    }
                    bwAsync.ReportProgress(80);

                }
            }
            bwAsync.ReportProgress(100);
           //Delete all source files after cloning
     
        }

        private void bwAsync_RunWorkerCompleted (object sender, RunWorkerCompletedEventArgs e)
        {
 
             
            // The background process is complete. We need to inspect
            // our response to see if an error occurred, a cancel was
            // requested or if we completed successfully.

            //bnAsync.Text = "Start Long Running Asynchronous Process";
            //bnAsync.Enabled = true;

            // Check to see if an error occurred in the
            // background process.
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
                return;
            }
            // Check to see if the background process was cancelled.
            if (e.Cancelled)
            {
                MessageBox.Show("Cancelled...");
                //lblStatus.Text = "Cancelled...";
 
            }
            else
            {
                // Everything completed normally.
                // process the response using e.Result
                MessageBox.Show("Completed...");
                //lblStatus.Text = "Completed...";
                
            }

            ProgressPercentageLevel = 0;
            UpdateProgress(0);

        }

        private void bwAsync_ProgressChanged
                (object sender, ProgressChangedEventArgs e)
        {
            // This function fires on the UI thread so it's safe to edit
            // the UI control directly, no funny business with Control.Invoke.
            // Update the progressBar with the integer supplied to us from the
            // ReportProgress() function.  Note, e.UserState is a "tag" property
            // that can be used to send other information from the
            // BackgroundThread to the UI thread.

            ProgressPercentageLevel = e.ProgressPercentage;
           // Console.WriteLine(ProgressPercentageLevel);
            UpdateProgress(ProgressPercentageLevel);
        }

        #endregion

        #region Main functions 
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Copy specific type of files
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void FilesCloner(string sourceDir, string backupDir, string ext = "*.suffixType", bool Overwrite = true)
        {
            try
            {
                //Create The Folder if is not exists
                if (Directory.Exists(backupDir) == false)
                {
                    Console.WriteLine("Folder does not exist.");
                    // Try to create the directory.
                    DirectoryInfo di = Directory.CreateDirectory(backupDir);
                    Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(backupDir));
                }

                //  The Copy Process.
                string[] extList = Directory.GetFiles(sourceDir, ext);
                // Copy Ext files.
                foreach (string f in extList)
                {

                    // Remove path from the file name.
                    string fName = f.Substring(sourceDir.Length + 1);
                    // Use the Path.Combine method to safely append the file name to the path.
                    // Will overwrite if the destination file already exists...
                    if (Overwrite) { 
                    File.Copy(Path.Combine(sourceDir, fName), Path.Combine(backupDir, fName), Overwrite);
                    }
                    else if (!Overwrite && !File.Exists(Path.Combine(backupDir, fName)))
                    {
                        File.Copy(Path.Combine(sourceDir, fName), Path.Combine(backupDir, fName), Overwrite);
                    }
                }
          
            }
            catch (DirectoryNotFoundException dirNotFound)
            {
                Console.WriteLine(dirNotFound.Message);
            }
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Copy specific type of files
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Delete files in Folder.
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void FileDeleter(string Dir, string ext = "*.suffixType")
        {
            try 
            { 

            string[] ListOfFiles = Directory.GetFiles(Dir, ext); //List of files in the target folder
                foreach (string f in ListOfFiles)
                {
                    File.Delete(f);
                }
            
            }
            catch (DirectoryNotFoundException dirNotFound)
            {
                Console.WriteLine(dirNotFound.Message);
            }
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Delete files in Folder.
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// s  
        #endregion




    }
}

