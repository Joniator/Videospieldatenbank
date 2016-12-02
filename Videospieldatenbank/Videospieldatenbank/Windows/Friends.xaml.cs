using System;
using System.Windows;

namespace Videospieldatenbank
{
    /// <summary>
    ///     Interaktionslogik für Friends.xaml
    /// </summary>
    public partial class Friends : Window
    {
        public Friends()
        {
            InitializeComponent();
        }

        private void Friends_OnClosed(object sender, EventArgs e)
        {
            MainWindow.friends = null;
        }
    }
}