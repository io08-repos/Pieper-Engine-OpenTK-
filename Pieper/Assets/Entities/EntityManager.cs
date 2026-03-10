using PieperEngine.Entities.Registries;

namespace Pieper.Assets.Entities
{
    public class EntityManager
    {
        public static readonly Registry<WorldEntity> EntityRegistry = new ();
    }
}
