using OpenTK.Mathematics;

using PieperEngine.Entities;
using PieperEngine.Entities.Registries;
using PieperEngine.Physics.Collisions;
using PieperEngine.Rendering.Renderables;
using PieperEngine.Time;
using System.Text.Json;

namespace PieperEngine.Scenes
{
    public static class SceneManager
    {
        private static readonly string _scenesPath = Path.Combine(Environment.CurrentDirectory, @"Assets\Scenes");

        private static Scene CurrentScene = null!;

        public static void RunScene(string sceneName)
        {
            TimeSystem.SetScale(1f);
            RenderManager.Reset();
            CollisionSystem.Reset();

            CreateScene(sceneName);
            StartEntities();
        }

        public static void UpdateScene()
        {
            CurrentScene.NewEntitiesToScene();
            foreach (var entity in CurrentScene.Entities)
            {
                entity.Update();
            }
        }

        public static void CreateScene(string sceneName)
        {
            string path = Path.Combine(_scenesPath, $"{sceneName}.json");
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"File at the given path could not be found: {path}");
            }

            string json = File.ReadAllText(path);
            EntityDTO[]? sceneEntities = JsonSerializer.Deserialize<EntityDTO[]>(json)
                ?? throw new NullReferenceException($"No entities found from file at: {path}");

            CurrentScene = new(sceneName);
            foreach (var dto in sceneEntities)
            {
                Entity entity = RegistryManager.CreateEntity(dto.ID);
                entity.Transform.Position = new Vector3(dto.PositionX, dto.PositionY, dto.PositionZ);
                entity.Transform.Scale = new Vector2(dto.ScaleX, dto.ScaleY);
                entity.Transform.Rotation = MathHelper.DegreesToRadians(dto.Rotation);
            }

            CurrentScene.NewEntitiesToScene();
        }

        private static void StartEntities()
        {
            foreach (var entity in CurrentScene.Entities)
            {
                entity.Start();
            }
        }

        public static Scene GetCurrentScene() => CurrentScene;
        public static string GetCurrentSceneName() => CurrentScene.Name;
        public static void InstantiateEntity(Entity entity) => CurrentScene.AddEntity(entity);
        public static T? FindEntityInCurrentScene<T>() where T : Entity
        {
            var entitiesOfType = CurrentScene.Entities.OfType<T>();
            return entitiesOfType?.FirstOrDefault();
        }
    }
}
