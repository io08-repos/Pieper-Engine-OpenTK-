using Pieper.Assets.Scripts;

using PieperEngine.Physics;
using PieperEngine.Physics.Collisions;
using PieperEngine.Rendering.Renderables;

namespace Pieper.Assets.Entities
{
    public class GroundBlock : WorldEntity
    {
        public GroundBlock(WorldEntity parent) : base(UnregisteredID)
        {
            Name = GetType().Name;
            Bitmask = new LayerMask("Ground");
            SetParent(parent);

            renderer = new SpriteRenderer(SpriteAtlas.GetTexture("spr_ground"));
            Collider collider = new (entity: this, renderer.Renderable.Mesh, ColliderState.Static, ColliderType.Solid);

            AddComponent(renderer);
            AddComponent(collider);
        }

        public override void Update()
        {
            
        }
    }
}
