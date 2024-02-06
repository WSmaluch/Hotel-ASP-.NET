using Hotel.Data.Data.Booking;
using Hotel.Data.Data.Desktop;
using Hotel.Data.Data.Employess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Desktop
{
    public interface IReservationRepository
    {
        List<Reservation> GetReservations();
        List<CleaningTask> GetCleaningTasks();
        List<Cleaner> GetCleaners();
        List<Room> GetRooms();
        List<RepairTask>? GetReapairTasks();
        List<RepairTask>? GetRepairsTasksWithEmployees();
        List<Employee>? GetEmployees();
    }
}
