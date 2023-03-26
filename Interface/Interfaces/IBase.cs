namespace Interface.Interfaces;

public interface IBase<Entity> where Entity : struct
{
    public bool Add(Entity entity);
    public Entity GetById(long id);
    public Entity Update(Entity entity);
    public bool Delete(long id);
    public IEnumerable<Entity> GetAll();
}