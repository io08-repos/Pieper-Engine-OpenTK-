using OpenTK.Mathematics;

namespace PieperEngine.Buffers.Geometry
{
    public struct LineSource : IVertexSource
    {
        public Vector2[] Points;

        public LineSource(Vector2 start, Vector2 end)
        {
            Points = [start, end];
        }

        public LineSource(params Vector2[] points)
        {
            Points = points;
        }

        public readonly Vertex[] GenerateVertices()
        {
            Vertex[] vertices = new Vertex[ Points.Length ];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vertex();
                vertices[i].SetAttributeValues(VertexAttribute.Position, Points[i].X, Points[i].Y, 0);
            }

            return vertices;
        }

        public readonly uint[] GetIndices() => throw new InvalidOperationException("LineSource doesn't generate indices.");
    }
}
