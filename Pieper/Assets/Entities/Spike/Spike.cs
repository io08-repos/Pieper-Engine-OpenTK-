using Pieper.Assets.Scripts;

using PieperEngine.Buffers.Geometry;
using PieperEngine.Physics;
using PieperEngine.Physics.Collisions;
using PieperEngine.Rendering.Renderables;

namespace Pieper.Assets.Entities
{
    public class Spike : WorldEntity
    {
        public Spike(string key) : base(key)
        {
            Name = GetType().Name;
            Bitmask = new LayerMask("Hazard");

            renderer = new SpriteRenderer(SpriteAtlas.GetTexture("spr_spike"));
            RectangleSource rectSource = new()
            {
                X = 0,
                Y = 0,
                Width = 12.8f,
                Height = 25.6f,
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
