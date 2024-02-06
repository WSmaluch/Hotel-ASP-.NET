using Hotel.Data.Data.Desktop;
using Hotel.Desktop.ViewModel;
using Hotel.Desktop;
using System.Collections.Generic;
using System.Linq;

public class RepairsViewModel : BaseViewModel
{
    private readonly IReservationRepository _reservationRepository;

    public RepairsViewModel(IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
        RepairsTasks = GetRepairsTasksWithEmployees();
    }

    public List<RepairTask> RepairsTasks { get; set; }

    public List<RepairTask> GetRepairsTasksWithEmployees()
    {
        var tasksWithEmployees = _reservationRepository.GetRepairsTasksWithEmployees();
        var firstEmployee = tasksWithEmployees.FirstOrDefault()?.Employee;
        Imie = firstEmployee != null ? firstEmployee.FirstName : null;

        return tasksWithEmployees;
    }

    private string imie;

    public string Imie
    {
        get { return imie; }
        set { imie = value; }
    }

}
