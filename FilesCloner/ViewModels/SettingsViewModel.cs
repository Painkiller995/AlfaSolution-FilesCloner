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
    class SettingsViewModel : Screen
    {



        private List<string> _ListOfThemes;
        private string _SelectedTheme;
        public List<string> ListOfThemes
        {
            get { return _ListOfThemes; }
            set
            {
                _ListOfThemes = value;
                NotifyOfPropertyChange(() => ListOfThemes);
            }
        }
        public string SelectedTheme
        {
            get { return _SelectedTheme; }
            set
            {
                _SelectedTheme = value;
                SetTheme("Theme", SelectedTheme);
                ApplyTheme(SelectedTheme);
                NotifyOfPropertyChange(() => ListOfThemes);
                NotifyOfPropertyChange(() => SelectedTheme);
            }
        }


        ///////////////////////////////////////////////////////////////////
        SettingsManager SettingsManager = new SettingsManager();
        /////////////////////////////////////////////////////////////////// 

        public SettingsViewModel()
        {
            ListOfThemes = new List<string>();
            ListOfThemes.Add("Auto");
            ListOfThemes.Add("Light");
            ListOfThemes.Add("Dark");
            if (string.IsNullOrEmpty(ReadTheme("Theme")) == false)
            {
                SelectedTheme = ReadTheme("Theme");

            }
        }

        public string ReadTheme(string ThemeKey)
        {
            try
            {
                return SettingsManager.ReadVal(ThemeKey);
            }
            catch
            {
                Console.WriteLine("cannot read value for SelTheme");
                return string.Empty;
            }
        }

        public bool SetTheme(string ThemeKey, string ThemeVal)
        {
            try
            {
                SettingsManager.WriteVal(ThemeKey, null, ThemeVal);
                return true;
            }
            catch
            {
                Console.WriteLine("cannot read value for SelTheme");
                return false;
            }
        }


        public void ApplyTheme(string ThemeVal)
        {


            if (ThemeVal.ToLower() == "auto")
            {
                ModernWpf.ThemeManager.Current.ApplicationTheme = null;

            }
            else if (ThemeVal.ToLower() == "light")
            {
                ModernWpf.ThemeManager.Current.ApplicationTheme = ModernWpf.ApplicationTheme.Light;

            }
            else if (ThemeVal.ToLower() == "dark")
            {
                ModernWpf.ThemeManager.Current.ApplicationTheme = ModernWpf.ApplicationTheme.Dark;

            }
            else
            {
                ModernWpf.ThemeManager.Current.ApplicationTheme = null;

            }


        }
    }
}
