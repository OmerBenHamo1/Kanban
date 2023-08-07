using IntroSE.Kanban.Backend.BuisnessLayer;
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
    /// Interaction logic for UserMainSpace.xaml
    /// </summary>
    public partial class UserMainSpace : Window
    {

        private UserMainSpaceVM userMainSpaceVM;
        private StartFrontend StartFrontend { get; }
        public UserMainSpace(StartFrontend startFrontend, UserUI user)
        {
            this.StartFrontend = startFrontend;
            InitializeComponent();
            this.userMainSpaceVM = new UserMainSpaceVM(StartFrontend, user);
            DataContext = userMainSpaceVM;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            List<TaskUI>? inProgressTasks = userMainSpaceVM.GetInProgressTasks();
            if (inProgressTasks != null)
            {
                new InPogressTasksWindow(StartFrontend, inProgressTasks).Show();
            }
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            userMainSpaceVM.Logout();
            this.Close();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem is int selectedBoardId)
            {
                BoardWindow boardWindow = new BoardWindow(StartFrontend, selectedBoardId);
                boardWindow.Show();
            }
        }
    }
}
