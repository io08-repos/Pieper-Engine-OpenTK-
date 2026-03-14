namespace PieperEngine.Rendering.Shaders
{
    public class Material(ShaderProgram shader)
    {
        public ShaderProgram Shader { get; } = shader;

        private readonly Dictionary<string, object> _uniforms = [];

        public void SetUniform(string name, object value)
            => _uniforms[name] = value;

        public void Use()
        {
            Shader.Use();

            foreach (var uniform in _uniforms)
            {
                Shader.SetUniform(uniform.Key, uniform.Value);
            }
        }

        public void Delete() => Shader.Delete();
    }
}
