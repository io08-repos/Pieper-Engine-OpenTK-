using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace PieperEngine.Input
{
    public class InputSystem
    {
        private static readonly HashSet<Keys> _pressedKeys = [];
        private static readonly HashSet<MouseButton> _pressedMouseButtons = [];
        
        private static HashSet<Keys> _pressedKeysLast = [];
        private static HashSet<MouseButton> _pressedMouseButtonsLast = [];

        public static void Initialize(GameWindow window)
        {
            window.KeyDown += OnKeyDown;
            window.KeyUp += OnKeyUp;
            window.MouseDown += OnMouseDown;
            window.MouseUp += OnMouseUp;
        }

        public static void Update()
        {
            _pressedKeysLast = [.. _pressedKeys];
            _pressedMouseButtonsLast = [.. _pressedMouseButtons];
        }

        public static bool GetKey(Keys key) => _pressedKeys.Contains(key);
        public static bool GetKeyDown(Keys key) => _pressedKeys.Contains(key) && !_pressedKeysLast.Contains(key);
        public static bool GetKeyUp(Keys key) => !_pressedKeys.Contains(key) && _pressedKeysLast.Contains(key);
        public static bool GetMouse(MouseButton mouseButton) => _pressedMouseButtons.Contains(mouseButton);
        public static bool GetMouseDown(MouseButton mouseButton) => _pressedMouseButtons.Contains(mouseButton) && !_pressedMouseButtonsLast.Contains(mouseButton);
        public static bool GetMouseUp(MouseButton mouseButton) => !_pressedMouseButtons.Contains(mouseButton) && _pressedMouseButtonsLast.Contains(mouseButton);


        public static void OnKeyDown(KeyboardKeyEventArgs e) => _pressedKeys.Add(e.Key);
        public static void OnKeyUp(KeyboardKeyEventArgs e) => _pressedKeys.Remove(e.Key);
        public static void OnMouseDown(MouseButtonEventArgs e) => _pressedMouseButtons.Add(e.Button);
        public static void OnMouseUp(MouseButtonEventArgs e) => _pressedMouseButtons.Remove(e.Button);
    }
}
