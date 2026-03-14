using OpenTK.Mathematics;

using PieperEngine.Buffers.Geometry;
using PieperEngine.Mathematics;
using PieperEngine.Rendering.Display;

namespace PieperEngine.Rendering.Camera
{
    public class Camera2D
    {
        public Transform2D Transform { get; set; }
        public float Zoom { get; set; }

        public Vector3 NextPosition { get; set; }

        public static readonly Camera2D Main = new();

        public float HalfWidth => DisplayManager.GameWindow.Size.X * 0.5f / Zoom;
        public float HalfHeight => DisplayManager.GameWindow.Size.Y * 0.5f / Zoom;

        public Camera2D(float x = 0f, float y = 0f, float zoom = 1.25f)
        {
            Transform = new Transform2D(x, y);
            Zoom = zoom;
        }

        public Camera2D(Vector2 position, float zoom = 1.25f)
        {
            Transform = new Transform2D(position.X, position.Y);
            Zoom = zoom;
        }

        public void Update()
        {
            Transform.Position = NextPosition;
        }

        public void SetPosition(Vector3 position) => NextPosition = position;

        public Matrix4 GetProjectionMatrix()
        {
            float halfWidth = HalfWidth;
            float halfHeight = HalfHeight;
            Matrix4 projectionMatrix = Matrix4.CreateOrthographicOffCenter(-halfWidth, halfWidth, -halfHeight, halfHeight, -1f, 1f);

            Matrix4 translation = Matrix4.CreateTranslation(-Transform.Position.X, -Transform.Position.Y, 0);
            Matrix4 rotation = Matrix4.CreateRotationZ(-Transform.Rotation);
            Matrix4 viewMatrix = translation * rotation;

            return viewMatrix * projectionMatrix;
        }

        public RectangleSource GetWorldBounds() => new()
        {
            X = Transform.Position.X,
            Y = Transform.Position.Y,
            Width = 2 * HalfWidth,
            Height = 2 * HalfHeight
        };

        public static void GetWorldBounds(Camera2D camera, out float left, out float right, out float top, out float bottom)
        {
            RectangleSource bounds = camera.GetWorldBounds();
            Vector2 halfExtents = bounds.HalfExtents;

            left = bounds.X - halfExtents.X;
            right = bounds.X + halfExtents.X;
            bottom = bounds.Y - halfExtents.Y;
            top = bounds.Y + halfExtents.Y;

            if (left > right)
            {
                (right, left) = (left, right);
            }

            if (bottom > top)
            {
                (top, bottom) = (bottom, top);
            }
        }
    }
}
