using PieperEngine.Entities;

namespace PieperEngine.Scenes
{
    public class Scene(string name)
    {
        public string Name { get; private set; } = name;

        public List<Entity> Entities = [];
        public List<Entity> NewEntities = [];

        public void AddEntity(Entity entity) => NewEntities.Add(entity);
        public void NewEntitiesToScene()
        {
            Entities.AddRange(NewEntities);
            NewEntities.Clear();
        }
    }
}
