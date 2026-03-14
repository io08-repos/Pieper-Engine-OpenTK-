using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

using PieperEngine.Components;
using PieperEngine.Entities;
using PieperEngine.Rendering.Camera;
using PieperEngine.Rendering.Meshes;
using PieperEngine.Rendering.Shaders;
using PieperEngine.Rendering.Textures;

namespace PieperEngine.Rendering.Renderables
{
    public class SpriteRenderer : Renderer, IComponent
    {
        public Texture2D Texture { get; private set; }

        public SpriteRenderer(Texture2D texture, string vertexShaderFile = "spriterenderer_vertex.glsl", string fragmentShaderFile = "spriterenderer_fragment.glsl")
            : base(vertexShaderFile, fragmentShaderFile)
        {
            Texture = texture;

            CreateRenderable();
        }

        protected override void CreateRenderable()
        {
            ShaderProgram shader = new(VertexShaderFile, FragmentShaderFile);
            Mesh mesh = MeshFactory.CreateRectangle(0f, 0f, Texture.Image.Width, Texture.Image.Height);
            Material material = new(shader);

            Renderable = new Renderable(mesh, material);
            Renderable.Mesh.SetColorAttribute(Color, Renderable.VBO);
            Renderable.Mesh.SetUVAttribute(Renderable.VBO);
        }

        public void SetTexture(Texture2D value)
        {
            Texture = value;
            CreateRenderable();
        }

        public override void Draw()
        {
            Texture.Use(TextureUnit.Texture0);
            Renderable.Material.SetUniform("projection", Camera2D.Main.GetProjectionMatrix());
            Renderable.Material.SetUniform("model", Entity.Transform.GetTransformMatrix());
            Renderable.Draw(PrimitiveType.Triangles);
        }

        public Vector2 GetSpriteWorldSize()
        {
            Vector2 size = new (Texture.Image.Width, Texture.Image.Height);
            size *= Entity.Transform.Scale;

            return size;
        }
    }
}
