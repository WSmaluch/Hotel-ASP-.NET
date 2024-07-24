using GalaSoft.MvvmLight.Command;
using Hotel.Data;
using Hotel.Data.Data.Desktop;
using Hotel.Desktop.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace Hotel.Desktop
{
    /// <summary>
    /// Interaction logic for CleaningTaskView.xaml
    /// </summary>
    public partial class CleaningTaskView : UserControl
    {
		private readonly HotelContext _context;
		private readonly ReservationRepository reservationRepository;
		public CleaningTaskView()
        {
            InitializeComponent();

            _context = DbContextFactory.CreateContext();

            reservationRepository = new ReservationRepository(_context);

			CleaningTaskViewModel viewModel = new CleaningTaskViewModel(reservationRepository);
            DataContext = viewModel;

			// Ładowanie danych
			LoadData();
		}

		private void LoadData()
		{
			var repairsTasksWithEmployees = reservationRepository.GetCleaningTasks();

			CleaningTaskListView.ItemsSource = repairsTasksWithEmployees;
		}

		private void CleaningTaskListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			var listView = (ListView)sender;
			var selectedCleaningTask = (CleaningTask)listView.SelectedItem;
			Window popupWindow = null;
			if (selectedCleaningTask != null)
			{
				popupWindow = new Window
				{
					Width = 400,
					Height = 260,
					Content = new StackPanel
					{
						Background = Brushes.LightGray,
						Margin = new Thickness(10),
						Children =
						{
							new TextBlock
							{
								Text = $"Repair ID: {selectedCleaningTask.Id}",
								FontWeight = FontWeights.Bold,
								Margin = new Thickness(0, 0, 0, 10),
							},
							new TextBlock
							{
								Text = $"Scheduled Date: {selectedCleaningTask.ScheduledDate}",
								Margin = new Thickness(0, 0, 0, 10),
							},
							new TextBlock
							{
								Text = $"Room ID: {selectedCleaningTask.RoomId}",
								Margin = new Thickness(0, 0, 0, 10),
							},
							new Button
							{
								Content = selectedCleaningTask.StatusId == 9 ? "To clean" : "Cleaned",
								Command = new RelayCommand<object>(o => ToggleStatus(selectedCleaningTask,popupWindow)),
								Margin = new Thickness(0, 0, 0, 0),
							},
						}
					}
				};

				popupWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;

				popupWindow.ShowDialog();
			}
		}


		private void ToggleStatus(CleaningTask cleaningTask, Window popout)
		{
			cleaningTask.StatusId = cleaningTask.StatusId == 9 ? 8 : 9;

			var statusEntity = _context.Status.SingleOrDefault(s => s.StatusId == cleaningTask.StatusId);

			if (statusEntity != null)
			{
				cleaningTask.StatusId = statusEntity.StatusId;

				_context.SaveChanges();

				popout.Close();

				LoadData();
			}
			else
			{
				MessageBox.Show("Cannot change the status.", "Warining", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}
	}
}
