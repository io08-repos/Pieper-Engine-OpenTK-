using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

using PieperEngine.Components;
using PieperEngine.Entities;

namespace PieperEngine.Rendering.Renderables
{
    public abstract class Renderer : IComponent
    {
        public bool Enabled { get; set; } = true;
        public Entity Entity { get; set; } = null!;

        public Renderable Renderable = null!;
        protected Color4 Color = Color4.White;

        protected readonly string VertexShaderFile;
        protected readonly string FragmentShaderFile;

        public Renderer(string vertexShaderFile, string fragmentShaderFile)
        {
            VertexShaderFile = vertexShaderFile;
            FragmentShaderFile = fragmentShaderFile;

            RenderManager.Register(this);
        }

        public virtual void Update() => Draw();
        public virtual void ChangeColor(Color4 color)
        {
            Color = color;
            Renderable.Mesh.SetColorAttribute(Color, Renderable.VBO);
        }

        public abstract void Draw();
        protected abstract void CreateRenderable();

        public static void EnableColorBlending()
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        }

        public void Delete() => Renderable.Delete();
    }
}
