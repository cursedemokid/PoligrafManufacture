using Poligraf.Model;
using Poligraf.View.Windows;

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

namespace Poligraf.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для MachinesPage.xaml
    /// </summary>
    public partial class MachinesPage : Page
    {
        List<PrintMachine> machines = App.context.PrintMachine.ToList();
        List<Schedule> schedules = App.context.Schedule.ToList();
        public MachinesPage()
        {
            InitializeComponent();

            PrintMachinesLv.ItemsSource = machines;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            AddEditMachineWindow addEditMachineWindow = new AddEditMachineWindow();
            addEditMachineWindow.ShowDialog();
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            PrintMachine selectedMachine = PrintMachinesLv.SelectedItem as PrintMachine;
            if (selectedMachine != null)
            {
                AddEditMachineWindow addEditMachineWindow = new AddEditMachineWindow(selectedMachine.Id);
                addEditMachineWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Вы не выбрали печатный станок из списка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            PrintMachine selectedMachine = PrintMachinesLv.SelectedItem as PrintMachine;
            if (selectedMachine != null)
            {
                if (schedules.FirstOrDefault(s => s.PrintMachineId == selectedMachine.Id) != null)
                {
                    MessageBox.Show("За печатным станком еще закреплен заказ!");
                }
                else
                {
                    if (MessageBox.Show("Вы уверены, что хотите удалить печатный станок из БД?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        App.context.PrintMachine.Remove(selectedMachine);
                        App.context.SaveChanges();
                        MessageBox.Show("Запись удалена из БД");
                    }
                }
            }
            else
            {
                MessageBox.Show("Вы не выбрали печатный станок из списка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
