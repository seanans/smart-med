using Newtonsoft.Json;
using SmartMed.Models;

namespace SmartMed.Services;

public class JsonDataService
{
        private string usersFilePath;
        private string appointmentsFilePath;
        private string medicationsFilePath;

        public JsonDataService(string usersFilePath, string appointmentsFilePath, string medicationsFilePath)
        {
            this.usersFilePath = usersFilePath;
            this.appointmentsFilePath = appointmentsFilePath;
            this.medicationsFilePath = medicationsFilePath;
        }

        public List<User> LoadUsers()
        {
            if (!File.Exists(usersFilePath))
            {
                return new List<User>();
            }

            string jsonData = File.ReadAllText(usersFilePath);
            return JsonConvert.DeserializeObject<List<User>>(jsonData) ?? new List<User>();
        }

        public void SaveUsers(List<User> users)
        {
            string jsonData = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText(usersFilePath, jsonData);
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