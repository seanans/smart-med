using System.Globalization;
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
    private readonly IAppointmentService _appointmentService;
    private readonly IMedicalRecordService _medicalRecordService;

    public Menu(IUserService userService, IPatientService patientService, IDoctorService doctorService,
        JsonDataService jsonDataService, IMedicalRecordService medicalRecordService, IAppointmentService appointmentService)
    {
        _userService = userService;
        _patientService = patientService;
        _doctorService = doctorService;
        _medicalRecordService = medicalRecordService;
        _appointmentService = appointmentService;
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
                    DisplayMedicalRecord(patient);
                    break;
                case "3":
                    CancelAppointment(patient);
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

    private void CancelAppointment(Patient patient)
    {
        var appointments = _appointmentService.GetAppointmentsByPatient(patient.Id)
            .Where(a => a.AppointmentStatus == AppointmentStatus.Scheduled).ToList();
        if (!appointments.Any())
        {
            Console.WriteLine("У вас немає запланованих зустрічей.");
            return;
        }
        
        Console.WriteLine("Ваші заплановані зустрічі:");
        for (int i = 0; i < appointments.Count; i++)
        {
            var doctor = _doctorService.getDoctorById(appointments[i].DoctorId);
            Console.WriteLine($"{i + 1}. Лікар: {doctor.FullName}, Дата та час: {appointments[i].DateTime}");
        }
        
        Console.Write("Виберіть зустріч для скасування (введіть номер): ");
        if (int.TryParse(Console.ReadLine(), out var appointmentIndex) && appointmentIndex > 0 && appointmentIndex <= appointments.Count)
        {
            var appointment = appointments[appointmentIndex - 1];
            _appointmentService.CancelAppointment(appointment.Id);
            Console.WriteLine("Зустріч скасовано.");
        }
        
    }

    private void DisplayMedicalRecord(Patient patient)
    {
        while (true)
        {
            Console.WriteLine("Медична карта:");
            Console.WriteLine("1. Переглянути хвороби");
            Console.WriteLine("2. Переглянути зустрічі");
            Console.WriteLine("3. Призначені ліки");
            Console.WriteLine("4. Назад");

            var choice = Console.ReadLine();
            var medicalRecord = _medicalRecordService.GetMedicalRecord(patient.Id);
            switch (choice)
            {
                case "1":
                    DisplayDiseases(medicalRecord.Diseases);
                    break;
                case "2":
                    DisplayAppointments(patient.Id);
                    break;
                case "3":
                    DisplayMedications(medicalRecord.Medications);
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Неправильний вибір. Спробуйте ще раз.");
                    break;
            }
        }
    }
    
    private void DisplayDiseases(List<Disease> diseases)
    {
        if (!diseases.Any())
        {
            Console.WriteLine("Немає зареєстрованих хвороб.");
            return;
        }

        Console.WriteLine("Хвороби:");
        for (int i = 0; i < diseases.Count; i++)
        {
            var disease = diseases[i];
            Console.WriteLine($"{i + 1}. Назва: {disease.Name}, Опис: {disease.Description}, Дата діагнозу: {disease.DiagnosisDate}");
        }

        Console.WriteLine("Натисніть будь-яку клавішу для повернення до медичної карти.");
        Console.ReadKey();
    }
    
    private void DisplayAppointments(int patientId)
    {
        var appointments = _appointmentService.GetAppointmentsByPatient(patientId);
        if (!appointments.Any())
        {
            Console.WriteLine("Немає запланованих зустрічей.");
            return;
        }

        Console.WriteLine("Зустрічі:");
        for (int i = 0; i < appointments.Count; i++)
        {
            var appointment = appointments[i];
            Console.WriteLine($"{i + 1}. Дата: {appointment.DateTime}, Симптоми: {appointment.Symptoms}, Статус: {appointment.AppointmentStatus}");
        }

        Console.WriteLine("Натисніть будь-яку клавішу для повернення до медичної карти.");
        Console.ReadKey();
    }
    
    private void DisplayMedications(List<Medication> medications)
    {
        if (!medications.Any())
        {
            Console.WriteLine("Немає призначених ліків.");
            return;
        }

        Console.WriteLine("Ліки:");
        for (int i = 0; i < medications.Count; i++)
        {
            var medication = medications[i];
            Console.WriteLine($"{i + 1}. Назва: {medication.Name}, Опис: {medication.Description}, Дозування: {medication.Dosage}");
        }

        Console.WriteLine("Натисніть будь-яку клавішу для повернення до медичної карти.");
        Console.ReadKey();
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
                    DisplayAppointments(doctor);
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

    private void DisplayAppointments(Doctor doctor)
    {
        var appointments = _appointmentService.GetAppointmentsByDoctor(doctor.Id)
            .Where(a => a.AppointmentStatus == AppointmentStatus.Scheduled).ToList();
        
        if (!appointments.Any())
        {
            Console.WriteLine("У вас немає запланованих зустрічей.");
            return;
        }
        
        Console.WriteLine("Ваші заплановані зустрічі:");
        foreach (var appointment in appointments)
        {
            var patient = _patientService.LoadPatients().FirstOrDefault(p => p.Id == appointment.PatientId);
            Console.WriteLine($"Пацієнт: {patient?.FullName ?? "Невідомий"}, Дата та час: {appointment.DateTime}, Симптоми: {appointment.Symptoms}, Статус: {appointment.AppointmentStatus}");
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

        DateTime chosenDate;
        while ((chosenDate = ChooseDate(chosenDoctor)) == default)
            Console.WriteLine("Неправильний вибір дати. Спробуйте ще раз.");

        var availableTimes = GetAvailableTimes(chosenDoctor, chosenDate);
        if (!availableTimes.Any())
        {
            Console.WriteLine("Немає доступних годин для обраного дня.");
            return;
        }

        string chosenTime;
        while ((chosenTime = ChooseTime(availableTimes)) == null)
            Console.WriteLine("Неправильний вибір часу. Спробуйте ще раз.");
        var chosenDateTime = DateTime.ParseExact($"{chosenDate:yyyy-MM-dd} {chosenTime}", "yyyy-MM-dd HH:mm", null);
        
        _appointmentService.ScheduleAppointment(chosenDoctor.Id, patient.Id, chosenDateTime, symptoms);
        Console.WriteLine("Запис успішно створений.");
    }

    private DateTime ChooseDate(Doctor doctor)
    {
        Console.WriteLine("Доступні дні для запису:");
        var availableDates = new List<DateTime>();
        for (var i = 0; i < 14; i++)
        {
            var date = DateTime.Today.AddDays(i);
            if (doctor.Schedule.ContainsKey(date.ToString("dddd", new CultureInfo("uk-UA"))))
            {
                availableDates.Add(date);
                Console.WriteLine(
                    $"{availableDates.Count}. {date:yyyy-MM-dd} ({date.ToString("dddd", new CultureInfo("uk-UA"))})");
            }
        }

        Console.Write("Виберіть день (введіть номер): ");
        if (int.TryParse(Console.ReadLine(), out var dayIndex) && dayIndex > 0 && dayIndex <= availableDates.Count)
            return availableDates[dayIndex - 1];

        return default;
    }

    private List<string> GetAvailableTimes(Doctor doctor, DateTime chosenDate)
    {
        var day = chosenDate.ToString("dddd", new CultureInfo("uk-UA"));
        var availableTimes = new List<string>(doctor.Schedule[day]);
        var doctorAppointments = _appointmentService.GetAppointmentsByDoctor(doctor.Id);
        var doctorAppointmentsForDay = doctorAppointments
            .Where(a => a.DateTime.Date == chosenDate.Date)
            .ToList();
        
        foreach (var appointment in doctorAppointmentsForDay)
        {
            availableTimes.Remove(appointment.DateTime.ToString("HH:mm"));
        }
        return availableTimes;
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
}