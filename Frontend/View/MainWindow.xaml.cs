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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LoginVM loginVM;
        private StartFrontend StartFrontend { get; set; }
        public MainWindow(StartFrontend startFrontend)
        {
            this.StartFrontend = startFrontend;
            InitializeComponent();
            loginVM = new LoginVM(StartFrontend);
            this.DataContext = loginVM;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var user = loginVM.Login();
            if(user != null)
            {
                new UserMainSpace(StartFrontend, user).Show();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            loginVM.Register();
        }
    }
}
