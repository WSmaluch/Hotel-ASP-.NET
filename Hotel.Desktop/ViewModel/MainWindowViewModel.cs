using Hotel.Data.Data.Booking;
using Hotel.Data.Data.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Desktop.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly IReservationRepository _reservationRepository;

        public MainWindowViewModel(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;

            // Domyślnie ustaw widok Home
            CurrentViewModel = new HomeViewModel();
        }
        public void ShowHomeView()
        {
            CurrentViewModel = new HomeViewModel();
        }

        public void ShowCreateView()
        {
            CurrentViewModel = new CreateViewModel();
        }


        public List<Reservation> Reservations => _reservationRepository.GetReservations();
        
        private BaseViewModel _currentViewModel;

        public BaseViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(() => CurrentViewModel);
            }
        }
    }
}
