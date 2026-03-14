using OpenTK.Graphics.OpenGL4;

using PieperEngine.Buffers.Geometry;
using PieperEngine.Buffers.Geometry.Utilities;

namespace PieperEngine.Buffers
{
    public class VertexArrayObject : IBuffer
    {
        public int ID { get; set; }

        public VertexArrayObject()
        {
            ID = GL.GenVertexArray();
            Bind();

            for (int i = 0; i < (int)VertexAttribute.MaxAttributes; i++)
            {
                GL.VertexAttribPointer(
                    index: i,
                    size: VertexLayout.AttributeSizes[i],
                    type: VertexAttribPointerType.Float,
                    normalized: false,
                    stride: VertexLayout.VertexStride * sizeof(float),
                    offset: VertexLayout.AttributeOffsets[i] * sizeof(float)
                );

                GL.EnableVertexAttribArray(i);
            }

            Unbind();
        }

        public void Bind() => GL.BindVertexArray(ID);
        public void Unbind() => GL.BindVertexArray(0);
        public void Delete() => GL.DeleteVertexArray(ID);
    }
}
