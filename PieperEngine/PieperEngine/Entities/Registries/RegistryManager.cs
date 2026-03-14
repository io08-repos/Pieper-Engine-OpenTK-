namespace PieperEngine.Entities.Registries
{
    public static class RegistryManager
    {
        private static readonly Dictionary<string, IRegistry> _allEntities = [];

        public static void RegisterEntityKey(string key, IRegistry registry)
            => _allEntities.Add(key, registry);

        public static IRegistry? GetRegistry(string key)
        {
            if (_allEntities.TryGetValue(key, out var registry))
            {
                return registry;
            }
            
            return null;
        }

        public static Entity CreateEntity(string key)
        {
            if (GetRegistry(key) is not IRegistry reg)
            {
                throw new KeyNotFoundException($"Unknown entity key: {key}");
            }

            return reg.Create(key);
        }
    }
}
