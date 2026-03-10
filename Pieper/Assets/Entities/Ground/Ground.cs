using OpenTK.Mathematics;

using PieperEngine.Rendering.Renderables;
using PieperEngine.Rendering.Camera;
using PieperEngine.Physics;

namespace Pieper.Assets.Entities
{
    public class Ground : WorldEntity
    {
        private GroundBlock[] _groundBlocks = [];
        private readonly float _groundSize;
        private const int MAX_VISIBLE_BLOCKS = 32;

        public Ground(string key) : base(key)
        {
            Name = GetType().Name;
            Bitmask = new LayerMask("Ground");

            var prefab = new GroundBlock(this) { Enabled = false };
            var groundRenderer = prefab.GetComponent<SpriteRenderer>()!;
            _groundSize = groundRenderer.GetSpriteWorldSize().X;
        }

        public override void Update()
        {
            Camera2D.GetWorldBounds(Camera2D.Main, out float left, out float right, out _, out _);

            left = (float)Math.Floor(left / _groundSize) * _groundSize;
            right = (float)Math.Ceiling(right / _groundSize) * _groundSize;
            
            int needed = Math.Min(
                (int)Math.Ceiling((right - left) / _groundSize) + 1,
                MAX_VISIBLE_BLOCKS);

            if (needed > _groundBlocks.Length)
            {
                int oldLength = _groundBlocks.Length;
                Array.Resize(ref _groundBlocks, needed);
                for (int i = oldLength; i < needed; i++)
                {
                    _groundBlocks[i] = new GroundBlock(this) { Enabled = false };
                }
            }

            for (int i = 0; i < needed; i++)
            {
                _groundBlocks[i].Enabled = true;
                _groundBlocks[i].Transform.Position = new Vector3(
                    left + i * _groundSize,
                    _groundBlocks[i].Transform.Position.Y,
                    _groundBlocks[i].Transform.Position.Z
                );
            }
        }
    }
}
