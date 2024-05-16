using System.Text;
using SmartMed.Interfaces;
using SmartMed.Services;

namespace SmartMed
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            string usersFilePath = "users.json";
            string appointmentsFilePath = "appointments.json";
            string medicationsFilePath = "medications.json";

            JsonDataService jsonDataService = new JsonDataService(usersFilePath, appointmentsFilePath, medicationsFilePath);
             IUserService userService = new UserService(jsonDataService);
             IPatientService patientService = new PatientService(jsonDataService);
             IDoctorService doctorService = new DoctorService(jsonDataService);
            
             Menu.Menu menu = new Menu.Menu(userService, patientService, doctorService);
             menu.DisplayMainMenu();
        }
    }
}