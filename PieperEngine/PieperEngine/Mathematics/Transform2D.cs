using OpenTK.Mathematics;

namespace PieperEngine.Mathematics
{
    public class Transform2D
    {
        public Vector3 Position { get; set; }
        public Vector2 Scale { get; set; }
        public float Rotation { get; set; }

        public Transform2D(float posX = 0f, float posY = 0f, float posZ = 0f, float scaleX = 1f, float scaleY = 1f, float rotation = 0f)
        {
            Position = new Vector3(posX, posY, posZ);
            Scale = new Vector2(scaleX, scaleY);
            Rotation = rotation;
        }

        public Transform2D(Vector3 position, Vector2 scale, float rotation = 0f)
        {
            Position = position;
            Scale = scale;
            Rotation = rotation;
        }

        public virtual Matrix4 GetTransformMatrix()
        {
            Matrix4 translation = Matrix4.CreateTranslation(Position);
            Matrix4 scale = Matrix4.CreateScale(Scale.X, Scale.Y, 1f);
            Matrix4 rotation = Matrix4.CreateRotationZ(Rotation);

            return scale * rotation * translation;
        }

        public Matrix4 GetUnrotatedTransformMatrix()
        {
            Matrix4 translation = Matrix4.CreateTranslation(Position);
            Matrix4 scale = Matrix4.CreateScale(Scale.X, Scale.Y, 1f);

            return scale * translation;
        }

        public static Transform2D Identity => new ();
    }
}
