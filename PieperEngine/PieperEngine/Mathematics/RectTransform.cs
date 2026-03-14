using OpenTK.Mathematics;

namespace PieperEngine.Mathematics
{
    public class RectTransform : Transform2D
    {
        public Anchor Anchor { get; set; }
        public Vector2 Pivot { get; private set; }

        public void SetPivot(Vector2 Pivot)
        {

        }

        public override Matrix4 GetTransformMatrix()
        {
            Vector3 vTranslation = new Vector3(Anchor.Point) + Position;

            Matrix4 translation = Matrix4.CreateTranslation(vTranslation);
            Matrix4 scale = Matrix4.CreateScale(Scale.X, Scale.Y, 1f);
            Matrix4 rotation = Matrix4.CreateRotationZ(Rotation);

            return scale * rotation * translation;
        }
    }
}
