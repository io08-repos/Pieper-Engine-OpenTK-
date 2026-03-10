namespace Pieper.Assets.Entities
{
    public class WorldEntities
    {
        public static void Initialize()
        {
            EntityManager.EntityRegistry.Register("pieper:player", key => new Player(key));
            EntityManager.EntityRegistry.Register("pieper:block", key => new Block(key));
            EntityManager.EntityRegistry.Register("pieper:grid", key => new Grid(key));
            EntityManager.EntityRegistry.Register("pieper:ground", key => new Ground(key));
            EntityManager.EntityRegistry.Register("pieper:spike", key => new Spike(key));
            EntityManager.EntityRegistry.Register("pieper:half_spike", key => new HalfSpike(key));
            EntityManager.EntityRegistry.Register("pieper:game_manager", key => new GameManager(key));
        }
    }
}
