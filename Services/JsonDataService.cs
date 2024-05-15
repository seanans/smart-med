using Newtonsoft.Json;
using SmartMed.Models;

namespace SmartMed.Services;

public class JsonDataService
{
    private string patientsFilePath;
        private string doctorsFilePath;
        private string appointmentsFilePath;
        private string medicationsFilePath;

        public JsonDataService(string patientsFilePath, string doctorsFilePath, string appointmentsFilePath, string medicationsFilePath)
        {
            this.patientsFilePath = patientsFilePath;
            this.doctorsFilePath = doctorsFilePath;
            this.appointmentsFilePath = appointmentsFilePath;
            this.medicationsFilePath = medicationsFilePath;
        }

        public List<Patient> LoadPatients()
        {
            if (!File.Exists(patientsFilePath))
            {
                return new List<Patient>();
            }

            string jsonData = File.ReadAllText(patientsFilePath);
            return JsonConvert.DeserializeObject<List<Patient>>(jsonData) ?? new List<Patient>();
        }

        public void SavePatients(List<Patient> patients)
        {
            string jsonData = JsonConvert.SerializeObject(patients, Formatting.Indented);
            File.WriteAllText(patientsFilePath, jsonData);
        }

        public List<Doctor> LoadDoctors()
        {
            if (!File.Exists(doctorsFilePath))
            {
                return new List<Doctor>();
            }

            string jsonData = File.ReadAllText(doctorsFilePath);
            return JsonConvert.DeserializeObject<List<Doctor>>(jsonData) ?? new List<Doctor>();
        }

        public void SaveDoctors(List<Doctor> doctors)
        {
            string jsonData = JsonConvert.SerializeObject(doctors, Formatting.Indented);
            File.WriteAllText(doctorsFilePath, jsonData);
        }

        public List<Appointment> LoadAppointments()
        {
            if (!File.Exists(appointmentsFilePath))
            {
                return new List<Appointment>();
            }

            string jsonData = File.ReadAllText(appointmentsFilePath);
            return JsonConvert.DeserializeObject<List<Appointment>>(jsonData) ?? new List<Appointment>();
        }

        public void SaveAppointments(List<Appointment> appointments)
        {
            string jsonData = JsonConvert.SerializeObject(appointments, Formatting.Indented);
            File.WriteAllText(appointmentsFilePath, jsonData);
        }

        public List<Medication> LoadMedications()
        {
            if (!File.Exists(medicationsFilePath))
            {
                return new List<Medication>();
            }

            string jsonData = File.ReadAllText(medicationsFilePath);
            return JsonConvert.DeserializeObject<List<Medication>>(jsonData) ?? new List<Medication>();
        }

        public void SaveMedications(List<Medication> medications)
        {
            string jsonData = JsonConvert.SerializeObject(medications, Formatting.Indented);
            File.WriteAllText(medicationsFilePath, jsonData);
        }    
    
}