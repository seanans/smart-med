namespace SmartMed.Models;

public class MedicationDTO
{
    private string _description;
    private string _dosage;
    private string _name;

    public string Dosage
    {
        get => _dosage;
        set => _dosage = value ?? throw new ArgumentNullException(nameof(value));
    }

    public TreatmentStatus TreatmentStatus { get; set; }

    public string Name
    {
        get => _name;
        set => _name = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Description
    {
        get => _description;
        set => _description = value ?? throw new ArgumentNullException(nameof(value));
    }
}