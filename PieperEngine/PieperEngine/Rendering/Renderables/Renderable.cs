using OpenTK.Graphics.OpenGL4;

using PieperEngine.Buffers;
using PieperEngine.Rendering.Meshes;
using PieperEngine.Rendering.Shaders;

namespace PieperEngine.Rendering.Renderables
{
    public class Renderable
    {
        public Mesh Mesh;
        public Material Material;

        public VertexBufferObject VBO = null!;
        public VertexArrayObject VAO = null!;
        public ElementBufferObject? EBO;

        public Renderable(Mesh mesh, Material material)
        {
            Mesh = mesh;
            Material = material;

            Initialize();
        }

        public void Initialize()
        {
            VBO = new VertexBufferObject(Mesh.GetVertexData(), BufferUsageHint.DynamicDraw);
            VBO.Bind();

            VAO = new();
            VAO.Bind();

            uint[]? indices = Mesh.GetIndices();
            if (indices != null) EBO = new ElementBufferObject(indices, BufferUsageHint.StaticDraw);

            VBO.Unbind();
            VAO.Unbind();
        }

        public void Draw(PrimitiveType primitiveType)
        {
            Material.Use();
            VAO.Bind();

            if (EBO == null)
            {
                GL.DrawArrays(primitiveType, 0, Mesh.Count);
            }
            else
            {
                EBO.Bind();
                GL.DrawElements(primitiveType, Mesh.GetIndices().Length, DrawElementsType.UnsignedInt, 0);
                EBO.Unbind();
            }

            VAO.Unbind();
        }

        public void UpdateVertexBufferObject()
        {
            float[] data = Mesh.GetVertexData();

            if (data.Length != VBO.BufferSize) VBO.BufferData(data);
            else VBO.BufferSubData(IntPtr.Zero, data);
        }

        public void Delete()
        {
            Material.Delete();
            VBO.Delete();
            VAO.Delete();
            EBO?.Delete();
        }
    }
}
