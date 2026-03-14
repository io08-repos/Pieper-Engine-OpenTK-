using OpenTK.Mathematics;

namespace PieperEngine.Mathematics
{
    public struct Anchor(Vector2 value)
    {
        public Vector2 Point = value;

        public static readonly Vector2 TopLeft = new(-1f, 1f);
        public static readonly Vector2 TopCenter = new(0f, 1f);
        public static readonly Vector2 TopRight = new(1f, 1f);
        public static readonly Vector2 MiddleLeft = new(-1f, 0f);
        public static readonly Vector2 MiddleCenter = new(0f, 0f);
        public static readonly Vector2 MiddleRight = new(1f, 0f);
        public static readonly Vector2 BottomLeft = new(-1f, -1f);
        public static readonly Vector2 BottomCenter = new(0f, -1f);
        public static readonly Vector2 BottomRight = new(1f, -1f);
    }
}
