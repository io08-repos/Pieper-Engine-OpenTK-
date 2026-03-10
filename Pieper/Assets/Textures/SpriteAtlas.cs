using PieperEngine.Rendering.Textures;

namespace Pieper.Assets.Scripts
{
    public static class SpriteAtlas
    {
        private static readonly string _texturePath = Path.Combine(Environment.CurrentDirectory, "Assets", "Textures");
        private static readonly Dictionary<string, Texture2D> _textures = [];

        public static Texture2D Empty => GetTexture("spr_square");

        static SpriteAtlas()
        {
            foreach (var textureFile in Directory.GetFiles(_texturePath))
            {
                if (Path.GetExtension(textureFile) != ".png")
                {
                    Console.WriteLine($"Couldn't load file '{Path.GetFileName(textureFile)}' to the sprite atlas.");
                    continue;
                }

                string textureName = Path.GetFileNameWithoutExtension(textureFile);
                Texture2D texture = new (textureFile);

                _textures.Add(textureName, texture);
            }
        }

        public static Texture2D GetTexture(string textureName)
            => _textures[textureName];
    }
}
