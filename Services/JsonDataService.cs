using System.Text;
using Newtonsoft.Json;
using SmartMed.Models;

namespace SmartMed.Services;

public class JsonDataService(
    string patientsFilePath,
    string doctorsFilePath,
    string appointmentsFilePath,
    string medicationsFilePath,
    string symptomsProfilesFilePath,
    string diseasesFilePath,
    string medicalRecordsFilePath)
{
    public List<Patient> LoadPatients()
    {
        return LoadData<List<Patient>>(patientsFilePath) ?? new List<Patient>();
    }

    public void SavePatients(List<Patient> patients)
    {
        SaveData(patientsFilePath, patients);
    }

    public List<Doctor> LoadDoctors()
    {
        return LoadData<List<Doctor>>(doctorsFilePath) ?? new List<Doctor>();
    }

    public void SaveDoctors(List<Doctor> doctors)
    {
        SaveData(doctorsFilePath, doctors);
    }

    public List<Appointment> LoadAppointments()
    {
        return LoadData<List<Appointment>>(appointmentsFilePath) ?? new List<Appointment>();
    }

    public void SaveAppointments(List<Appointment> appointments)
    {
        SaveData(appointmentsFilePath, appointments);
    }

    public List<Medication> LoadMedications()
    {
        return LoadData<List<Medication>>(medicationsFilePath) ?? new List<Medication>();
    }

    public void SaveMedications(List<Medication> medications)
    {
        SaveData(medicationsFilePath, medications);
    }

    public Dictionary<string, List<string>> LoadSymptomProfiles()
    {
        return LoadData<Dictionary<string, List<string>>>(symptomsProfilesFilePath) ??
               new Dictionary<string, List<string>>();
    }

    public List<Disease> LoadDiseases()
    {
        return LoadData<List<Disease>>(diseasesFilePath) ?? new List<Disease>();
    }

    public void SaveDiseases(List<Disease> diseases)
    {
        SaveData(diseasesFilePath, diseases);
    }

    public List<MedicalRecord> LoadMedicalRecords()
    {
        return LoadData<List<MedicalRecord>>(medicalRecordsFilePath) ?? new List<MedicalRecord>();
    }

    public void SaveMedicalRecords(List<MedicalRecord> medicalRecords)
    {
        SaveData(medicalRecordsFilePath, medicalRecords);
    }

    private T? LoadData<T>(string filePath)
    {
        try
        {
            if (!File.Exists(filePath)) return default;

            var jsonData = File.ReadAllText(filePath, Encoding.UTF8);
            return JsonConvert.DeserializeObject<T>(jsonData);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Помилка при завантаженні даних з файлу {filePath}: {e.Message}");
            return default;
        }
    }

    private void SaveData<T>(string filePath, T data)
    {
        try
        {
            var jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filePath, jsonData, Encoding.UTF8);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка при збереженні даних у файл {filePath}: {ex.Message}");
        }
    }
}