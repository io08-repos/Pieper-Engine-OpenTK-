using PieperEngine.Entities;

namespace PieperEngine.Components
{
    public interface IComponent
    {
        public bool Enabled { get; set; } 
        public Entity Entity { get; set; }

        public virtual void Update() { }
    }
}
