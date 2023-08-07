using IntroSE.Kanban.Frontend.Model;
using IntroSE.Kanban.Frontend.ViewModel;
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

namespace IntroSE.Kanban.Frontend.View
{
    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        private StartFrontend StartFrontend { get;}
        private TaskVM TaskVM;

        public TaskWindow(StartFrontend startFrontend, TaskUI taskUI)
        {
            InitializeComponent();
            StartFrontend = startFrontend;
            TaskVM = new TaskVM(StartFrontend, taskUI);
        }

        private void Button_back(object sender, RoutedEventArgs e)
        {

            this.Close();

        }
    }
}
