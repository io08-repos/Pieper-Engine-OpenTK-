using Pieper.Assets.Scripts;

using PieperEngine.Physics;
using PieperEngine.Physics.Collisions;
using PieperEngine.Rendering.Renderables;

namespace Pieper.Assets.Entities
{
    public class Block : WorldEntity
    {
        public Block(string key) : base(key)
        {
            Name = GetType().Name;
            Bitmask = new LayerMask("Ground");

            renderer = new SpriteRenderer(SpriteAtlas.GetTexture("spr_block"));
            Collider collider = new (entity: this, renderer.Renderable.Mesh, ColliderState.Static, ColliderType.Solid);

            AddComponent(renderer);
            AddComponent(collider);
        }

        public override void Update()
        {
            
        }
    }
}
