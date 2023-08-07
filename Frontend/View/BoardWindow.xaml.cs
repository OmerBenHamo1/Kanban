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
    /// Interaction logic for BoardWindow.xaml
    /// </summary>
    /// 
    public partial class BoardWindow : Window
    {
        private StartFrontend StartFrontend;
        private BoardVM boardVM;
        public BoardWindow(StartFrontend startFrontend, int boardID)
        {
            InitializeComponent();
            StartFrontend = startFrontend;
            this.boardVM = new(StartFrontend, boardID);
            DataContext = boardVM;
        }

     

        private void Button_Click_back(object sender, RoutedEventArgs e)
        {

            this.Close();

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DataGrid_SelectionChanged_2(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
