namespace PieperEngine.Time
{
    public static class TimeSystem
    {
        public static float DeltaTime { get; private set; }
        public static float Time { get; private set; }
        public static float Scale { get; private set; } = 1f;

        public static void Update(float deltaTime)
        {
            DeltaTime = deltaTime * Scale;
            Time += deltaTime;
        }

        public static void SetScale(float scale) => Scale = scale;
    }
}
