using System.Text;
using SmartMed.Interfaces;
using SmartMed.Services;

namespace SmartMed;

internal static class Program
{
    private static void Main()
    {
        Console.OutputEncoding = Encoding.Unicode;
        Console.InputEncoding = Encoding.Unicode;

        const string basePath = "JsonData";
        Directory.CreateDirectory(basePath); // Створюємо директорію, якщо її немає

        const string patientsFilePath = $"{basePath}/patients.json";
        const string doctorsFilePath = $"{basePath}/doctors.json";
        const string appointmentsFilePath = $"{basePath}/appointments.json";
        const string medicationsFilePath = $"{basePath}/medications.json";

        var jsonDataService =
            new JsonDataService(patientsFilePath, doctorsFilePath, appointmentsFilePath, medicationsFilePath);
        IUserService userService = new UserService(jsonDataService);
        IPatientService patientService = new PatientService(jsonDataService);
        IDoctorService doctorService = new DoctorService(jsonDataService);

        var menu = new Menu.Menu(userService, patientService, doctorService);
        menu.DisplayMainMenu();
    }
}