using System.Runtime.InteropServices;

using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

using PieperEngine.Rendering.Renderables;
using PieperEngine.Rendering.Display;
using PieperEngine.Rendering.Camera;
using PieperEngine.Physics.Collisions;
using PieperEngine.Physics;
using PieperEngine.Input;
using PieperEngine.Time;
using PieperEngine.Scenes;

using Pieper.Assets.Entities;

namespace Pieper
{
    public class Game(GameWindowSettings gws, NativeWindowSettings nws) : GameWindow(gws, nws)
    {
        protected override void OnLoad()
        {
            base.OnLoad();

            GL.Enable(EnableCap.DebugOutput);
            GL.Enable(EnableCap.DebugOutputSynchronous);
            GL.DebugMessageCallback(DebugCallback, IntPtr.Zero);

            GL.ClearColor(Color4.CornflowerBlue);

            DisplayManager.Initialize(this);
            InputSystem.Initialize(DisplayManager.GameWindow);
            WorldEntities.Initialize();
            SaveSystem.Initialize();

            Renderer.EnableColorBlending();

            SceneManager.RunScene("spiky_spikes");
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            TimeSystem.Update((float)args.Time);
            SceneManager.UpdateScene();

            PhysicsSystem.UpdateX();
            CollisionSystem.Update(ResolutionAxis.X);

            PhysicsSystem.UpdateY();
            CollisionSystem.Update(ResolutionAxis.Y);
            
            if (InputSystem.GetKeyDown(Keys.R))
            {
                string currentScene = SceneManager.GetCurrentSceneName();
                SceneManager.RunScene(currentScene);
            }

            InputSystem.Update();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            Camera2D.Main.Update();
            RenderManager.Update();

            SwapBuffers();
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            var currentScene = SceneManager.GetCurrentScene();
            var task = Task.Run(async () => await SaveSystem.SaveScene(currentScene));
            task.GetAwaiter().GetResult();

            RenderManager.Reset();
            CollisionSystem.Reset();
        }

        private static void DebugCallback(
            DebugSource source,
            DebugType type,
            int id,
            DebugSeverity severity,
            int length,
            IntPtr message,
            IntPtr userParam
            )
        {
            string msg = Marshal.PtrToStringAnsi(message, length);
            Console.WriteLine($"[GL DEBUG] {type} (Severity: {severity}): {msg}");
        }
    }
}
