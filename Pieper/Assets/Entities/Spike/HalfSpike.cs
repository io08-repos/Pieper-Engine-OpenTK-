using Pieper.Assets.Scripts;

using PieperEngine.Buffers.Geometry;
using PieperEngine.Physics;
using PieperEngine.Physics.Collisions;
using PieperEngine.Rendering.Renderables;

namespace Pieper.Assets.Entities
{
    public class HalfSpike : WorldEntity
    {
        public HalfSpike(string key) : base(key)
        {
            Name = GetType().Name;
            Bitmask = new LayerMask("Hazard");

            renderer = new SpriteRenderer(SpriteAtlas.GetTexture("spr_half_spike"));
            RectangleSource rectSource = new()
            {
                X = 0,
                Y = -18f,
                Width = 12.8f,
                Height = 12.16f,
            };
            Collider collider = new (entity: this, rectSource, ColliderState.Static, ColliderType.Trigger);

            AddComponent(renderer);
            AddComponent(collider);
        }

        public override void Update()
        {
            
        }
    }
}
