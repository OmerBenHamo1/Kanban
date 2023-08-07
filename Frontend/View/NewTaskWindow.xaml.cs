using IntroSE.Kanban.Frontend.ViewModel;
using IntroSE.Kanban.Backend.BuisnessLayer;
using IntroSE.Kanban.Frontend.Model;
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
    /// Interaction logic for NewTaskWindow.xaml
    /// </summary>
    public partial class NewTaskWindow : Window
    {
        private StartFrontend StartFrontend;
        private NewTaskVM newTaskVM;

        public NewTaskWindow(StartFrontend startFrontend)
        {
            InitializeComponent();
            StartFrontend = startFrontend;
            newTaskVM = new NewTaskVM(StartFrontend);
            this.DataContext = newTaskVM;
        }
        private void Button_add(object sender, RoutedEventArgs e)
        {

            this.Close();

        }
        private void Button_cancel(object sender, RoutedEventArgs e)
        {

            this.Close();

        }
    }
}
