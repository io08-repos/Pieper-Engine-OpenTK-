namespace PieperEngine.Buffers.Geometry
{
    /// <summary>
    /// Interface implemented into any data structure that vertices can be derived from.
    /// </summary>
    public interface IVertexSource
    {
        /// <summary>
        /// Generates basic vertex data with a position attribute.
        /// </summary>
        /// <returns>An array of vertices derived from this <see cref="IVertexSource"/>.</returns>
        public Vertex[] GenerateVertices();

        /// <summary>
        /// Fetches a constant set of indices for the shape to be derived from this <see cref="IVertexSource"/>.
        /// </summary>
        /// <returns>An array of indices linked with this <see cref="IVertexSource"/>'s shape type.</returns>
        public uint[]? GetIndices();
    }
}
