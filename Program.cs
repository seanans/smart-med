﻿using System.Text;
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
        const string symptomsProfilesFilePath = $"{basePath}/symptoms_profiles.json";

        var jsonDataService =
            new JsonDataService(patientsFilePath, doctorsFilePath, appointmentsFilePath, medicationsFilePath,
                symptomsProfilesFilePath);
        IUserService userService = new UserService(jsonDataService);
        IPatientService patientService = new PatientService(jsonDataService);
        IDoctorService doctorService = new DoctorService(jsonDataService);
        IMedicalRecordService medicalRecordService = new MedicalRecordService(jsonDataService);

        var menu = new Menu.Menu(userService, patientService, doctorService, jsonDataService, medicalRecordService);
        menu.DisplayMainMenu();
    }
}