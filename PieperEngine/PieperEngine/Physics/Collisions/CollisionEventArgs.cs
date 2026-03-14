namespace PieperEngine.Physics.Collisions
{
    public readonly struct CollisionEventArgs
    {
        public readonly Collider Other { get; init; }
        public readonly ResolutionAxis Axis { get; init; }
    }
}
