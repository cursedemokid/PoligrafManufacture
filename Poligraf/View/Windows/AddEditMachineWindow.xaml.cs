using Poligraf.Model;

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
using System.Windows.Shapes;

namespace Poligraf.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddEditMachineWindow.xaml
    /// </summary>
    public partial class AddEditMachineWindow : Window
    {
        List<PrintMachine> machines = App.context.PrintMachine.ToList();
        PrintMachine currentMachine = new PrintMachine();
        public AddEditMachineWindow()
        {
            InitializeComponent();
            EditBtn.Visibility = Visibility.Collapsed;
        }
        public AddEditMachineWindow(int id)
        {
            InitializeComponent();
            AddBtn.Visibility = Visibility.Collapsed;
            currentMachine = machines.FirstOrDefault(x => x.Id == id);
            ModelTb.Text = currentMachine.Model;
            SpeedTb.Text = currentMachine.PrintSpeedInMinute.ToString();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PrintMachine newPrintMachine = new PrintMachine()
                {
                    Model = ModelTb.Text,
                    StateId = 2,
                    PrintSpeedInMinute = Convert.ToInt32(SpeedTb.Text)
                };
                App.context.PrintMachine.Add(newPrintMachine);
                App.context.SaveChanges();
                MessageBox.Show("Вы успешно добавили печатную машину");
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"{ex}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            currentMachine.Model = ModelTb.Text;
            currentMachine.PrintSpeedInMinute = Convert.ToInt32(SpeedTb.Text);
            App.context.SaveChanges();
            MessageBox.Show("Вы успешно изменили данные");
            Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
