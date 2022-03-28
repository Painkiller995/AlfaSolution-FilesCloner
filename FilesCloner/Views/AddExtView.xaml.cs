using System;
using System.Windows;
using System.Windows.Controls;

namespace FilesCloner.Views
{
    /// <summary>
    /// Interaction logic for AddExtView.xaml
    /// </summary>
    public partial class AddExtView : Window
    {
        public AddExtView()
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
