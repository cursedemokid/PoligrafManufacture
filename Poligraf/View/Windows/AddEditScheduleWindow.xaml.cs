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
            EditBtn.Visibility = Visibility.Collapsed;
            EmployeeCmb.ItemsSource = App.context.Employee.ToList();
            OrderCmb.ItemsSource = App.context.Order.ToList();
            PrinterCmb.ItemsSource = App.context.PrintMachine.ToList();
        }
        public AddEditScheduleWindow(int Id)
        {
            InitializeComponent();
            AddBtn.Visibility = Visibility.Collapsed;
            schedule = schedules.FirstOrDefault(s => s.Id == Id);
            EmployeeCmb.ItemsSource = App.context.Employee.ToList();
            OrderCmb.ItemsSource = App.context.Order.ToList();
            PrinterCmb.ItemsSource = App.context.PrintMachine.ToList();
            StartDp.SelectedDate = schedule.StartDateTime;
            FinishDp.SelectedDate = schedule.FinishDateTime;
            EmployeeCmb.SelectedValue = schedule.EmployeeId;
            OrderCmb.SelectedValue = schedule.OrderId;
            PrinterCmb.SelectedValue = schedule.PrintMachineId;
            
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime startDateTime = Convert.ToDateTime(StartDp.SelectedDate);
                DateTime finishDateTime = Convert.ToDateTime(FinishDp.SelectedDate);
                Schedule newSchedule = new Schedule()
                {
                    EmployeeId = Convert.ToInt32(EmployeeCmb.SelectedValue),
                    OrderId = Convert.ToInt32(OrderCmb.SelectedValue),
                    PrintMachineId = Convert.ToInt32(PrinterCmb.SelectedValue),
                    StartDateTime = startDateTime,
                    FinishDateTime = finishDateTime
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
            App.context.SaveChanges();
            MessageBox.Show("Вы успешно обновили запись");
            Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
