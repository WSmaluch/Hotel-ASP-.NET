using Hotel.Data;
using Hotel.Data.Data.Booking;
using Hotel.Desktop.ViewModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Hotel.Desktop.View
{
    public partial class ReservationsView : UserControl
    {
        private HotelContext hotelContext;

        public ReservationsView()
        {
            InitializeComponent();
            hotelContext = DbContextFactory.CreateContext();

            // Tworzenie i ustawianie ViewModel
            ReservationsViewModel viewModel = new ReservationsViewModel(new ReservationRepository(hotelContext));
            DataContext = viewModel;
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var listView = (ListView)sender;
            var selectedReservation = (Reservation)listView.SelectedItem;

            if (selectedReservation != null)
            {
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

                popupWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                popupWindow.ShowDialog();
            }
        }
    }
}
