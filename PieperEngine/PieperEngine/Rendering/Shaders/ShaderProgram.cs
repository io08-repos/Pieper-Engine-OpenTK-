using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace PieperEngine.Rendering.Shaders
{
    public class ShaderProgram
    {
        public readonly int ID;
        private readonly Dictionary<string, int> _uniformLocations = [];

        private static readonly string _shadersPath = Path.Combine(Environment.CurrentDirectory, @"Assets\Shaders");

        private readonly string _vertexShaderPath;
        private readonly string _fragmentShaderPath;

        private readonly string _vertexShaderSource;
        private readonly string _fragmentShaderSource;

        public ShaderProgram(string vertexShaderName, string fragmentShaderName)
        {
            _vertexShaderPath = Path.Combine(_shadersPath, vertexShaderName);
            _fragmentShaderPath = Path.Combine(_shadersPath, fragmentShaderName);

            _vertexShaderSource = File.ReadAllText(_vertexShaderPath);
            _fragmentShaderSource = File.ReadAllText(_fragmentShaderPath);

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, _vertexShaderSource);
            GL.CompileShader(vertexShader);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, _fragmentShaderSource);
            GL.CompileShader(fragmentShader);

            ID = GL.CreateProgram();
            GL.AttachShader(ID, vertexShader);
            GL.AttachShader(ID, fragmentShader);
            GL.LinkProgram(ID);

            GL.DetachShader(ID, vertexShader);
            GL.DetachShader(ID, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        public void Use() => GL.UseProgram(ID);
        public void Delete() => GL.DeleteProgram(ID);

        private int GetUniformLocation(string name)
        {
            if (_uniformLocations.TryGetValue(name, out int location))
                return location;

            location = GL.GetUniformLocation(ID, name);
            if (location == -1)
            {
                throw new Exception($"Uniform '{name}' not found.");
            }

            _uniformLocations[name] = location;
            return location;
        }

        public void SetUniform(string name, object value)
        {
            switch (value)
            {
                case int i:
                    SetUniform(name, i);
                    break;
                case float f:
                    SetUniform(name, f);
                    break;
                case Vector2 v2:
                    SetUniform(name, v2);
                    break;
                case Vector3 v3:
                    SetUniform(name, v3);
                    break;
                case Vector4 v4:
                    SetUniform(name, v4);
                    break;
                case Matrix4 matrix:
                    SetUniform(name, matrix);
                    break;
                default:
                    throw new Exception($"Unsupported uniform type: {value.GetType()}");
            }
        }
        public void SetUniform(string name, int value)
            => GL.Uniform1(GetUniformLocation(name), value);
        public void SetUniform(string name, float value)
            => GL.Uniform1(GetUniformLocation(name), value);
        public void SetUniform(string name, Vector2 value)
            => GL.Uniform2(GetUniformLocation(name), value);
        public void SetUniform(string name, Vector3 value)
            => GL.Uniform3(GetUniformLocation(name), value);
        public void SetUniform(string name, Vector4 value)
            => GL.Uniform4(GetUniformLocation(name), value);
        public void SetUniform(string name, Matrix4 value)
            => GL.UniformMatrix4(GetUniformLocation(name), false, ref value);
    }
}
