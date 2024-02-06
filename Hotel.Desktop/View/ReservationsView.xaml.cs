using Hotel.Data;
using Hotel.Data.Data.Booking;
using Hotel.Desktop.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for Reservations.xaml
    /// </summary>
    public partial class ReservationsView : UserControl
    {
        private HotelContext hotelContext;
        public ReservationsView()
        {
            InitializeComponent();

            // Utwórz instancję opcji dla HotelContext
            var options = new DbContextOptionsBuilder<HotelContext>()
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=HotelContext2023;Trusted_Connection=True;MultipleActiveResultSets=true")
                .LogTo(Console.WriteLine) // Dodaj to logowanie do konsoli
                .Options;

            // Utwórz instancję HotelContext i przekaż opcje jako parametr
            hotelContext = new HotelContext(options);

            ReservationsViewModel viewModel = new ReservationsViewModel(new ReservationRepository(hotelContext));
            DataContext = viewModel;
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Pobierz zaznaczoną rezerwację
            var listView = (ListView)sender;
            var selectedReservation = (Reservation)listView.SelectedItem;

            if (selectedReservation != null)
            {
                // Utwórz nowe okno z informacjami o rezerwacji
                Window popupWindow = new Window
                {
                    Width = 400,
                    Height = 300,
                    Content = new StackPanel
                    {
                        Background = Brushes.LightGray,
                        Margin = new Thickness(10),
                        Children =
                {
                    new TextBlock
                    {
                        Text = $"Reservation ID: {selectedReservation.IdReservation}",
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(0, 0, 0, 10),
                    },
                    new TextBlock
                    {
                        Text = $"Room ID: {selectedReservation.RoomId}",
                    },
                    new TextBlock
                    {
                        Text = $"Name: {selectedReservation.Name}",
                    },
                    new TextBlock
                    {
                        Text = $"Check-In: {selectedReservation.CheckIn.ToString("dd/MM/yyyy")}",
                    },
                    new TextBlock
                    {
                        Text = $"Check-Out: {selectedReservation.CheckOut.ToString("dd/MM/yyyy")}",
                    },
                    new TextBlock
                    {
                        Text = $"Number of Adults: {selectedReservation.NumberOfAdults}",
                    },
                    new TextBlock
                    {
                        Text = $"Number of Children: {selectedReservation.NumberOfChildren}",
                    },
                    new TextBlock
                    {
                        Text = $"Special Requests: {selectedReservation.SpecialRequests}",
                    },
                    new TextBlock
                    {
                        Text = $"Total Price: {selectedReservation.TotalPrice}",
                    },
                    new TextBlock
                    {
                        Text = $"Option ID: {hotelContext.Options.FirstOrDefault(r => r.IdOption == selectedReservation.OptionId)?.Name}",
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




    }
}
