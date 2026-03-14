using PieperEngine.Components;
using PieperEngine.Mathematics;
using PieperEngine.Physics;
using PieperEngine.Physics.Collisions;
using PieperEngine.Scenes;

namespace PieperEngine.Entities
{
    public enum EntityType
    {
        World,
        UI
    }

    public abstract class Entity
    {
        private bool _enabled = true;
        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (value != _enabled)
                {
                    _enabled = value;
                    foreach (var componentList in Components.Values)
                    {
                        for (int i = 0; i < componentList.Count; i++)
                        {
                            componentList[i].Enabled = _enabled;
                        }
                    }
                }
            }
        }
        public string Name { get; set; } = null!;
        public string ID { get; private set; } = null!;
        public EntityType Type { get; private set; } = EntityType.World;

        public Entity? Parent { get; private set; }
        public Entity[] Children = [];

        public Transform2D Transform { get; private set; }
        protected Dictionary<Type, List<IComponent>> Components { get; private set; } = [];
        public LayerMask Bitmask { get; set; }

        public static readonly string UnregisteredID = "Unregistered";

        public Entity(string key)
        {
            ID = key;

            Transform = Transform2D.Identity;
            if (key != UnregisteredID || Type == EntityType.World)
            {
                SceneManager.InstantiateEntity(this);
            }
        }

        public virtual void Start() { }
        public virtual void Update() { }

        public void AddComponent(IComponent component)
        {
            component.Entity = this;
            var type = component.GetType();

            if (!Components.ContainsKey(type))
            {
                Components[type] = [];
            }

            Components[component.GetType()].Add(component);
        }

        public T? GetComponent<T>() where T : IComponent
            => Components.TryGetValue(typeof(T), out var components) ? (T) components[0] : default;

        public T? GetComponentByIndex<T>(int index) where T : IComponent
            => Components.TryGetValue(typeof(T), out var components) ? (T) components[index] : default;

        public void SetTransform(Transform2D transform) => Transform = transform;
        private void InsertChild(Entity child)
        {
            Array.Resize(ref Children, Children.Length + 1);
            Children[^1] = child;
        }
        public void SetParent(Entity parent)
        {
            Parent = parent;
            parent.InsertChild(this);
        }

        public void AddChild(Entity child) => child.SetParent(this);

        public virtual void OnCollisionEnter(CollisionEventArgs args) { }
        public virtual void OnTriggerEnter(Collider other) { }
    }
}
