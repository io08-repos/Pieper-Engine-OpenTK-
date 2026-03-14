namespace PieperEngine.Buffers
{
    public interface IBuffer
    {
        public int ID { get; set; }

        public void Bind();
        public void Unbind();
        public void Delete();
    }
}
