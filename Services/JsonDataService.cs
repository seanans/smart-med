using System.Text;
using Newtonsoft.Json;
using SmartMed.Models;

namespace SmartMed.Services;

/// <summary>
///     Клас, який забезпечує методи для завантаження і збереження даних у форматі JSON.
/// </summary>
public class JsonDataService(
    string patientsFilePath,
    string doctorsFilePath,
    string appointmentsFilePath,
    string medicationsFilePath,
    string symptomsProfilesFilePath,
    string diseasesFilePath,
    string medicalRecordsFilePath)
{
    /// <summary>
    ///     Завантажує дані пацієнтів із JSON файлу.
    /// </summary>
    /// <returns>Список пацієнтів.</returns>
    public List<Patient> LoadPatients()
    {
        return LoadData<List<Patient>>(patientsFilePath) ?? new List<Patient>();
    }

    /// <summary>
    ///     Зберігає дані пацієнтів у JSON файл.
    /// </summary>
    /// <param name="patients">Список пацієнтів.</param>
    public void SavePatients(List<Patient> patients)
    {
        SaveData(patientsFilePath, patients);
    }

    /// <summary>
    ///     Завантажує дані лікарів із JSON файлу.
    /// </summary>
    /// <returns>Список лікарів.</returns>
    public List<Doctor> LoadDoctors()
    {
        return LoadData<List<Doctor>>(doctorsFilePath) ?? new List<Doctor>();
    }

    /// <summary>
    ///     Зберігає дані лікарів у JSON файл.
    /// </summary>
    /// <param name="doctors">Список лікарів.</param>
    public void SaveDoctors(List<Doctor> doctors)
    {
        SaveData(doctorsFilePath, doctors);
    }

    /// <summary>
    ///     Завантажує дані записів на прийом із JSON файлу.
    /// </summary>
    /// <returns>Список записів на прийом.</returns>
    public List<Appointment> LoadAppointments()
    {
        return LoadData<List<Appointment>>(appointmentsFilePath) ?? new List<Appointment>();
    }

    /// <summary>
    ///     Зберігає дані записів на прийом у JSON файл.
    /// </summary>
    /// <param name="appointments">Список записів на прийом.</param>
    public void SaveAppointments(List<Appointment> appointments)
    {
        SaveData(appointmentsFilePath, appointments);
    }

    /// <summary>
    ///     Завантажує дані про ліки із JSON файлу.
    /// </summary>
    /// <returns>Список ліків.</returns>
    public List<Medication> LoadMedications()
    {
        return LoadData<List<Medication>>(medicationsFilePath) ?? new List<Medication>();
    }

    /// <summary>
    ///     Зберігає дані про ліки у JSON файл.
    /// </summary>
    /// <param name="medications">Список ліків.</param>
    public void SaveMedications(List<Medication> medications)
    {
        SaveData(medicationsFilePath, medications);
    }

    /// <summary>
    ///     Завантажує профілі симптомів із JSON файлу.
    /// </summary>
    /// <returns>Словник профілів симптомів.</returns>
    public Dictionary<string, List<string>> LoadSymptomProfiles()
    {
        return LoadData<Dictionary<string, List<string>>>(symptomsProfilesFilePath) ??
               new Dictionary<string, List<string>>();
    }

    /// <summary>
    ///     Завантажує дані про хвороби із JSON файлу.
    /// </summary>
    /// <returns>Список хвороб.</returns>
    public List<Disease> LoadDiseases()
    {
        return LoadData<List<Disease>>(diseasesFilePath) ?? new List<Disease>();
    }

    /// <summary>
    ///     Зберігає дані про хвороби у JSON файл.
    /// </summary>
    /// <param name="diseases">Список хвороб.</param>
    public void SaveDiseases(List<Disease> diseases)
    {
        SaveData(diseasesFilePath, diseases);
    }

    /// <summary>
    ///     Завантажує медичні записи із JSON файлу.
    /// </summary>
    /// <returns>Список медичних записів.</returns>
    public List<MedicalRecord> LoadMedicalRecords()
    {
        return LoadData<List<MedicalRecord>>(medicalRecordsFilePath) ?? new List<MedicalRecord>();
    }

    /// <summary>
    ///     Зберігає медичні записи у JSON файл.
    /// </summary>
    /// <param name="medicalRecords">Список медичних записів.</param>
    public void SaveMedicalRecords(List<MedicalRecord> medicalRecords)
    {
        SaveData(medicalRecordsFilePath, medicalRecords);
    }


    /// <summary>
    ///     Завантажує дані із JSON файлу і десеріалізує їх у вказаний тип T.
    /// </summary>
    /// <typeparam name="T">Тип даних, який необхідно десеріалізувати.</typeparam>
    /// <param name="filePath">Шлях до файлу JSON, з якого необхідно завантажити дані.</param>
    /// <returns>Десеріалізовані дані типу T, або значення за замовчуванням для типу T, якщо файл не існує або сталася помилка.</returns>
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

    /// <summary>
    ///     Зберігає дані у JSON файл.
    /// </summary>
    /// <typeparam name="T">Тип даних, який необхідно серіалізувати.</typeparam>
    /// <param name="filePath">Шлях до файлу JSON, у який необхідно зберегти дані.</param>
    /// <param name="data">Дані, які необхідно зберегти.</param>
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