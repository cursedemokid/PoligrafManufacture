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
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        List<Employee> employees = App.context.Employee.ToList();
        public AuthorizationWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (employees.FirstOrDefault(em => em.Login == LoginTb.Text && em.Password == PasswordPb.Password) != null)
            {
                Employee currentEmployee = employees.FirstOrDefault(em => em.Login == LoginTb.Text && em.Password == PasswordPb.Password);
                App.currentEmployee = currentEmployee;
                MessageBox.Show($"Добро пожаловать, {currentEmployee.LastName} {currentEmployee.FirstName}!");
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show($"Вы неправильно ввели логин или пароль!");
            }
        }
    }
}
