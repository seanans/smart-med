using System.Text;
using SmartMed.Interfaces;
using SmartMed.Services;

namespace SmartMed;

/// <summary>
///     Головний клас програми. Відповідає за ініціалізацію сервісів та запуск головного меню.
/// </summary>
internal static class Program
{
    /// <summary>
    ///     Основний метод програми, який виконує ініціалізацію та запуск головного меню.
    /// </summary>
    private static void Main()
    {
        //Ввід/вивід консоль в Unicode
        Console.OutputEncoding = Encoding.Unicode;
        Console.InputEncoding = Encoding.Unicode;

        //Json файли (bin/Debug/net8.0/JsonData)
        const string basePath = "JsonData";
        Directory.CreateDirectory(basePath);

        const string patientsFilePath = $"{basePath}/patients.json";
        const string doctorsFilePath = $"{basePath}/doctors.json";
        const string appointmentsFilePath = $"{basePath}/appointments.json";
        const string medicationsFilePath = $"{basePath}/medications.json";
        const string symptomsProfilesFilePath = $"{basePath}/symptoms_profiles.json";
        const string medicalRecordsFilePath = $"{basePath}/medical_records.json";
        const string diseasesFilePath = $"{basePath}/diseases.json";


        var jsonDataService =
            new JsonDataService(patientsFilePath, doctorsFilePath, appointmentsFilePath, medicationsFilePath,
                symptomsProfilesFilePath, diseasesFilePath, medicalRecordsFilePath);
        IUserService userService = new UserService(jsonDataService);
        IPatientService patientService = new PatientService(jsonDataService);
        IDoctorService doctorService = new DoctorService(jsonDataService);
        IMedicalRecordService medicalRecordService = new MedicalRecordService(jsonDataService);
        IAppointmentService appointmentService = new AppointmentService(jsonDataService);

        var menu = new Menu.Menu(userService, patientService, doctorService, jsonDataService, medicalRecordService,
            appointmentService);
        menu.DisplayMainMenu();
    }
}