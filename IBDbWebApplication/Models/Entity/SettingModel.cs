namespace IBDbWebApplication.Models.Entity;

public class SettingModel
{
    public byte? Id { get; set; }
    public string Description { get; set; }

    public SettingModel(byte? id, string description) => 
        (Id, Description) = (id, description);
}