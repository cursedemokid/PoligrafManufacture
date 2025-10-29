using Poligraf.View.Pages;

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

namespace Poligraf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainFrame.Navigate(new MachinesPage());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите закрыть приложение?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Close();
            }
        }

        private void MachineBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new MachinesPage());
        }

        private void OrderBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new OrderPage());
        }
    }
}
