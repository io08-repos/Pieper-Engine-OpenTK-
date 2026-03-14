using OpenTK.Mathematics;

using PieperEngine.Buffers.Geometry;
using PieperEngine.Buffers.Geometry.Utilities;

namespace PieperEngine.Rendering.Meshes
{
    public static class MeshFactory
    {
        public static Mesh CreateRectangle(float x, float y, float width, float height)
        {
            RectangleSource rectangleSource = new()
            {
                X = x,
                Y = y,
                Width = width,
                Height = height
            };

            Vertex[] vertices = rectangleSource.GenerateVertices();
            uint[] indices = rectangleSource.GetIndices();
            return new Mesh(vertices, indices);
        }

        public static Mesh CreateLines(Vector2[] points)
        {
            LineSource lineSource = new(points);
            Vertex[] vertices = lineSource.GenerateVertices();
            return new Mesh(vertices);
        }
    }
}
