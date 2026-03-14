using OpenTK.Graphics.OpenGL4;

using PieperEngine.Buffers.Geometry;
using PieperEngine.Buffers.Geometry.Utilities;

namespace PieperEngine.Buffers
{
    public class VertexBufferObject : IBuffer
    {
        public int ID { get; set; }
        public BufferUsageHint Usage { get; set; }

        public int BufferSize { get; private set; }

        public VertexBufferObject(float[] vertices, BufferUsageHint usage)
        {
            ID = GL.GenBuffer();
            Usage = usage;

            BufferData(vertices);
        }

        public void LoadAttributeDataToBuffer(VertexAttribute attribute, int vertexOffset, params float[] values)
        {
            int offset = (VertexLayout.AttributeOffsets[(int)attribute] + vertexOffset) * sizeof(float);
            int size = VertexLayout.AttributeSizes[(int)attribute] * sizeof(float);

            Bind();

            GL.BufferSubData(BufferTarget.ArrayBuffer, offset, size, values);

            Unbind();
        }

        public void Bind() => GL.BindBuffer(BufferTarget.ArrayBuffer, ID);
        public void Unbind() => GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        public void Delete() => GL.DeleteBuffer(ID);

        public void BufferData(float[] vertices)
        {
            BufferSize = vertices.Length;

            Bind();
            GL.BufferData(BufferTarget.ArrayBuffer, BufferSize * sizeof(float), vertices, Usage);
            Unbind();
        }

        public void BufferSubData(nint offset, float[] vertices)
        {
            Bind();
            GL.BufferSubData(BufferTarget.ArrayBuffer, offset, vertices.Length * sizeof(float), vertices);
            Unbind();
        }

        public void ReadAll()
        {
            Bind();

            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out int byteSize);
            float[] data = new float[byteSize / sizeof(float)];
            GL.GetBufferSubData(BufferTarget.ArrayBuffer, 0, byteSize, data);

            for (int i = 0; i < 4; i++)
            {
                int vertexPosition = i * VertexLayout.VertexStride;
                Console.WriteLine($"\n" +
                $"Vertex number {i}\n" +
                $"Position: ({data[0 + vertexPosition]}, {data[1 + vertexPosition]}, {data[2 + vertexPosition]})\n" +
                $"Color: ({data[3 + vertexPosition]}, {data[4 + vertexPosition]}, {data[5 + vertexPosition]}, {data[6 + vertexPosition]})\n" +
                $"UV0: ({data[7 + vertexPosition]}, {data[8 + vertexPosition]})\n" +
                $"UV1, ({data[9 + vertexPosition]}, {data[10 + vertexPosition]})\n");
            }

            Unbind();
        }
    }
}
