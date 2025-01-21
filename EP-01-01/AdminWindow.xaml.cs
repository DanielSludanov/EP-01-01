using EP_01_01.AdminPages;
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

namespace EP_01_01
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private bool isNav = false;
        public AdminWindow()
        {
            InitializeComponent();
            FrameManager.MainFrame = MainFrame;
        }









        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isNav)
            {
                return;
            }

            var result = MessageBox.Show("Вы действительно хотите выйти из приложения?",
                "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes) { e.Cancel = false; } else { e.Cancel = true; }
        }

        private void BtnTests_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new Uri("AdminPages/Tests.xaml", UriKind.Relative));
        }

        private void BtnQuesitons_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnReports_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
