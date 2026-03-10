using OpenTK.Mathematics;

using PieperEngine.Rendering.Camera;
using PieperEngine.Rendering.Renderables;

namespace Pieper.Assets.Entities
{
    public class Grid : WorldEntity
    {
        private readonly float _cellSize = 64f;
        private static readonly LineRenderer _originLine = new ();

        public Grid(string key) : base(key)
        {
            Name = GetType().Name;

            renderer = new LineRenderer();
            renderer.ChangeColor(Color4.Black);
            AddComponent(renderer);

            _originLine.ChangeColor(Color4.Aqua);
            AddComponent(_originLine);
        }

        public override void Update()
        {
            Camera2D.GetWorldBounds(Camera2D.Main, out float left, out float right, out float top, out float bottom);

            left = (float)Math.Floor(left / _cellSize) * _cellSize;
            right = (float)Math.Ceiling(right / _cellSize) * _cellSize;
            bottom = (float)Math.Floor(bottom / _cellSize) * _cellSize;
            top = (float)Math.Ceiling(top / _cellSize) * _cellSize;

            List<Vector2> points = [];

            for (float x = left; x <= right; x += _cellSize)
            {
                if (x == 0f)
                {
                    _originLine.Points = [ new Vector2(x, top), new Vector2(x, bottom) ];
                    continue;
                }

                points.Add(new Vector2(x, top));
                points.Add(new Vector2(x, bottom));
            }

            for (float y = bottom; y <= top; y += _cellSize)
            {
                points.Add(new Vector2(left, y));
                points.Add(new Vector2(right, y));
            }

            var lineRenderer = (LineRenderer) renderer;
            lineRenderer.Points = [.. points];
        }
    }
}
