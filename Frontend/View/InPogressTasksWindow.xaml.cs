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
    /// Interaction logic for InPogressTasksWindow.xaml
    /// </summary>
    public partial class InPogressTasksWindow : Window
    {
        private InProgressTasksVM inProgressTasksVM;
        private StartFrontend StartFrontend { get; }
        public InPogressTasksWindow(StartFrontend startFrontend, List<TaskUI> inProgressTasks)
        {
            InitializeComponent();
            StartFrontend = startFrontend;
            inProgressTasksVM = new InProgressTasksVM(startFrontend, inProgressTasks);
            this.DataContext = inProgressTasksVM;
            RefreshDataGrid();
        }
        private void RefreshDataGrid()
        {
            dataGrid.ItemsSource = null; 
            dataGrid.ItemsSource = inProgressTasksVM.InProgressTasks;
        }
        private void OpenTaskWindow(TaskUI selectedTask)
        {
            TaskWindow taskWindow = new TaskWindow(StartFrontend, selectedTask);
            taskWindow.Show();
        }
    }
}
