using Poligraf.Model;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Poligraf
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static PoligrafEntities1 context = new PoligrafEntities1();
        public static Employee currentEmployee = new Employee();
    }
}
