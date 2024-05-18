using System.Text;
using Newtonsoft.Json;
using SmartMed.Models;

namespace SmartMed.Services;

public class JsonDataService
{
    private readonly string _appointmentsFilePath;
    private readonly string _doctorsFilePath;
    private readonly string _medicationsFilePath;
    private readonly string _patientsFilePath;
    private readonly string _symptomsProfilesFilePath;

    public JsonDataService(string patientsFilePath, string doctorsFilePath, string appointmentsFilePath,
        string medicationsFilePath, string symptomsProfilesFilePath)
    {
        _patientsFilePath = patientsFilePath;
        _doctorsFilePath = doctorsFilePath;
        _appointmentsFilePath = appointmentsFilePath;
        _medicationsFilePath = medicationsFilePath;
        _symptomsProfilesFilePath = symptomsProfilesFilePath;
    }

    public List<Patient> LoadPatients()
    {
        if (!File.Exists(_patientsFilePath)) return new List<Patient>();

        var jsonData = File.ReadAllText(_patientsFilePath, Encoding.UTF8);
        return JsonConvert.DeserializeObject<List<Patient>>(jsonData) ?? new List<Patient>();
    }

    public void SavePatients(List<Patient> patients)
    {
        var jsonData = JsonConvert.SerializeObject(patients, Formatting.Indented);
        File.WriteAllText(_patientsFilePath, jsonData, Encoding.UTF8);
    }

    public List<Doctor> LoadDoctors()
    {
        if (!File.Exists(_doctorsFilePath)) return new List<Doctor>();

        var jsonData = File.ReadAllText(_doctorsFilePath, Encoding.UTF8);
        return JsonConvert.DeserializeObject<List<Doctor>>(jsonData) ?? new List<Doctor>();
    }

    public void SaveDoctors(List<Doctor> doctors)
    {
        var jsonData = JsonConvert.SerializeObject(doctors, Formatting.Indented);
        File.WriteAllText(_doctorsFilePath, jsonData, Encoding.UTF8);
    }

    public List<Appointment> LoadAppointments()
    {
        if (!File.Exists(_appointmentsFilePath)) return new List<Appointment>();

        var jsonData = File.ReadAllText(_appointmentsFilePath, Encoding.UTF8);
        return JsonConvert.DeserializeObject<List<Appointment>>(jsonData) ?? new List<Appointment>();
    }

    public void SaveAppointments(List<Appointment> appointments)
    {
        var jsonData = JsonConvert.SerializeObject(appointments, Formatting.Indented);
        File.WriteAllText(_appointmentsFilePath, jsonData, Encoding.UTF8);
    }

    public List<Medication> LoadMedications()
    {
        if (!File.Exists(_medicationsFilePath)) return new List<Medication>();

        var jsonData = File.ReadAllText(_medicationsFilePath, Encoding.UTF8);
        return JsonConvert.DeserializeObject<List<Medication>>(jsonData) ?? new List<Medication>();
    }

    public void SaveMedications(List<Medication> medications)
    {
        var jsonData = JsonConvert.SerializeObject(medications, Formatting.Indented);
        File.WriteAllText(_medicationsFilePath, jsonData, Encoding.UTF8);
    }

    public Dictionary<string, List<string>> LoadSymptomProfiles()
    {
        if (!File.Exists(_symptomsProfilesFilePath)) return new Dictionary<string, List<string>>();
        var jsonData = File.ReadAllText(_symptomsProfilesFilePath, Encoding.UTF8);
        return JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(jsonData) ??
               new Dictionary<string, List<string>>();
    }
}