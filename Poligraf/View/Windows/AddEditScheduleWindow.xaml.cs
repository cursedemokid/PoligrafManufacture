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
    /// Логика взаимодействия для AddEditScheduleWindow.xaml
    /// </summary>
    public partial class AddEditScheduleWindow : Window
    {
        List<Schedule> schedules = App.context.Schedule.ToList();
        Schedule schedule = new Schedule();
        public AddEditScheduleWindow()
        {
            InitializeComponent();
        }
        public AddEditScheduleWindow(int Id)
        {
            InitializeComponent();
            schedule = schedules.FirstOrDefault(s => s.Id == Id);
            EmployeeCmb.ItemsSource = App.context.Employee.ToList();
            OrderCmb.ItemsSource = App.context.Order.ToList();
            PrinterCmb.ItemsSource = App.context.PrintMachine.ToList();
            StartDp.SelectedDate = schedule.StartDateTime;
            FinishDp.SelectedDate = schedule.FinishDateTime;
            MessageBox.Show("Вы успешно обновили запись");
            Close();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Schedule newSchedule = new Schedule()
                {
                    EmployeeId = Convert.ToInt32(EmployeeCmb.SelectedValue),
                    OrderId = Convert.ToInt32(OrderCmb.SelectedValue),
                    PrintMachineId = Convert.ToInt32(PrinterCmb.SelectedValue),
                    StartDateTime = Convert.ToDateTime(StartDp.SelectedDate),
                    FinishDateTime = Convert.ToDateTime(FinishDp.SelectedDate)
                };
                App.context.Schedule.Add(newSchedule);
                App.context.SaveChanges();
                MessageBox.Show("Вы успешно добавили запись");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            schedule.EmployeeId = Convert.ToInt32(EmployeeCmb.SelectedValue);
            schedule.OrderId = Convert.ToInt32(OrderCmb.SelectedValue);
            schedule.PrintMachineId = Convert.ToInt32(PrinterCmb.SelectedValue);
            schedule.StartDateTime = Convert.ToDateTime(StartDp.SelectedDate);
            schedule.FinishDateTime = Convert.ToDateTime(FinishDp.SelectedDate);
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
