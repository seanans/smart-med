using System.Text;
using SmartMed.Interfaces;
using SmartMed.Models;
using SmartMed.Services;

namespace SmartMed.Menu;

public class Menu
{
    private readonly IDoctorService _doctorService;
    private readonly IPatientService _patientService;
    private readonly Dictionary<string, List<string>> _symptomProfiles;
    private readonly IUserService _userService;

    public Menu(IUserService userService, IPatientService patientService, IDoctorService doctorService,
        JsonDataService jsonDataService)
    {
        _userService = userService;
        _patientService = patientService;
        _doctorService = doctorService;
        _symptomProfiles = jsonDataService.LoadSymptomProfiles();
    }

    public void DisplayMainMenu()
    {
        while (true)
        {
            Console.WriteLine("1. Увійти");
            Console.WriteLine("2. Зареєструватися");
            Console.WriteLine("3. Вийти");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    var user = _userService.SignIn();
                    if (user is Patient)
                        DisplayPatientMenu(user as Patient);
                    else if (user is Doctor)
                        DisplayDoctorMenu(user as Doctor);

                    break;
                case "2":
                    _userService.SignUp();
                    break;
                case "3":
                    return;
                case "4":
                    File.WriteAllText("JsonData/patients.json", "[]", Encoding.UTF8);
                    File.WriteAllText("JsonData/doctors.json", "[]", Encoding.UTF8);
                    File.WriteAllText("JsonData/appointments.json", "[]", Encoding.UTF8);
                    File.WriteAllText("JsonData/medications.json", "[]", Encoding.UTF8);
                    break;
                default:
                    Console.WriteLine("Неправильний вибір. Спробуйте ще раз.");
                    break;
            }
        }
    }

    private void DisplayPatientMenu(Patient patient)
    {
        while (true)
        {
            Console.WriteLine("1. Записатися на прийом");
            Console.WriteLine("2. Переглянути медичну карту");
            Console.WriteLine("3. Скасувати запис");
            Console.WriteLine("4. Вийти");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateAppointment(patient);
                    break;
                case "2":
                    // DisplayMedicalRecord(patient);
                    break;
                case "3":
                    //CancelAppointment(patient);
                    break;
                case "4":
                    _userService.SignOut();
                    return;
                default:
                    Console.WriteLine("Неправильний вибір. Спробуйте ще раз.");
                    break;
            }
        }
    }


    private void DisplayDoctorMenu(Doctor doctor)
    {
        while (true)
        {
            Console.WriteLine("1. Переглянути поточні записи");
            Console.WriteLine("2. Прийняти пацієнта");
            Console.WriteLine("3. Провести огляд");
            Console.WriteLine("4. Записати діагноз");
            Console.WriteLine("5. Вийти");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    // DisplayAppointments(doctor);
                    break;
                case "2":
                    //AcceptPatient(doctor);
                    break;
                case "3":
                    // ConductExamination(doctor);
                    break;
                case "4":
                    //RecordDiagnosis(doctor);
                    break;
                case "5":
                    _userService.SignOut();
                    return;
                default:
                    Console.WriteLine("Неправильний вибір. Спробуйте ще раз.");
                    break;
            }
        }
    }

    private void CreateAppointment(Patient patient)
    {
        Console.Write("Опис симптомів: ");
        var symptoms = Console.ReadLine();

        var suitableDoctors = FindDoctorsBySymptoms(symptoms);
        if (!suitableDoctors.Any())
        {
            Console.WriteLine("Немає доступних лікарів для ваших симптомів.");
            return;
        }

        Doctor chosenDoctor;
        while ((chosenDoctor = ChooseDoctor(suitableDoctors)) == null)
            Console.WriteLine("Неправильний вибір лікаря. Спробуйте ще раз.");

        string chosenDay;
        while ((chosenDay = ChooseDay(chosenDoctor)) == null)
            Console.WriteLine("Неправильний вибір дня. Спробуйте ще раз.");

        var availableTimes = GetAvailableTimes(chosenDoctor, chosenDay);
        if (!availableTimes.Any())
        {
            Console.WriteLine("Немає доступних годин для обраного дня.");
            return;
        }

        string chosenTime;
        while ((chosenTime = ChooseTime(availableTimes)) == null)
            Console.WriteLine("Неправильний вибір часу. Спробуйте ще раз.");

        var chosenDateTime = DateTime.ParseExact($"{chosenDay} {chosenTime}", "dddd HH:mm", null);
        var appointment = new Appointment
        {
            DoctorId = chosenDoctor.Id,
            PatientId = patient.Id,
            DateTime = chosenDateTime,
            Symptoms = symptoms,
            AppointmentStatus = AppointmentStatus.Scheduled
        };

        _patientService.AddAppointment(patient.Id, appointment);
        _doctorService.AddAppointment(appointment);
        Console.WriteLine("Запис успішно створений.");
    }

    private string? ChooseDay(Doctor doctor)
    {
        Console.WriteLine("Доступні дні для запису:");
        foreach (var day in doctor.Schedule.Keys) Console.WriteLine(day);
        Console.Write("Виберіть день (введіть назву дня): ");
        var chosenDay = Console.ReadLine();
        return doctor.Schedule.ContainsKey(chosenDay) ? chosenDay : null;
    }

    private List<string> GetAvailableTimes(Doctor doctor, string chosenDay)
    {
        var availableTimes = new List<string>(doctor.Schedule[chosenDay]);
        foreach (var appointment in doctor.Appointments)
            if (appointment.DateTime.ToString("dddd") == chosenDay)
                availableTimes.Remove(appointment.DateTime.ToString("HH:mm"));
        return availableTimes;
    }

    private Doctor? ChooseDoctor(List<Doctor> suitableDoctors)
    {
        Console.WriteLine("Доступні лікарі для ваших симптомів:");
        for (var i = 0; i < suitableDoctors.Count; i++)
            Console.WriteLine(
                $"{i + 1}. {suitableDoctors[i].FullName} - Профілі: {string.Join(", ", suitableDoctors[i].Profiles)}");
        Console.Write("Виберіть лікаря (введіть номер): ");
        if (int.TryParse(Console.ReadLine(), out var doctorIndex) && doctorIndex > 0 &&
            doctorIndex <= suitableDoctors.Count) return suitableDoctors[doctorIndex - 1];
        return null;
    }

    private string ChooseTime(List<string> availableTimes)
    {
        Console.WriteLine("Доступні години для запису:");
        for (var i = 0; i < availableTimes.Count; i++) Console.WriteLine($"{i + 1}. {availableTimes[i]}");
        Console.Write("Виберіть час (введіть номер): ");
        if (int.TryParse(Console.ReadLine(), out var timeIndex) && timeIndex > 0 && timeIndex <= availableTimes.Count)
            return availableTimes[timeIndex - 1];
        return null;
    }

    private List<Doctor> FindDoctorsBySymptoms(string symptoms)
    {
        var allDoctors = _doctorService.LoadDoctors();
        var suitableDoctors = new List<Doctor>();

        var symptomWords = symptoms.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var matchedProfiles = new HashSet<string>();

        foreach (var word in symptomWords)
        foreach (var profile in _symptomProfiles)
            if (profile.Value.Any(symptom => symptom.Contains(word, StringComparison.OrdinalIgnoreCase)))
                matchedProfiles.Add(profile.Key);

        foreach (var doctor in allDoctors)
            if (doctor.Profiles.Any(profile => matchedProfiles.Contains(profile)))
                suitableDoctors.Add(doctor);

        return suitableDoctors;
    }
}