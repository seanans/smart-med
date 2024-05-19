namespace SmartMed.Models;

public class Doctor : User
{ 
    public List<string> Profiles { get; set; } = new();
    public Dictionary<string, List<string>> Schedule { get; set; } = new();
}