namespace PieperEngine.Entities.Registries
{
    public class Registry<T> : IRegistry where T : Entity
    {
        private readonly Dictionary<string, Func<string, T>> _entries = [];
        private readonly List<Func<string, T>> _id = [];
        private readonly Dictionary<Func<string, T>, int> _reverseId = [];

        private bool _frozen = false;

        public void Register(string key, Func<string, T> value)
        {
            if (_frozen)
            {
                throw new InvalidOperationException("Registry is frozen!");
            }

            if (_entries.ContainsKey(key))
            {
                throw new ArgumentException($"Duplicate key: '{key}'");
            }

            _entries[key] = value;
            _id.Add(value);
            _reverseId[value] = _id.Count - 1;

            RegistryManager.RegisterEntityKey(key, this);
        }

        public void Freeze() => _frozen = true;

        public Entity Create(string key) => _entries[key](key);
    }
}
