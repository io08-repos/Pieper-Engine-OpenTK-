using OpenTK.Mathematics;

using PieperEngine.Buffers.Geometry;
using PieperEngine.Components;
using PieperEngine.Entities;
using PieperEngine.Rendering.Meshes;
using PieperEngine.Rendering.Renderables;

namespace PieperEngine.Physics.Collisions
{
    public enum ColliderState
    {
        Active,
        Static
    }

    public enum ColliderType
    {
        Solid,
        Trigger
    }

    public class Collider : IComponent
    {
        public bool Enabled { get; set; } = true;
        public Entity Entity { get; set; } = null!;

        //private LineRenderer _outline = null!;
        private readonly Mesh _mesh;
        
        public Vector2[] Points;
        public Vector2i Cell { get; set; }
        public Vector2 Offset { get; set; } = Vector2.Zero;

        private ColliderType _type;
        public ColliderType Type
        {
            get => _type;
            set
            {
                if (_state != ColliderState.Active)
                {
                    _type = value;
                }
            }
        }

        private ColliderState _state;
        public ColliderState State
        {
            get => _state;
            set
            {
                if (value == ColliderState.Active)
                {
                    _type = ColliderType.Solid;
                }

                _state = value;
            }
        }
        public bool Active => State == ColliderState.Active;
        public bool FreezeRotation { get; set; } = true;

        public Collider(Entity entity, Mesh mesh, ColliderState state, ColliderType type)
        {
            Entity = entity;
            State = state;
            Type = (State == ColliderState.Static)
                ? type : ColliderType.Solid;

            _mesh = mesh;
            Points = new Vector2[mesh.Count];

            //CreateDebugOutline(entity);

            ExtractTransformedMeshPoints();
            CollisionSystem.Register(this);
        }

        public Collider(Entity entity, IVertexSource source, ColliderState state, ColliderType type)
        {
            Entity = entity;
            State = state;
            Type = type;

            _mesh = new Mesh(source.GenerateVertices());
            Points = new Vector2[_mesh.Count];

            //CreateDebugOutline(entity);

            ExtractTransformedMeshPoints();
            CollisionSystem.Register(this);
        }

        public void Update()
        {
            ExtractTransformedMeshPoints();

            //_outline.Enabled = CollisionSystem.ShowHitboxes;
            //if (_outline.Enabled)
            //{
            //    ConvertPointsToLines();
            //}
        }

        //public void CreateDebugOutline(Entity entity)
        //{
        //    _outline = new LineRenderer();
        //    _outline.ChangeColor(Color4.Blue);
        //    _outline.Entity = entity;
        //}

        private void ExtractTransformedMeshPoints()
        {
            for (int i = 0; i < _mesh.Count; i++)
            {
                Vertex v = _mesh.GetVertex(i);
                float[] position = v.GetAttributeValues(VertexAttribute.Position);
                Vector2 point = new(position[0], position[1]);

                Points[i] = TransformedPoint(point);
            }
        }

        //private void ConvertPointsToLines()
        //{
        //    Vector2[] linePoints = new Vector2[Points.Length * 2];
        //    for (int i = 0; i < Points.Length; i++)
        //    {
        //        Vector2 start = Points[i];
        //        Vector2 end = Points[(i + 1) % Points.Length];
        //        (linePoints[i], linePoints[i + 1]) = (start, end);
        //    }

        //    _outline.Points = linePoints;
        //}

        private Vector2 TransformedPoint(Vector2 point)
        {
            Vector4 vPoint = new(point.X + Offset.X, point.Y + Offset.Y, 0, 1);
            Matrix4 transform = !FreezeRotation
                ? Entity.Transform.GetTransformMatrix()
                : Entity.Transform.GetUnrotatedTransformMatrix();
            Vector4 nextPoint = Vector4.TransformRow(vPoint, transform);

            return new Vector2(nextPoint.X, nextPoint.Y);
        }



        public Vector2[] GetPoints() => Points;
    }
}
