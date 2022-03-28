using System;
using System.Windows;
using System.Windows.Controls;

namespace FilesCloner.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : Window
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Mica.Window_Loaded(sender,e);
        }
        private void Window_OnContentRendered(object sender, EventArgs e) // fix outline
        {
            InvalidateVisual();
        }

    }
}
