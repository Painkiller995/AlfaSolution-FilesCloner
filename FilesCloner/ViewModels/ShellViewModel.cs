using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilesCloner.Core;
using System.Windows;
using System.IO;


namespace FilesCloner.ViewModels
{
    class ShellViewModel : Screen
    {
        /// WindowManager
        IWindowManager WM = new WindowManager();
        /// WindowManager

        /// User configuration manager
        SettingsManager Loader = new SettingsManager();
        /// User configuration manager

        /// DirectoryManager
        DirectoryManager DirectoryManager = new DirectoryManager();
        /// DirectoryManager

        /// Cloner
        CopyEngine Cloner = new CopyEngine();
        /// Cloner

        // Fire on ExtensionsManager
        ExtensionsManager ExtensionsManager = new ExtensionsManager();
        // Fire on ExtensionsManager

        /// Items section

        private string _SourceDis;
        private string _TargetDis;
        private List<string> OrginalSubfolders;
        private List<string> _Subfolders;
        private List<string> _ListOfExt;
        private string _SearchBar;
        private bool _OverWriteBool;
        private bool _MainBool;
        private bool _CopySubfoldersBool;
        private bool _DeleteSubfoldersBool;
        private bool _DelAllTargetFilesBefore;
        private bool _DelAllSourceFilesAfter;
        private bool _CloneAllFilesBool;
        private bool _DelAllFilesBool;
        private string _SelectedSubFolder;
        private int _ProgBarVal;
        private string _LogInfo;
        
        public string SourceDis
        {
            get { return _SourceDis; }
            set
            {
                _SourceDis = value;
                Loader.WriteVal(MainKey: "SourceDis", Val: SourceDis);
                NotifyOfPropertyChange(() => SourceDis);
                SubfoldersRefresher();
                NotifyOfPropertyChange(() => Subfolders);
            }

        }
        public string TargetDis
        {
            get { return _TargetDis; }
            set
            {
                _TargetDis = value;
                Loader.WriteVal(MainKey: "TargetDis", Val: TargetDis);
                NotifyOfPropertyChange(() => TargetDis);
            }
        }
        public List<string> Subfolders
        {
            get { return _Subfolders; }
            set
            {
                _Subfolders = value;
                NotifyOfPropertyChange(() => Subfolders);
            }
        }

        public List<string> ListOfExt
        {
            get { return _ListOfExt; }
            set
            {
                _ListOfExt = value;
                NotifyOfPropertyChange(() => ListOfExt);

            }
        }


        public string SelectedSubFolder
        {
            get { return _SelectedSubFolder; }
            set
            {
                _SelectedSubFolder = value;
                NotifyOfPropertyChange(() => SelectedSubFolder);
            }
        }
        public string SearchBar
        {
            get { return _SearchBar; }
            set
            {
                _SearchBar = value;
                NotifyOfPropertyChange(() => SearchBar);
                NotifyOfPropertyChange(() => Subfolders);
                UpdateSubfolderList();
                NotifyOfPropertyChange(() => Subfolders);
            }
        }
        public bool OverWriteBool
        {
            get { return _OverWriteBool; }
            set
            {
                _OverWriteBool = value;
                Loader.WriteVal(MainKey: "OverWriteBool", Val: OverWriteBool.ToString());
                NotifyOfPropertyChange(() => OverWriteBool);
            }
        }
        public bool MainBool
        {
            get { return _MainBool; }
            set
            {
                _MainBool = value;
                Loader.WriteVal(MainKey: "CopyMainBool", Val: MainBool.ToString());
                NotifyOfPropertyChange(() => MainBool);
            }
        }
        public bool CopySubfoldersBool
        {
            get { return _CopySubfoldersBool; }
            set
            {
                _CopySubfoldersBool = value;
                Loader.WriteVal(MainKey: "CopySubfoldersBool", Val: CopySubfoldersBool.ToString());
                NotifyOfPropertyChange(() => CopySubfoldersBool);

            }
        }

        public bool DeleteSubfoldersBool
        {
            get { return _DeleteSubfoldersBool; }
            set
            {
                _DeleteSubfoldersBool = value;
                Loader.WriteVal(MainKey: "DeleteSubfoldersBool", Val: DeleteSubfoldersBool.ToString());
                NotifyOfPropertyChange(() => DeleteSubfoldersBool);
            }
        }

        public bool DelAllTargetFilesBefore
        {
            get { return _DelAllTargetFilesBefore; }
            set
            {
                _DelAllTargetFilesBefore = value;
                Loader.WriteVal(MainKey: "DelAllTargetFilesBefore", Val: DelAllTargetFilesBefore.ToString());
                NotifyOfPropertyChange(() => DelAllTargetFilesBefore);
            }
        }
        public bool DelAllSourceFilesAfter
        {
            get { return _DelAllSourceFilesAfter; }
            set
            {
                _DelAllSourceFilesAfter = value;
                Loader.WriteVal(MainKey: "DelAllSourceFilesAfter", Val: DelAllSourceFilesAfter.ToString());
                NotifyOfPropertyChange(() => DelAllSourceFilesAfter);
            }
        }

        public bool CloneAllFilesBool
        {
            get { return _CloneAllFilesBool; }
            set
            {
                _CloneAllFilesBool = value;
                Loader.WriteVal(MainKey: "CloneAllFilesBool", Val: CloneAllFilesBool.ToString());
                NotifyOfPropertyChange(() => CloneAllFilesBool);
            }
        }

        public bool DelAllFilesBool
        {
            get { return _DelAllFilesBool; }
            set
            {
                _DelAllFilesBool = value;
                Loader.WriteVal(MainKey: "DelAllFilesBool", Val: DelAllFilesBool.ToString());
                NotifyOfPropertyChange(() => DelAllFilesBool);
            }
        }

        public int ProgBarVal
        {
            get { return _ProgBarVal; }
            set
            {
                _ProgBarVal = value;
                NotifyOfPropertyChange(() => ProgBarVal);
            }
        }

        public string LogInfo
        {
            get { return _LogInfo; }
            set
            {
                _LogInfo = value;
                NotifyOfPropertyChange(() => LogInfo);
            }
        }

        /// Items section

        public ShellViewModel()  //Constructor function
        {
            try
            {

                SettingsReader();

            }
            catch
            {
                Console.WriteLine("Error while trying to load all settings");
            }

            Cloner.UpdateProgress += UpdateProgress;
        }

        private void UpdateProgress(int ProgressPercentage, string EnInfo)
        {
            ProgBarVal = ProgressPercentage;
            LogInfo = EnInfo;
        }

        ///////////////////// SettingsReader /////////////////////
        public bool ParseBool(string input) //Custom ParseBool
        {
            switch (input.ToLower())
            {
                case "true":
                    return true;
                default:
                    return false;
            }
        }
        public void SettingsReader()
        {
            //Try to Read Settings
            if (Loader.SettingsChecker())
            {
                SourceDis = Loader.ReadVal("SourceDis");
                OrginalSubfolders = DirectoryManager.SubFoldersList(SourceDis);
                TargetDis = Loader.ReadVal("TargetDis");
                OverWriteBool = ParseBool(Loader.ReadVal("OverWriteBool"));
                MainBool = ParseBool(Loader.ReadVal("CopyMainBool"));
                CopySubfoldersBool = ParseBool(Loader.ReadVal("CopySubfoldersBool"));
                DeleteSubfoldersBool = ParseBool(Loader.ReadVal("DeleteSubfoldersBool"));
                CloneAllFilesBool = ParseBool(Loader.ReadVal("CloneAllFilesBool"));
                DelAllFilesBool = ParseBool(Loader.ReadVal("DelAllFilesBool"));
                DelAllTargetFilesBefore = ParseBool(Loader.ReadVal("DelAllTargetFilesBefore"));
                DelAllSourceFilesAfter = ParseBool(Loader.ReadVal("DelAllSourceFilesAfter"));
                ListOfExt = ExtensionsManager.ListOfEXT();
                SubfoldersRefresher();
                ///////////////////////////////////////////////////////////////
                SettingsViewModel ThemeReader = new SettingsViewModel();
                ThemeReader.ApplyTheme(Loader.ReadVal("Theme"));
            }
            else
            {
                Loader.SettingsInitializer();
            }
        }
        ///////////////////// SettingsReader /////////////////////

        ///////////////////// Refresh The List Of Subfolders /////////////////////
        public void SubfoldersRefresher()
        {
            try
            {
                OrginalSubfolders = DirectoryManager.SubFoldersList(SourceDis);
                Subfolders = OrginalSubfolders;
            }
            catch
            {
                Console.WriteLine("SubfoldersRefresher Error");
            }


        }
        ///////////////////// Refresh The List Of Subfolders /////////////////////

        ///////////////////// SearchBar /////////////////////////////////////////
        public void UpdateSubfolderList()
        {
            try
            {
                //Console.WriteLine(SearchBar);
                List<string> FilteredUsers = new List<string>();
                FilteredUsers.AddRange(OrginalSubfolders.Where(i => i.ToLower().Contains(SearchBar.ToLower())));
                Subfolders = FilteredUsers;
            }
            catch
            {
                Console.WriteLine("Cannot find subfolders. Make sure you add source directory");
                //MessageBox.Show("Cannot find subfolders. Make sure you add source directory");
            }
        }
        ///////////////////// SearchBar ///////////////////////////////////////// 

        ///////////////////// SourceBrowse BTN //////////////////////////////////
        public void SourceBrowse()
        {
            //Console.WriteLine("Browse the Source");
            SourceDis = DirectoryManager.DirectoryFinder("Please Select The Source Folder:");
            //OrginalSubfolders = DirectoryManager.SubFoldersList(SourceDis);
            //Subfolders = OrginalSubfolders;
            SubfoldersRefresher();
        }
        ///////////////////// SourceBrowse BTN //////////////////////////////////


        ///////////////////// TargetBrowse BTN //////////////////////////////////
        public void TargetBrowse()
        {
            //Console.WriteLine("Browse the Target");       
            TargetDis = DirectoryManager.DirectoryFinder("Please Select The Target Folder:");

        }
        ///////////////////// TargetBrowse BTN //////////////////////////////////

        ///////////////////// EXT Manager BTN ////////////////////////////////// 
        public void EXTMNG()
        {

            WM.ShowDialogAsync(new AddExtViewModel(), null, null);
            //Read All Ext Settings Again
            ListOfExt = ExtensionsManager.ListOfEXT();

        }
        ///////////////////// EXT Manager BTN //////////////////////////////////

        ///////////////////// EXT Manager BTN ////////////////////////////////// 
        public void SettingsWindow()
        {

            WM.ShowDialogAsync(new SettingsViewModel(), null, null);
            //Read All Ext Settings Again
            ListOfExt = ExtensionsManager.ListOfEXT();

        }
        ///////////////////// EXT Manager BTN //////////////////////////////////
        public void Refresh()
        {
            try
            {
                SubfoldersRefresher();
                UpdateSubfolderList();
            }
            catch
            {
                Console.WriteLine("Error while trying to load data");
            }
        }
        ///////////////////// Refresh BTN ///////////////////////////////////////// 

        ///////////////////// Refresh BTN ///////////////////////////////////////// 
        ///
        ///////////////////// Clone BTN ///////////////////////////////////////// 
        public void StartClone()
        {
            try
            {
                if (MainBool == true || MainBool == false && SelectedSubFolder is object)
                {
                    //Talk to CopyEngine
                    Cloner.EngineDatarovider(
                    SourceDis: SourceDis,
                    TargetDis: TargetDis,
                    Subfolders: Subfolders,
                    SelectedSubFolder: SelectedSubFolder,
                    IsItMain: MainBool,
                    CloneSubfolders: CopySubfoldersBool,
                    DelSubfolders: DeleteSubfoldersBool,
                    ext: ListOfExt,
                    DelExt: ListOfExt,
                    CloneAllFiles: CloneAllFilesBool,
                    DeleteAllFiles: DelAllFilesBool,
                    DeleteSource: DelAllSourceFilesAfter,
                    DeleteTarget: DelAllTargetFilesBefore,
                    Overwrite: OverWriteBool
                    );
                    Cloner.bnAsync_Click();
                }
                else
                {
                    MessageBox.Show("Please select subfolder or check -> Clone the main directory files");
                }
            }
            catch
            {
                MessageBox.Show("Something went wrong. Try again.");
            }

        }
        ///////////////////// Clone BTN ///////////////////////////////////////// 

    }
}
