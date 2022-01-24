using FilesCloner.Models;
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

         
        public void bnAsync_Click()
        {
            // If the background thread is running then clicking this
            // button causes a cancel, otherwise clicking this button
            // launches the background thread.
            if (m_AsyncWorker.IsBusy)
            {
                if (MessageBox.Show("Do you want to interrupt the cloning process?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    // Notify the worker thread that a cancel has been requested.
                    // The cancel will not actually happen until the thread in the
                    // DoWork checks the bwAsync.CancellationPending flag
                    m_AsyncWorker.CancelAsync();
                } 
            }
            else
            {
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
            int Percentagelevel = 0;
            //Delete target files before cloning by selected ext if is it true
            if (Engine_DeleteTarget)
            {
                foreach (string Delext in Engine_DelExt) 
                { 
                //List of all ext files in the Target
                List<FileModel> ListOfFiless =  DirectoryManager.TreeOfFiles(DisChecker(Engine_IsItMain, Engine_TargetDis), "*." +Delext, Engine_CloneSubfolders);

                foreach (FileModel f in ListOfFiless)
                {
                        File.Delete(f.FilePath);   
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
            //Delete target files before cloning by selected ext if is it true

            /////////////////////////////////////////////////////////////////////////// From 0 to 25 is the deleting stage of the target folder
            Percentagelevel = 25;
            bwAsync.ReportProgress(Percentagelevel);
            ///////////////////////////////////////////////////////////////////////////
        
            //The Cloning Process
            foreach (string ext in Engine_ext)
            {
                List<FileModel> ListToCopy = DirectoryManager.TreeOfFiles(DisChecker(Engine_IsItMain, Engine_SourceDis), "*." + ext, Engine_CloneSubfolders);
                foreach (FileModel FileToCopy in ListToCopy)
                {            
                    if (!Directory.Exists(FileToCopy.DirectoryName.Replace(Engine_SourceDis, Engine_TargetDis)))
                    {
                        Directory.CreateDirectory(FileToCopy.DirectoryName.Replace(Engine_SourceDis, Engine_TargetDis));
                    }
                    if (File.Exists(FileToCopy.FilePath.Replace(Engine_SourceDis, Engine_TargetDis)) && Engine_Overwrite == false)
                    {
                        Console.WriteLine("File " + FileToCopy.FilePath.Replace(Engine_SourceDis, Engine_TargetDis) + " is already exist");
                    }
                    else  
                    {                  
                        File.Copy(FileToCopy.FilePath, FileToCopy.FilePath.Replace(Engine_SourceDis, Engine_TargetDis), Engine_Overwrite);
                    }
     
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
            //The Cloning Process

            /////////////////////////////////////////////////////////////////////////// From 25 to 75 is the copy stage of files
            Percentagelevel = 75;
            bwAsync.ReportProgress(Percentagelevel);
            ///////////////////////////////////////////////////////////////////////////

            //Delete source files after cloning by selected ext if is it true
            if (Engine_DeleteSource)
            {
                if (MessageBox.Show("Are you sure you want to delete files from the source folder?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    //do no stuff
                }
                else
                {
                    foreach (string Delext in Engine_DelExt)
                    {
                        //List of all ext files in the Target
                        List<FileModel> ListOfFiless = DirectoryManager.TreeOfFiles(DisChecker(Engine_IsItMain, Engine_SourceDis), "*." + Delext, Engine_CloneSubfolders);
                   
                        foreach (FileModel sf in ListOfFiless)
                        {
                            File.Delete(sf.FilePath);
               
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
            //Delete source files after cloning by selected ext if is it true
            /////////////////////////////////////////////////////////////////////////// From 75 to 100 is the deleting stage of source folder if is it true
            Percentagelevel = 100;
            bwAsync.ReportProgress(Percentagelevel);
            ///////////////////////////////////////////////////////////////////////////
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
                
            }
            else
            {
                // Everything completed normally.
                // process the response using e.Result
                MessageBox.Show("Cloning process is complete");
                
            }
             
            // Reset all percentages
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

    }
}

