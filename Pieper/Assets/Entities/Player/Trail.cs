using OpenTK.Mathematics;

using PieperEngine.Rendering.Renderables;
using PieperEngine.Time;

namespace Pieper.Assets.Entities
{
    public class Trail : WorldEntity
    {
        private readonly LineRenderer _lineRenderer;
        private readonly WorldEntity _marker;

        private readonly float _trackingRate = 30f;
        private readonly float _cooldown = 2f;
        private float _waitTime = 0f;
        private int _size;

        private float Threshold => 1 / _trackingRate;

        public Trail(WorldEntity marker) : base(UnregisteredID)
        {
            Name = GetType().Name;
            _marker = marker;

            renderer = new LineRenderer();
            renderer.ChangeColor(Color4.Lime);

            _lineRenderer = (LineRenderer) renderer;
            CreatePointArray();

            AddComponent(_lineRenderer);
        }

        public override void Start()
        {
            base.Start();
            SetStartPosition();
        }

        public override void Update()
        {
            _waitTime += TimeSystem.DeltaTime;

            if (_waitTime > Threshold)
            {
                _waitTime = 0;

                Vector2 position = new(_marker.Transform.Position.X, _marker.Transform.Position.Y);
                _lineRenderer.ShiftPoints(2);
                _lineRenderer.Points[0] = position;
                _lineRenderer.Points[1] = _lineRenderer.Points[2];
            }
        }

        private void CreatePointArray()
        {
            _size = (int)Math.Ceiling(_trackingRate * _cooldown) * 2;
            _lineRenderer.Points = new Vector2[_size];
        }

        private void SetStartPosition()
        {
            var points = _lineRenderer.Points;
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = new Vector2(_marker.Transform.Position.X, _marker.Transform.Position.Y);
            }
        }
    }
}
