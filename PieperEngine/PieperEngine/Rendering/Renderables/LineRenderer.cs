using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

using PieperEngine.Components;
using PieperEngine.Rendering.Camera;
using PieperEngine.Rendering.Meshes;
using PieperEngine.Rendering.Shaders;

namespace PieperEngine.Rendering.Renderables
{
    public class LineRenderer : Renderer, IComponent
    {
        public Vector2[] Points { get; set; }

        public LineRenderer(string vertexShaderFile = "linerenderer_vertex.glsl", string fragmentShaderFile = "linerenderer_fragment.glsl", params Vector2[] points)
            : base(vertexShaderFile, fragmentShaderFile)
        {
            Points = points;

            CreateRenderable();
        }

        public override void Draw()
        {
            RecreateMesh();

            Renderable.Material.SetUniform("projection", Camera2D.Main.GetProjectionMatrix());
            Renderable.Material.SetUniform("model", Entity.Transform.GetTransformMatrix());
            Renderable.Draw(PrimitiveType.Lines);
        }

        protected override void CreateRenderable()
        {
            ShaderProgram shader = new(VertexShaderFile, FragmentShaderFile);
            Mesh mesh = MeshFactory.CreateLines(Points);
            Material material = new(shader);

            Renderable = new Renderable(mesh, material);
            Renderable.Mesh.SetColorAttribute(Color, Renderable.VBO);
        }

        public void RecreateMesh()
        {
            Renderable.Mesh = MeshFactory.CreateLines(Points);
            Renderable.UpdateVertexBufferObject();

            Renderable.Mesh.SetColorAttribute(Color, Renderable.VBO);
        }

        public void ShiftPoints(int shift)
        {
            Vector2[] points = new Vector2[Points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                int shiftIndex = (i + shift) % points.Length;
                points[shiftIndex] = Points[i];
            }

            Points = points;
        }
    }
}
