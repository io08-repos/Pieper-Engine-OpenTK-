namespace PieperEngine.Physics
{
    public static class PhysicsSystem
    {
        private static readonly List<Rigidbody2D> _bodies = [];

        public const float Earth = -9.8f;

        public static readonly float Gravity = Earth * 750f;

        public static void Register(Rigidbody2D body) => _bodies.Add(body);

        public static void UpdateX()
        {
            foreach (var body in _bodies)
            {
                if (body.Enabled) body.UpdateX();
            }
        }

        public static void UpdateY()
        {
            foreach (var body in _bodies)
            {
                if (body.Enabled) body.UpdateY();
            }
        }
    }
}
