using PieperEngine.Buffers.Geometry.Utilities;

namespace PieperEngine.Buffers.Geometry
{
    public enum VertexAttribute
    {
        Position, //vec3
        Color,    //vec4
        UV0,      //vec2
        UV1,      //vec2

        MaxAttributes
    }

    public struct Vertex
    {
        public float[] Data { get; private set; }

        public Vertex()
        {
            Data = new float[VertexLayout.VertexStride];
        }

        public readonly void SetAttributeValues(VertexAttribute attribute, params float[] values)
        {
            int offset = VertexLayout.AttributeOffsets[(int)attribute];
            int size = VertexLayout.AttributeSizes[(int)attribute];

            if (values.Length != size)
            {
                throw new ArgumentException($"Attribute '{attribute}' expects {size} values.");
            }

            Array.Copy(values, 0, Data, offset, size);
        }

        public readonly float[] GetAttributeValues(VertexAttribute attribute)
        {
            int offset = VertexLayout.AttributeOffsets[(int)attribute];
            int size = VertexLayout.AttributeSizes[(int)attribute];

            float[] result = new float[size];
            Array.Copy(Data, offset, result, 0, size);
            return result;
        }
    }
}
