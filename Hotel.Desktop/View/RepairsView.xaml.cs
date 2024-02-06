using Hotel.Data;
using Hotel.Data.Data.Booking;
using Hotel.Data.Data.Desktop;
using Hotel.Desktop.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using GalaSoft.MvvmLight.Command;
using System.Windows.Controls.Primitives;

namespace Hotel.Desktop.View
{
    public partial class RepairsView : UserControl
    {
        private readonly HotelContext _context;
        private readonly ReservationRepository reservationRepository;

        public RepairsView()
        {
            InitializeComponent();

            // Utwórz instancję opcji dla HotelContext
            var options = new DbContextOptionsBuilder<HotelContext>()
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=HotelContext2023;Trusted_Connection=True;MultipleActiveResultSets=true")
                .LogTo(Console.WriteLine) // Dodaj to logowanie do konsoli
                .Options;

            // Utwórz instancję HotelContext i przekaż opcje jako parametr
            _context = new HotelContext(options);

            reservationRepository = new ReservationRepository(_context);

            RepairsViewModel viewModel = new RepairsViewModel(reservationRepository);
            DataContext = viewModel;

            // Ładowanie danych
            LoadData();
        }

        private void LoadData()
        {
            // Pobierz zadania naprawcze z informacjami o pracownikach i pokojach
            var repairsTasksWithEmployees = reservationRepository.GetRepairsTasksWithEmployees();
            //var repairsTasksWithEmployees = _reservationRepository.GetRepairsTasksWithEmployees();

            // Ustaw źródło danych dla widoku
            RepairsListView.ItemsSource = repairsTasksWithEmployees;
        }

        private void RepairsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Pobierz zaznaczoną rezerwację
            var listView = (ListView)sender;
            var selectedRepair = (RepairTask)listView.SelectedItem;
            Window popupWindow = null;
            if (selectedRepair != null)
            {
                // Utwórz nowe okno z informacjami o rezerwacji
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
                                Text = $"Repair ID: {selectedRepair.Id}",
                                FontWeight = FontWeights.Bold,
                                Margin = new Thickness(0, 0, 0, 10),
                            },
                            new TextBlock
                            {
                                Text = $"Scheduled Date: {selectedRepair.ScheduledDate}",
                                Margin = new Thickness(0, 0, 0, 10),
                            },
                            new TextBlock
                            {
                                Text = $"Status: {selectedRepair.StatusName}",
                                Margin = new Thickness(0, 0, 0, 10),
                            },
                            new TextBlock
                            {
                                Text = $"Room ID: {selectedRepair.RoomId}",
                                Margin = new Thickness(0, 0, 0, 10),
                            },
                            new TextBlock
                            {
                                Text = $"Employee Name: {selectedRepair.Employee?.FirstName ?? "N/A"}",
                                Margin = new Thickness(0, 0, 0, 10),
                            },
                            new TextBlock
                            {
                                Text = $"Note: {selectedRepair.Note ?? "N/A"}",
                                Margin = new Thickness(0, 0, 0, 10),
                            },
                            new Button
                            {
                                Content = selectedRepair.StatusId == 9 ? "To repair" : "Repaired",
                                Command = new RelayCommand<object>(o => ToggleStatus(selectedRepair,popupWindow)),
                                Margin = new Thickness(0, 0, 0, 0),
                            },
                        }
                    }
                };

                // Ustaw pozycję okna na środku ekranu
                popupWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                // Wyświetl okno
                popupWindow.ShowDialog();
            }
        }


        private void ToggleStatus(RepairTask repairTask,Window popout)
        {
            repairTask.StatusId = repairTask.StatusId == 9 ? 12 : 9;

            var statusEntity = _context.Status.SingleOrDefault(s => s.StatusId == repairTask.StatusId);

            if (statusEntity != null)
            {
                repairTask.StatusName = statusEntity.StatusName;

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
