using Hotel.Data.Data.Booking;
using Hotel.Data.Data.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Desktop.ViewModel
{
    public class CleaningTaskViewModel : BaseViewModel
    {
        private readonly IReservationRepository _reservationRepository;

        public CleaningTaskViewModel(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
            CleaningTasks = _reservationRepository.GetCleaningTasks();
        }

        public List<CleaningTask> CleaningTasks { get; set; }
    }
}
