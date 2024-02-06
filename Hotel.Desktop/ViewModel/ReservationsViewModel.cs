using Hotel.Data.Data.Booking;
using Hotel.Data.Data.Desktop;
using Hotel.Desktop.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Desktop.ViewModel
{
    public class ReservationsViewModel : BaseViewModel
    {
        private readonly IReservationRepository _reservationRepository;

        public ReservationsViewModel(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
            // Get the current date without the time component
            DateTime today = DateTime.Today;

            // Filter reservations for today's CheckIn
            Reservation = _reservationRepository.GetReservations()
                .Where(r => r.CheckIn.Date <= today && today <= r.CheckOut.Date)
                .ToList();
        }

        public List<Reservation> Reservation { get; set; }
    }
}
