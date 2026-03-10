using PieperEngine.Entities;
using PieperEngine.Rendering.Renderables;
using PieperEngine.Physics;
using PieperEngine.Physics.Collisions;

namespace Pieper.Assets.Entities
{
    public abstract class WorldEntity(string key) : Entity(key)
    {
        protected Renderer renderer = null!;
        protected Rigidbody2D _rb2d = null!;

        public override void OnCollisionEnter(CollisionEventArgs args)
        {
            if (args.Axis == ResolutionAxis.X) _rb2d.Velocity.SetX(0f);
            else if (args.Axis == ResolutionAxis.Y) _rb2d.Velocity.SetY(0f);
        }
    }
}
