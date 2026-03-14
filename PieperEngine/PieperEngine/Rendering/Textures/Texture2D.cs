using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

using PieperEngine.Buffers;

namespace PieperEngine.Rendering.Textures
{
    public class Texture2D : IBuffer
    {
        public int ID { get; set; }
        public ImageResult Image { get; set; }
        
        public Texture2D(string path)
        {
            ID = GL.GenTexture();

            Bind();

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            StbImage.stbi_set_flip_vertically_on_load(1);
            Image = ImageResult.FromStream(File.OpenRead(path), ColorComponents.RedGreenBlueAlpha);

            UploadImage();
        }

        public void Bind() => GL.BindTexture(TextureTarget.Texture2D, ID);
        public void Unbind() => GL.BindTexture(TextureTarget.Texture2D, 0);
        public void Delete() => GL.DeleteTexture(ID);

        public void UploadImage()
        {
            Bind();

            int level = 0;
            int border = 0;
            GL.TexImage2D
            (
                TextureTarget.Texture2D,
                level,
                PixelInternalFormat.Rgba,
                Image.Width,
                Image.Height,
                border,
                PixelFormat.Rgba,
                PixelType.UnsignedByte,
                Image.Data
            );

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            Unbind();
        }

        public void Use(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            Bind();
        }
    }
}
