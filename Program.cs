    using System.Text;
    using SmartMed.Interfaces;
    using SmartMed.Services;

    namespace SmartMed
    {
        class Program
        {
            static void Main(string[] args)
            {
                Console.OutputEncoding = System.Text.Encoding.Unicode;
                Console.InputEncoding = System.Text.Encoding.Unicode;

                string basePath = "JsonData";
                Directory.CreateDirectory(basePath); // Створюємо директорію, якщо її немає

                string patientsFilePath = $"{basePath}/patients.json";
                string doctorsFilePath = $"{basePath}/doctors.json";
                string appointmentsFilePath = $"{basePath}/appointments.json";
                string medicationsFilePath = $"{basePath}/medications.json";

                JsonDataService jsonDataService = new JsonDataService(patientsFilePath, doctorsFilePath, appointmentsFilePath, medicationsFilePath);
                IUserService userService = new UserService(jsonDataService);
                IPatientService patientService = new PatientService(jsonDataService);
                IDoctorService doctorService = new DoctorService(jsonDataService);

                Menu.Menu menu = new Menu.Menu(userService, patientService, doctorService);
                menu.DisplayMainMenu();
            }
        }
    }