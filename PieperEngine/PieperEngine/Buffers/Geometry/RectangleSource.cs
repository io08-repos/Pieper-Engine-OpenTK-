using OpenTK.Mathematics;

namespace PieperEngine.Buffers.Geometry
{
    /// <summary>
    /// Vertex source for rectangle shapes.
    /// </summary>
    public struct RectangleSource(float x, float y, float width, float height) : IVertexSource
    {
        public float X = x;
        public float Y = y;
        public float Width = width;
        public float Height = height;

        //public readonly float Left => X;
        //public readonly float Right => X + Width;
        //public readonly float Top => Y;
        //public readonly float Bottom => Y - Height;
        public readonly Vector2 HalfExtents => new (Width / 2f, Height / 2f);

        public readonly Vertex[] GenerateVertices()
        {
            Vector2 halfExtents = HalfExtents;

            Vertex[] vertices = new Vertex[4];
            for (int i = 0; i < vertices.Length; i++) vertices[i] = new Vertex();

            vertices[0].SetAttributeValues(VertexAttribute.Position, X - halfExtents.X, Y + halfExtents.Y, 0f);
            vertices[1].SetAttributeValues(VertexAttribute.Position, X + halfExtents.X, Y + halfExtents.Y, 0f);
            vertices[2].SetAttributeValues(VertexAttribute.Position, X + halfExtents.X, Y - halfExtents.Y, 0f);
            vertices[3].SetAttributeValues(VertexAttribute.Position, X - halfExtents.X, Y - halfExtents.Y, 0f);

            return vertices;
        }

        public readonly uint[] GetIndices() => [
            0, 1, 2,
            0, 2, 3
        ];
    }
}
