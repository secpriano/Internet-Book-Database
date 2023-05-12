namespace IBDbWebApplication.Models.Entity;

public class SettingModel
{
    public long? Id { get; set; }
    public string Description { get; set; }

    public SettingModel(long? id, string description) => 
        (Id, Description) = (id, description);
}