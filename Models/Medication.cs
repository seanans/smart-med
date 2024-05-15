namespace SmartMed.Models;

public class Medication
{
    private int id;
    private string name;
    private string description;
    private string dosage;

    public int Id
    {
        get => id;
        set => id = value;
    }

    public string Name
    {
        get => name;
        set => name = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Description
    {
        get => description;
        set => description = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Dosage
    {
        get => dosage;
        set => dosage = value ?? throw new ArgumentNullException(nameof(value));
    }
}