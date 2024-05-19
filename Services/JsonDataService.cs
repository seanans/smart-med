using System.Text;
using Newtonsoft.Json;
using SmartMed.Models;

namespace SmartMed.Services;

public class JsonDataService(
    string patientsFilePath,
    string doctorsFilePath,
    string appointmentsFilePath,
    string medicationsFilePath,
    string symptomsProfilesFilePath)
{
    public List<Patient> LoadPatients() => LoadData<List<Patient>>(patientsFilePath) ?? new List<Patient>();

    public void SavePatients(List<Patient> patients) => SaveData(patientsFilePath, patients);

    public List<Doctor> LoadDoctors() => LoadData<List<Doctor>>(doctorsFilePath) ?? new List<Doctor>();

    public void SaveDoctors(List<Doctor> doctors) => SaveData(doctorsFilePath, doctors);

    public List<Appointment> LoadAppointments() => LoadData<List<Appointment>>(appointmentsFilePath) ?? new List<Appointment>();
    public void SaveAppointments(List<Appointment> appointments) => SaveData(appointmentsFilePath, appointments);

    public List<Medication> LoadMedications() => LoadData<List<Medication>>(medicationsFilePath) ?? new List<Medication>();

    public void SaveMedications(List<Medication> medications) => SaveData(medicationsFilePath, medications);

    public Dictionary<string, List<string>> LoadSymptomProfiles() => LoadData<Dictionary<string, List<string>>>(symptomsProfilesFilePath) ?? new Dictionary<string, List<string>>();

    private T? LoadData<T>(string filePath)
    {
        try
        {
            if (!File.Exists(filePath)) return default(T);

            var jsonData = File.ReadAllText(filePath, Encoding.UTF8);
            return JsonConvert.DeserializeObject<T>(jsonData);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Помилка при завантаженні даних з файлу {filePath}: {e.Message}");
            return default(T);
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