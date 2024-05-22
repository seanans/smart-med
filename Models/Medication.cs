namespace SmartMed.Models;

public class Medication
{
    private string _contraindications;
    private string _description;
    private string _name;
    private string _recommendedDosage;

    public int Id { get; set; }

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

    public string Contraindications
    {
        get => _contraindications;
        set => _contraindications = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string RecommendedDosage
    {
        get => _recommendedDosage;
        set => _recommendedDosage = value ?? throw new ArgumentNullException(nameof(value));
    }

    public List<string> EffectiveSymptoms { get; set; } = new();
}