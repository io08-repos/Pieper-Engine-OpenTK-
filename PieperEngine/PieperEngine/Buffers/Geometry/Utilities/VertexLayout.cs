namespace PieperEngine.Buffers.Geometry.Utilities
{
    public static class VertexLayout
    {
        public static readonly int[] AttributeSizes =
        [
            3, //Position
            4, //Color
            2, //UV0
            2  //UV1
        ];

        public static readonly int[] AttributeOffsets;

        static VertexLayout()
        {
            AttributeOffsets = new int[(int)VertexAttribute.MaxAttributes];
            int offset = 0;
            for (int i = 0; i < (int)VertexAttribute.MaxAttributes; i++)
            {
                AttributeOffsets[i] = offset;
                offset += AttributeSizes[i];
            }

            VertexStride = offset;
        }

        public static readonly int VertexStride;
    }
}
