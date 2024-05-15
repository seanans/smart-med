namespace SmartMed.Models;

public class Disease
{
    private int _id;
    private string _name;
    private string _description;
    private DateTime _diagnosisDate;

    public int Id
    {
        get => _id;
        set => _id = value;
    }

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

    public DateTime DiagnosisDate
    {
        get => _diagnosisDate;
        set => _diagnosisDate = value;
    }
}