using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Mathematics;

namespace Pieper
{
    class Program
    {
        static Game game = null!;

        private static void Main()
        {
            GameWindowSettings gws = new()
            {
                UpdateFrequency = 0
            };

            NativeWindowSettings nws = new()
            {
                API = ContextAPI.OpenGL,
                APIVersion = new Version(4, 5),
                Flags = ContextFlags.Debug,

                ClientSize = new Vector2i(1280, 720),
                Location = new Vector2i(0, 0),
                Title = "Pieper Engine"
            };

            game = new (gws, nws);
            game.Run();
        }
    }
}