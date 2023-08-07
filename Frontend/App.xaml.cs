using IntroSE.Kanban.Frontend.Model;
using IntroSE.Kanban.Frontend.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace IntroSE.Kanban.Frontend
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            StartFrontend StartFrontend = new StartFrontend();
            MainWindow MainWindow = new MainWindow(StartFrontend);
            MainWindow.Show();
        }        
    }
}
