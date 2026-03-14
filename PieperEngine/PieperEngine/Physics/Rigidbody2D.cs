using PieperEngine.Components;
using PieperEngine.Entities;
using PieperEngine.Time;

namespace PieperEngine.Physics
{
    public class Rigidbody2D : IComponent
    {
        public bool Enabled { get; set; } = true;
        public Entity Entity { get; set; } = null!;

        public float Mass { get; set; }
        public Velocity Velocity { get; set; }

        private float TerminalVelocity => (Mass * PhysicsSystem.Gravity) / 25f;

        public Rigidbody2D(float mass = 1f)
        {
            Mass = mass;
            Velocity = new Velocity(this);

            PhysicsSystem.Register(this);
        }

        public void UpdateX() => Velocity.UpdateX();

        public void UpdateY()
        {
            float velY = Velocity.Y;
            velY += PhysicsSystem.Gravity * TimeSystem.DeltaTime;

            float terminalVelY = TerminalVelocity;
            if (terminalVelY < 0)
            {
                velY = Math.Max(velY, terminalVelY);
            }
            else
            {
                velY = Math.Min(velY, terminalVelY);
            }

            Velocity.SetY(velY);
            Velocity.UpdateY();
        }

        public void ApplyForceX(float x) => Velocity.AddX(x);
        public void ApplyForceY(float y) => Velocity.AddY(y);
    }
}
