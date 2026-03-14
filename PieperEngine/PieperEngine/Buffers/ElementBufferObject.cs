using OpenTK.Graphics.OpenGL4;

namespace PieperEngine.Buffers
{
    public class ElementBufferObject : IBuffer
    {
        public int ID { get; set; }

        public ElementBufferObject(uint[] indices, BufferUsageHint usage)
        {
            ID = GL.GenBuffer();
            Bind();

            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, usage);

            Unbind();
        }

        public void Bind() => GL.BindBuffer(BufferTarget.ElementArrayBuffer, ID);
        public void Unbind() => GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        public void Delete() => GL.DeleteBuffer(ID);
    }
}
