using Caliburn.Micro;

using FilesCloner.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FilesCloner.ViewModels
{
    class AddExtViewModel : Screen
    {
        private List<string> _ListOfEXT;
        private string _NewExtText;
        private string _SelectedExt;
        public List<string> ListOfEXT
        {
            get { return _ListOfEXT; }
            set
            {
                _ListOfEXT = value;
                NotifyOfPropertyChange(() => ListOfEXT);
            }
        }
        public string NewExtText
        {
            get { return _NewExtText; }
            set
            {
                _NewExtText = value;
                NotifyOfPropertyChange(() => NewExtText);
            }
        }

        public string SelectedExt
        {
            get { return _SelectedExt; }
            set
            {
                _SelectedExt = value;
                NotifyOfPropertyChange(() => SelectedExt);
            }
        }


        ///////////////////////////////////////////////////////////////////
        ExtensionsManager ExtensionsManager = new ExtensionsManager();
        /////////////////////////////////////////////////////////////////// 
        public AddExtViewModel()
        {
            LoadListOfExt();
        }

        public void LoadListOfExt()
        {
            try
            {
                ListOfEXT = ExtensionsManager.ListOfEXT();
            }
            catch
            {
                MessageBox.Show("Error while loading EXT list");
            }
        }

        public void AddExt()
        {
            try
            {

                if (!string.IsNullOrWhiteSpace(NewExtText))
                {
                    ExtensionsManager.addExt(NewExtText);
                    NewExtText = string.Empty;
                }
                else
                {
                    MessageBox.Show("Please Enter Ext first");
                }
                LoadListOfExt();
            }
            catch
            {
                MessageBox.Show("Error while adding new ext");
            }

        }

        public void DelExt()
        {
            try
            {

                if (!string.IsNullOrWhiteSpace(SelectedExt))
                {
                    ExtensionsManager.DelExt(SelectedExt);
                    NewExtText = string.Empty;
                }
                else
                {
                    MessageBox.Show("Please Select Ext first");
                }
                LoadListOfExt();
            }
            catch
            {
                MessageBox.Show("Error while deleting ext");
            }

        }
    }
}
