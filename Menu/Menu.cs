﻿using System.Globalization;
using SmartMed.Interfaces;
using SmartMed.Models;
using SmartMed.Services;

namespace SmartMed.Menu;

public class Menu
{
    private readonly IAppointmentService _appointmentService;
    private readonly IDoctorService _doctorService;
    private readonly JsonDataService _jsonDataService;
    private readonly IMedicalRecordService _medicalRecordService;
    private readonly IPatientService _patientService;
    private readonly Dictionary<string, List<string>> _symptomProfiles;
    private readonly IUserService _userService;

    public Menu(IUserService userService, IPatientService patientService, IDoctorService doctorService,
        JsonDataService jsonDataService, IMedicalRecordService medicalRecordService,
        IAppointmentService appointmentService)
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
                    switch (user)
                    {
                        case Patient patient:
                            DisplayPatientMenu(patient);
                            break;
                        case Doctor doctor:
                            DisplayDoctorMenu(doctor);
                            break;
                    }

                    break;
                case "2":
                    _userService.SignUp();
                    break;
                case "3":
                    return;
                /*     case "4":
                         File.WriteAllText("JsonData/patients.json", "[]", Encoding.UTF8);
                         File.WriteAllText("JsonData/doctors.json", "[]", Encoding.UTF8);
                         File.WriteAllText("JsonData/appointments.json", "[]", Encoding.UTF8);
                         break; */
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
            Console.WriteLine("3. Записатися на огляд для закриття хвороби");
            Console.WriteLine("4. Скасувати запис");
            Console.WriteLine("5. Вийти");
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
                    ScheduleFollowUpAppointment(patient);
                    break;
                case "4":
                    CancelAppointment(patient);
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
        for (var i = 0; i < appointments.Count; i++)
        {
            var doctor = _doctorService.GetDoctorById(appointments[i].DoctorId);
            Console.WriteLine($"{i + 1}. Лікар: {doctor.FullName}, Дата та час: {appointments[i].DateTime}");
        }

        Console.Write("Виберіть зустріч для скасування (введіть номер): ");
        if (int.TryParse(Console.ReadLine(), out var appointmentIndex) && appointmentIndex > 0 &&
            appointmentIndex <= appointments.Count)
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
        for (var i = 0; i < diseaseRecords.Count; i++)
        {
            var diseaseRecord = diseaseRecords[i];
            var disease = diseases.FirstOrDefault(d => d.Id == diseaseRecord.DiseaseId);

            if (disease == null) continue;
            Console.WriteLine($"{i + 1}. Назва: {disease.Name}");
            Console.WriteLine($"   Опис: {disease.Description}");
            Console.WriteLine($"   Симптоми: {string.Join(", ", disease.Symptoms)}");
            Console.WriteLine($"   Лікар: {_doctorService.GetDoctorById(diseaseRecord.DoctorId).FullName}");
            Console.WriteLine($"   Статус: {diseaseRecord.Status}");
            Console.WriteLine($"   Дата діагнозу: {diseaseRecord.DateDiagnosed}");
            Console.WriteLine(
                $"   Дата завершення: {(diseaseRecord.Status == DiseaseStatus.Recovered ? diseaseRecord.DateRecovered.ToString() : "Ще не завершено")}");
            Console.WriteLine($"   Симптоми пацієнта: {diseaseRecord.Symptoms}");
            Console.WriteLine("   Лікування:");
            foreach (var treatment in diseaseRecord.Treatments)
            {
                var medication = _jsonDataService.LoadMedications()
                    .FirstOrDefault(m => m.Id == treatment.MedicationId);
                if (medication != null)
                    Console.WriteLine($"      Ліки: {medication.Name}, Дозування: {treatment.Dosage}");
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
        for (var i = 0; i < appointments.Count; i++)
        {
            var appointment = appointments[i];
            Console.WriteLine(
                $"{i + 1}. Дата: {appointment.DateTime}, Лікар: {_doctorService.GetDoctorById(appointment.DoctorId).FullName}, Симптоми: {appointment.Symptoms}, Статус: {appointment.AppointmentStatus}");
        }

        Console.WriteLine("Натисніть будь-яку клавішу для повернення до медичної карти.");
        Console.ReadKey();
    }

    private void DisplayMedications(List<DiseaseRecord> diseaseRecords)
    {
        if (diseaseRecords.Count == 0)
        {
            Console.WriteLine("Немає призначених ліків.");
            return;
        }

        var medications = _jsonDataService.LoadMedications();
        var medicationDTOs = new List<MedicationDTO>();
        foreach (var diseaseRecord in diseaseRecords)
        foreach (var treatment in diseaseRecord.Treatments)
        {
            var medication = medications.FirstOrDefault(m => m.Id == treatment.MedicationId);
            if (medication != null)
                medicationDTOs.Add(new MedicationDTO
                {
                    Name = medication.Name,
                    Description = medication.Description,
                    Dosage = treatment.Dosage,
                    TreatmentStatus = treatment.TreatmentStatus
                });
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
            Console.WriteLine("3. Закрити хворобу");
            Console.WriteLine("4. Назад");
            var choice = Console.ReadLine();

            var patient = _patientService.LoadPatients().FirstOrDefault(p => p.Id == appointment.PatientId);
            if (patient == null)
            {
                Console.WriteLine("Пацієнта не знайдено.");
                return;
            }

            switch (choice)
            {
                case "1":
                    DisplayMedicalRecord(patient);
                    break;
                case "2":
                    ConductExamination(doctor, appointment);
                    return;
                case "3":
                    CloseDisease(doctor, appointment);
                    return;
                case "4":
                    EditDiseaseRecord(patient);
                    return;
                case "5":
                    return;
                default:
                    Console.WriteLine("Неправильний вибір. Спробуйте ще раз.");
                    break;
            }
        }
    }

    private void CloseDisease(Doctor doctor, Appointment appointment)
    {
        var patient = _patientService.LoadPatients().FirstOrDefault(p => p.Id == appointment.PatientId);
        if (patient == null)
        {
            Console.WriteLine("Пацієнта не знайдено.");
            return;
        }

        var medicalRecord = _medicalRecordService.GetMedicalRecord(patient.Id);
        var ongoingDiseases = medicalRecord.DiseasesRecords.Where(d => d.Status == DiseaseStatus.Sick).ToList();

        if (!ongoingDiseases.Any())
        {
            Console.WriteLine("Немає активних хвороб для закриття.");
            return;
        }

        Console.WriteLine("Активні хвороби:");
        for (var i = 0; i < ongoingDiseases.Count; i++)
        {
            var disease = ongoingDiseases[i];
            Console.WriteLine($"{i + 1}. {disease.DiseaseName} (Діагноз встановлений: {disease.DateDiagnosed})");
        }

        Console.Write("Оберіть хворобу для закриття (введіть номер): ");
        if (int.TryParse(Console.ReadLine(), out var diseaseIndex) && diseaseIndex > 0 &&
            diseaseIndex <= ongoingDiseases.Count)
        {
            var disease = ongoingDiseases[diseaseIndex - 1];
            Console.WriteLine("Введіть дату одужання (dd.MM.yyyy):");
            var recoveryDateInput = Console.ReadLine();
            if (!DateTime.TryParseExact(recoveryDateInput, "dd.MM.yyyy", null, DateTimeStyles.None,
                    out var recoveryDate))
            {
                Console.WriteLine("Неправильний формат дати.");
                return;
            }

            disease.Status = DiseaseStatus.Recovered;
            disease.DateRecovered = recoveryDate;

            foreach (var treatment in disease.Treatments)
            {
                treatment.TreatmentStatus = TreatmentStatus.NotTaking;
            }

            _medicalRecordService.SaveMedicalRecord(medicalRecord);
            _appointmentService.CompleteAppointment(appointment.Id); // Завершення зустрічі
            Console.WriteLine("Хворобу успішно закрито.");
        }
        else
        {
            Console.WriteLine("Неправильний вибір.");
        }
    }

    private void ConductExamination(Doctor doctor, Appointment appointment)
    {
        Console.WriteLine("Симптоми вказані при зверненні:" + appointment.Symptoms);
        Console.WriteLine("Рекомендації по огляду: ...");
        Thread.Sleep(1500);
        Console.WriteLine("1.-------");
        Thread.Sleep(1000);
        Console.WriteLine("2.-------");
        Thread.Sleep(1000);
        Console.WriteLine("3.-------");
        Thread.Sleep(1000);

        Console.WriteLine(
            "Введіть усі симптоми пацієнта:"); //Лікар вводить симптоми і скарги пацієнта, які виявив під час огляду
        var symptoms = Console.ReadLine();

        Console.WriteLine("Введіть дату початку хвороби (dd.MM.yyyy):");
        var startDateInput = Console.ReadLine();
        if (!DateTime.TryParseExact(startDateInput, "dd.MM.yyyy", null, DateTimeStyles.None, out var startDate))
        {
            Console.WriteLine("Неправильний формат дати.");
            return;
        }

        var diseases = _jsonDataService.LoadDiseases();
        var medications = _jsonDataService.LoadMedications();
        var possibleDiseases = diseases
            .Where(d => d.Symptoms.Any(s => symptoms.Contains(s, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        if (possibleDiseases.Count != 0)
        {
            Console.WriteLine("Ось можливий діагноз:");
            foreach (var possibleDisease in possibleDiseases) Console.WriteLine($"- {possibleDisease.Name}");
        }
        else
        {
            Console.WriteLine("Жодної хвороби не знайдено за вказаними симптомами.");
        }

        Console.Write(
            "Введіть назву хвороби: "); //Лікар враховуючи або не враховуючи запропонований діагноз вводить хворобу яку приєднає до запису в картці
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

        if (recommendedMedications.Count != 0)
            foreach (var medication in recommendedMedications)
                Console.WriteLine(
                    $"- {medication.Name}: {medication.Description} (Рекомендоване дозування: {medication.RecommendedDosage})");
        else
            Console.WriteLine("Немає рекомендованих ліків на основі вказаних симптомів.");

        while (true)
        {
            Console.Write("Додати лікування? (так/ні): ");
            var addTreatment = Console.ReadLine();
            if (addTreatment?.ToLower() != "так" && addTreatment?.ToLower() != "1") break;

            Console.Write("Введіть назву ліків: ");
            var medicationName = Console.ReadLine();
            var medication =
                medications.FirstOrDefault(m => m.Name.Equals(medicationName, StringComparison.OrdinalIgnoreCase));

            if (medication == null)
            {
                Console.WriteLine("Ліки не знайдено.");
                continue;
            }

            Console.Write("Введіть дозування або натисніть 1 для рекомендованого дозування: ");
            var dosage = Console.ReadLine();
            if (dosage == "1") dosage = medication.RecommendedDosage;
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

        if (appointments.Count == 0)
        {
            Console.WriteLine("У вас немає запланованих зустрічей.");
            return;
        }

        Console.WriteLine("Ваші заплановані зустрічі:");
        for (var i = 0; i < appointments.Count; i++)
        {
            var patient = _patientService.LoadPatients().FirstOrDefault(p => p.Id == appointments[i].PatientId);
            Console.WriteLine(
                $"{i + 1}. Пацієнт: {patient?.FullName ?? "Невідомий"}, Дата та час: {appointments[i].DateTime}");
        }

        Console.Write(
            "Оберіть запис який хочете прийняти (введіть номер) або натисніть будь-яку іншу клавішу для виходу: ");
        if (int.TryParse(Console.ReadLine(), out var appointmentIndex) && appointmentIndex > 0 &&
            appointmentIndex <= appointments.Count)
        {
            var appointment = appointments[appointmentIndex - 1];
            AcceptPatient(doctor, appointment);
        }
    }

    private void CreateAppointment(Patient patient)
    {
        Console.Write("Введіть ваші симптоми (через кому):  ");
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
            availableTimes.Remove(appointment.DateTime.ToString("HH:mm"));
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

        var symptomWords = symptoms.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
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

    private void ScheduleFollowUpAppointment(Patient patient)
    {
        var ongoingDiseases = _medicalRecordService.GetMedicalRecord(patient.Id)
            .DiseasesRecords.Where(d => d.Status == DiseaseStatus.Sick).ToList();

        if (!ongoingDiseases.Any())
        {
            Console.WriteLine("У вас немає активних хвороб для огляду.");
            return;
        }

        Console.WriteLine("Ваші активні хвороби:");
        for (var i = 0; i < ongoingDiseases.Count; i++)
        {
            var disease = ongoingDiseases[i];
            Console.WriteLine($"{i + 1}. {disease.DiseaseName} (Діагноз встановлений: {disease.DateDiagnosed})");
        }

        Console.Write("Оберіть хворобу для огляду (введіть номер): ");
        if (int.TryParse(Console.ReadLine(), out var diseaseIndex) && diseaseIndex > 0 &&
            diseaseIndex <= ongoingDiseases.Count)
        {
            var disease = ongoingDiseases[diseaseIndex - 1];
            Console.WriteLine("Запис на огляд для закриття хвороби.");
            CreateAppointment(patient, disease);
        }
        else
        {
            Console.WriteLine("Неправильний вибір.");
        }
    }

    private void CreateAppointment(Patient patient, DiseaseRecord diseaseRecord)
    {
        Console.Write("Введіть ваші симптоми (через кому): ");
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

        _appointmentService.ScheduleAppointment(chosenDoctor.Id, patient.Id, chosenDateTime, symptoms,
            diseaseRecord.DiseaseId);
        Console.WriteLine("Запис успішно створений.");
    }

    private void EditDiseaseRecord(Patient patient)
    {
        var medicalRecord = _medicalRecordService.GetMedicalRecord(patient.Id);
        var diseaseRecord = medicalRecord.DiseasesRecords.FirstOrDefault(d => d.Status == DiseaseStatus.Sick);

        if (diseaseRecord == null)
        {
            Console.WriteLine("Немає активних хвороб для редагування.");
            return;
        }

        Console.WriteLine("Редагування запису про хворобу");
        Console.Write("Додайте симптоми (через кому): ");
        var newSymptoms = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newSymptoms)) diseaseRecord.Symptoms += ", " + newSymptoms;

        var medications = _jsonDataService.LoadMedications();
        while (true)
        {
            Console.Write("Додати лікування? (так/ні): ");
            var addTreatment = Console.ReadLine();
            if (addTreatment?.ToLower() != "так" && addTreatment?.ToLower() != "1") break;

            Console.Write("Введіть назву ліків: ");
            var medicationName = Console.ReadLine();
            var medication =
                medications.FirstOrDefault(m => m.Name.Equals(medicationName, StringComparison.OrdinalIgnoreCase));

            if (medication == null)
            {
                Console.WriteLine("Ліки не знайдено.");
                continue;
            }

            Console.Write("Введіть дозування або натисніть 1 для рекомендованого дозування: ");
            var dosage = Console.ReadLine();
            if (dosage == "1") dosage = medication.RecommendedDosage;
            diseaseRecord.Treatments.Add(new Treatment
            {
                MedicationId = medication.Id,
                Dosage = dosage,
                TreatmentStatus = TreatmentStatus.Taking
            });
        }

        _medicalRecordService.SaveMedicalRecord(medicalRecord);
        Console.WriteLine("Запис про хворобу успішно оновлено.");
    }
}