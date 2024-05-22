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
    private readonly JsonDataService _jsonDataService;

    public Menu(IUserService userService, IPatientService patientService, IDoctorService doctorService,
        JsonDataService jsonDataService, IMedicalRecordService medicalRecordService, IAppointmentService appointmentService)
    {
        _userService = userService;
        _patientService = patientService;
        _doctorService = doctorService;
        _jsonDataService = jsonDataService;
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
                    DisplayDiseases(medicalRecord.DiseasesRecords);
                    break;
                case "2":
                    DisplayAppointments(patient.Id);
                    break;
                case "3":
                    DisplayMedications(medicalRecord.DiseasesRecords);
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Неправильний вибір. Спробуйте ще раз.");
                    break;
            }
        }
    }
    
    private void DisplayDiseases(List<DiseaseRecord> diseaseRecords)
    {
        if (!diseaseRecords.Any())
        {
            Console.WriteLine("Немає зареєстрованих хвороб.");
            return;
        }

        var diseases = _jsonDataService.LoadDiseases();

        Console.WriteLine("Хвороби:");
        for (int i = 0; i < diseaseRecords.Count; i++)
        {
            var diseaseRecord = diseaseRecords[i];
            var disease = diseases.FirstOrDefault(d => d.Id == diseaseRecord.DiseaseId);

            if (disease != null)
            {
                Console.WriteLine($"{i + 1}. Назва: {disease.Name}");
                Console.WriteLine($"   Опис: {disease.Description}");
                Console.WriteLine($"   Симптоми: {string.Join(", ", disease.Symptoms)}");
                Console.WriteLine($"   Лікар: {_doctorService.getDoctorById(diseaseRecord.DoctorId).FullName}");
                Console.WriteLine($"   Статус: {diseaseRecord.Status}");
                Console.WriteLine($"   Дата діагнозу: {diseaseRecord.DateDiagnosed}");
                Console.WriteLine($"   Дата завершення: {(diseaseRecord.Status == DiseaseStatus.Recovered ? diseaseRecord.DateRecovered.ToString() : "Ще не завершено")}");
                Console.WriteLine($"   Симптоми пацієнта: {diseaseRecord.Symptoms}");
                Console.WriteLine($"   Лікування:");
                foreach (var treatment in diseaseRecord.Treatments)
                {
                    var medication = _jsonDataService.LoadMedications().FirstOrDefault(m => m.Id == treatment.MedicationId);
                    if (medication != null)
                    {
                        Console.WriteLine($"      Ліки: {medication.Name}, Дозування: {treatment.Dosage}");
                    }
                }
            }
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
    
    private void DisplayMedications(List<DiseaseRecord> diseaseRecords)
    {
        if (!diseaseRecords.Any())
        {
            Console.WriteLine("Немає призначених ліків.");
            return;
        }
        
        var medications = _jsonDataService.LoadMedications();
        var medicationDTOs = new List<MedicationDTO>();
        foreach (var diseaseRecord in diseaseRecords)
        {
            foreach (var treatment in diseaseRecord.Treatments)
            {
                var medication = medications.FirstOrDefault(m => m.Id == treatment.MedicationId);
                if (medication != null)
                {
                    medicationDTOs.Add(new MedicationDTO
                    {
                        Name = medication.Name,
                        Description = medication.Description,
                        Dosage = treatment.Dosage,
                        TreatmentStatus = treatment.TreatmentStatus
                    });
                }

            }
        }
        
        if (!medicationDTOs.Any())
        {
            Console.WriteLine("Немає призначених ліків.");
            return;
        }

        Console.WriteLine("Ліки:");
        foreach (var medicationDTO in medicationDTOs)
        {
            Console.WriteLine($"Назва: {medicationDTO.Name}");
            Console.WriteLine($"Опис: {medicationDTO.Description}");
            Console.WriteLine($"Дозування: {medicationDTO.Dosage}");
            Console.WriteLine($"Статус: {medicationDTO.TreatmentStatus}");
            Console.WriteLine(new string('-', 20));
        }

        Console.WriteLine("Натисніть будь-яку клавішу для повернення до медичної карти.");
        Console.ReadKey();
    }

    private void DisplayDoctorMenu(Doctor doctor)
    {
        while (true)
        {
            Console.WriteLine("1. Переглянути поточні записи");
            Console.WriteLine("2. Вийти");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    DisplayAppointments(doctor);
                    break;
                case "2":
                    _userService.SignOut();
                    return;
                default:
                    Console.WriteLine("Неправильний вибір. Спробуйте ще раз.");
                    break;
            }
        }
    }

    private void AcceptPatient(Doctor doctor, Appointment appointment)
    {
        while (true)
        {
            Console.WriteLine("1. Переглянути картку пацієнта");
            Console.WriteLine("2. Провести огляд");
            Console.WriteLine("3. Назад");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    var patient = _patientService.LoadPatients().FirstOrDefault(p => p.Id == appointment.PatientId);
                    if (patient != null)
                        DisplayMedicalRecord(patient);
                    break;
                case "2":
                    ConductExamination(doctor, appointment);
                    return;
                case "3":
                    return;
                default:
                    Console.WriteLine("Неправильний вибір. Спробуйте ще раз.");
                    break;
            }
        }
    }

    private void ConductExamination(Doctor doctor, Appointment appointment)
    {
        Console.WriteLine("Симптоми вказані при зверненні:" + appointment.Symptoms);
        Console.WriteLine("Рекомендації по огляду: ...");
        Thread.Sleep(1000);
        Console.WriteLine("1.-------");
        Console.WriteLine("2.-------");
        Console.WriteLine("3.-------");
        
        Console.WriteLine("Введіть усі симптоми пацієнта:"); //Лікар вводить симптоми і скарги пацієнта, які виявив під час огляду
        var symptoms = Console.ReadLine();
        var diseases = _jsonDataService.LoadDiseases();
        var medications = _jsonDataService.LoadMedications();
        var possibleDiseases = diseases
            .Where(d => d.Symptoms.Any(s => symptoms.Contains(s, StringComparison.OrdinalIgnoreCase)))
            .ToList();
        
        if (possibleDiseases.Any())
        {
            Console.WriteLine("Ось можливий діагноз:");
            foreach (var possibleDisease in possibleDiseases)
            {
                Console.WriteLine($"- {possibleDisease.Name}");
            }
        }
        else
        {
            Console.WriteLine("Жодної хвороби не знайдено за вказаними симптомами.");
        }
        Console.Write("Введіть назву хвороби: "); //Лікар враховуючи або не враховуючи запропонований діагноз вводить хворобу яку приєднає до запису в картці
        var diseaseName = Console.ReadLine();
        
        var disease = diseases.FirstOrDefault(d => d.Name.Equals(diseaseName, StringComparison.OrdinalIgnoreCase));
        if (disease == null)
        {
            Console.WriteLine("Хворобу не знайдено.");
            return;
        }
        
        var diseaseRecord = new DiseaseRecord
        {
            DiseaseId = disease.Id,
            DiseaseName = disease.Name,
            DiseaseDescription = disease.Description,
            DoctorId = doctor.Id,
            PatientId = appointment.PatientId,
            Symptoms = symptoms,
            Status = DiseaseStatus.Sick,
            DateDiagnosed = DateTime.Now
        };
        Console.WriteLine("Рекомендовані ліки на основі симптомів:");
        var recommendedMedications = medications
            .Where(m => m.EffectiveSymptoms.Any(s => symptoms.Contains(s, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        if (recommendedMedications.Any())
        {
            foreach (var medication in recommendedMedications)
            {
                Console.WriteLine($"- {medication.Name}: {medication.Description} (Рекомендоване дозування: {medication.RecommendedDosage})");
            }
        }
        else
        {
            Console.WriteLine("Немає рекомендованих ліків на основі вказаних симптомів.");
        }
        
        while (true)
        {
            Console.Write("Додати лікування? (так/ні): ");
            var addTreatment = Console.ReadLine();
            if (addTreatment?.ToLower() != "так") break;

            Console.Write("Введіть назву ліків: ");
            var medicationName = Console.ReadLine();
            var medication = medications.FirstOrDefault(m => m.Name.Equals(medicationName, StringComparison.OrdinalIgnoreCase));

            if (medication == null)
            {
                Console.WriteLine("Ліки не знайдено.");
                continue;
            }

            Console.Write("Введіть дозування: ");
            var dosage = Console.ReadLine();

            diseaseRecord.Treatments.Add(new Treatment
            {
                MedicationId = medication.Id,
                Dosage = dosage,
                TreatmentStatus = TreatmentStatus.Taking
            });
        }
        
        _medicalRecordService.AddDisease(appointment.PatientId, diseaseRecord);
        _appointmentService.CompleteAppointment(appointment.Id);
        _medicalRecordService.AddAppointment(appointment.PatientId, appointment.Id);
        _appointmentService.SaveAppointments(_appointmentService.GetAppointments());

        Console.WriteLine("Огляд завершено. Запис оновлено.");
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
        for (int i = 0; i < appointments.Count; i++)
        {
            var patient = _patientService.LoadPatients().FirstOrDefault(p => p.Id == appointments[i].PatientId);
            Console.WriteLine($"{i + 1}. Пацієнт: {patient?.FullName ?? "Невідомий"}, Дата та час: {appointments[i].DateTime}");
        }

        Console.Write("Оберіть запис який хочете прийняти (введіть номер) або натисніть будь-яку іншу клавішу для виходу: ");
        if (int.TryParse(Console.ReadLine(), out var appointmentIndex) && appointmentIndex > 0 && appointmentIndex <= appointments.Count)
        {
            var appointment = appointments[appointmentIndex - 1];
            AcceptPatient(doctor, appointment);
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