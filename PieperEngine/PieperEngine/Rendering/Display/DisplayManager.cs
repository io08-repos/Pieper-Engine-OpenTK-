using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace PieperEngine.Rendering.Display
{
    public static class DisplayManager
    {
        public static GameWindow GameWindow { get; set; } = null!;
        public static Vector2i NativeSize { get; private set; }

        public static void Initialize(GameWindow window)
        {
            GameWindow = window;
            NativeSize = window.Size;
        }
    }
}
