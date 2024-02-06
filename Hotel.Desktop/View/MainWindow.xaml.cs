using Hotel.Data;
using Hotel.Desktop.ViewModel;
using Microsoft.EntityFrameworkCore;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hotel.Desktop.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonPopUpLogout_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        //private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        //{
        //    ButtonOpenMenu.Visibility = Visibility.Collapsed;
        //    ButtonCloseMenu.Visibility = Visibility.Visible;
        //}

        //private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        //{
        //    ButtonOpenMenu.Visibility = Visibility.Visible;
        //    ButtonCloseMenu.Visibility = Visibility.Collapsed;
        //}

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ListViewMenu.SelectedIndex;

            switch (index)
            {
                case 0:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new ReservationsView());
                    break;
                case 1:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new CleaningTaskView());
                    break;
                case 2:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new RepairsView());
                    break;
                default:
                    break;
            }
        }

        private void ExitButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ExitButton.Background = new SolidColorBrush(Colors.Red);
        }

        private void ExitButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ExitButton.Background = new SolidColorBrush(Color.FromRgb(33, 150, 243));
        }
    }
}
