using OpenTK.Mathematics;

using PieperEngine.Buffers;
using PieperEngine.Buffers.Geometry;
using PieperEngine.Buffers.Geometry.Utilities;

namespace PieperEngine.Rendering.Meshes
{
    public readonly struct Mesh(Vertex[] vertices, uint[] indices = null!)
    {
        private readonly Vertex[] _vertices = vertices;
        private readonly uint[] _indices = indices;

        public int Count => _vertices.Length;

        public readonly float[] GetVertexData()
        {
            float[] data = new float[_vertices.Length * VertexLayout.VertexStride];
            int offset = 0;
            for (int i = 0; i < _vertices.Length; i++)
            {
                Array.Copy(_vertices[i].Data, 0, data, offset, VertexLayout.VertexStride);
                offset += VertexLayout.VertexStride;
            }

            return data;
        }

        public readonly Vertex GetVertex(int index) => _vertices[index];

        public readonly uint[] GetIndices()
            => _indices;

        public void SetColorAttribute(Color4 color, VertexBufferObject vbo)
        {
            for (int i = 0; i < _vertices.Length; i++)
            {
                ref var vertex = ref _vertices[i];
                vertex.SetAttributeValues(VertexAttribute.Color, color.R, color.G, color.B, color.A);
                vbo.LoadAttributeDataToBuffer(VertexAttribute.Color, i * VertexLayout.VertexStride, vertex.GetAttributeValues(VertexAttribute.Color));
            }
        }

        public void SetUVAttribute(float u, float v, float width, float height, VertexBufferObject vbo)
        {
            if (_vertices.Length != 4)
            {
                throw new ArgumentException("This mesh is not a rectangle, but your arguments imply otherwise.");
            }

            float left = u;
            float right = u + width;
            float top = v + height;
            float bottom = v;

            _vertices[0].SetAttributeValues(VertexAttribute.UV0, left, top);
            _vertices[1].SetAttributeValues(VertexAttribute.UV0, right, top);
            _vertices[2].SetAttributeValues(VertexAttribute.UV0, right, bottom);
            _vertices[3].SetAttributeValues(VertexAttribute.UV0, left, bottom);

            for (int i = 0; i < _vertices.Length; i++)
            {
                ref readonly var vertex = ref _vertices[i];
                vbo.LoadAttributeDataToBuffer(VertexAttribute.UV0, i * VertexLayout.VertexStride, vertex.GetAttributeValues(VertexAttribute.UV0));
            }
        }

        public void SetUVAttribute(VertexBufferObject vbo)
            => SetUVAttribute(u: 0f, v: 0f, width: 1f, height: 1f, vbo);
    }
}
