using OpenTK.Mathematics;

using PieperEngine.Time;

namespace PieperEngine.Physics
{
    public class Velocity(Rigidbody2D rb2d, float x = 0f, float y = 0f)
    {
        private readonly Rigidbody2D _rb2d = rb2d;

        public float X = x;
        public float Y = y;

        public Vector3 Value => new (X, Y, 0);

        public void UpdateX()
        {
            _rb2d.Entity.Transform.Position += X * TimeSystem.DeltaTime * Vector3.UnitX;
        }
        public void UpdateY()
        {
            _rb2d.Entity.Transform.Position += Y * TimeSystem.DeltaTime * Vector3.UnitY;
        }

        public void Set(float x, float y)
        {
            X = x;
            Y = y;
        }

        public void SetX(float x) => X = x;
        public void SetY(float y) => Y = y;

        public void AddX(float x) => X += x;
        public void AddY(float y) => Y += y;
    }
}
