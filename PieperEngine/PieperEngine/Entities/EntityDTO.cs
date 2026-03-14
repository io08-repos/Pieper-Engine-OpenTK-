using OpenTK.Mathematics;

namespace PieperEngine.Entities
{
    [Serializable]
    public struct EntityDTO
    {
        public string ID { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
        public float ScaleX { get; set; }
        public float ScaleY { get; set; }
        public float Rotation { get; set; }

        public void SetID(string id) => ID = id;

        public void SetPosition(Vector3 value)
        {
            PositionX = value.X;
            PositionY = value.Y;
            PositionZ = value.Z;
        }

        public void SetScale(Vector2 value)
        {
            ScaleX = value.X;
            ScaleY = value.Y;
        }

        public void SetRotation(float rotation) => Rotation = rotation;
    }
}
