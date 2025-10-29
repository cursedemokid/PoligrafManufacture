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
    /// Логика взаимодействия для OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        List<Schedule> schedules = App.context.Schedule.ToList();
        public OrderPage()
        {
            InitializeComponent();
            OrdersLv.ItemsSource = schedules;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            AddEditScheduleWindow addEditScheduleWindow = new AddEditScheduleWindow();
            addEditScheduleWindow.ShowDialog();
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            Schedule selectedSchedule = OrdersLv.SelectedItem as Schedule;
            if (selectedSchedule != null)
            {
                AddEditScheduleWindow editScheduleWindow = new AddEditScheduleWindow(selectedSchedule.Id);
                editScheduleWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Вы не выбрали записьк из списка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            Schedule selectedSchedule = OrdersLv.SelectedItem as Schedule;
            if (selectedSchedule != null)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить запись из БД?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    PrintMachine printMachine = selectedSchedule.PrintMachine;
                    printMachine.StateId = 2;
                    App.context.Schedule.Remove(selectedSchedule);
                    App.context.SaveChanges();

                    MessageBox.Show("Запись удалена из БД");
                }
            }
            else
            {
                MessageBox.Show("Вы не выбрали записьк из списка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
