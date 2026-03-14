namespace PieperEngine.Rendering.Renderables
{
    public static class RenderManager
    {
        private static readonly List<Renderer> renderers = [];

        public static void Register(Renderer renderer) => renderers.Add(renderer);
        public static void Update()
        {
            foreach (var renderer in renderers)
            {
                if (renderer.Enabled) renderer.Draw();
            }
        }

        public static void Reset()
        {
            foreach (var renderer in renderers)
            {
                renderer.Delete();
            }

            renderers.Clear();
        }
    }
}
