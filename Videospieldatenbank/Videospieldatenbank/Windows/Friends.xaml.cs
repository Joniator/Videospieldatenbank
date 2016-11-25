using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Videospieldatenbank
{
    /// <summary>
    /// Interaktionslogik für Friends.xaml
    /// </summary>
    public partial class Friends : Window
    {
        public Friends()
        {
            InitializeComponent();
        }

        private void Friends_OnClosed(object sender, EventArgs e)
        {
            MainWindow.Friends = null;
        }
    }
}
