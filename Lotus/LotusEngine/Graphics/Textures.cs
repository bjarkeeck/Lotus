using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using SharpGL;

namespace LotusEngine
{
    public static class Textures
    {
        private static List<Texture> textures;

        /// <summary>
        /// Unloads all textures from memory and unbinds them from the given OpenGL object.
        /// </summary>
        public static void UnloadAllTextures()
        {
            if (textures != null)
            {
                Rendering.gl.DeleteTextures(textures.Count, textures.ConvertAll<uint>(n => n.OpenGLName).ToArray());

                textures = new List<Texture>();
            }
        }

        /// <summary>
        /// Loads all textures in Settings.Assets.TexturePath and subdirs into memory and binds them to the given OpenGL object.
        /// Note that it is a bad idea for two textures to have the same filename, as retrieving them from memory will not be reliable in the given case.
        /// </summary>
        public static void LoadAllTextures()
        {
            UnloadAllTextures();

            if (!Directory.Exists(Settings.Assets.TexturePath))
                Directory.CreateDirectory(Settings.Assets.TexturePath);

            DirectoryInfo dirInfo = new DirectoryInfo(Settings.Assets.TexturePath);

            OpenGL gl = Rendering.gl;

            textures = new List<Texture>();

            List<FileInfo> textureFiles = GetAllTextureFilesFromSubDirs(dirInfo);

            uint[] textureOpenGLNames = new uint[textureFiles.Count];

            gl.GenTextures(textureOpenGLNames.Length, textureOpenGLNames);

            for (int i = 0; i < textureOpenGLNames.Length; i++)
            {
                gl.BindTexture(OpenGL.GL_TEXTURE_2D, textureOpenGLNames[i]);

                Bitmap bitmap = new Bitmap(textureFiles[i].FullName);

                int bitmapFormat,
                    openGLFormat;

                if (bitmap.PixelFormat == PixelFormat.Format24bppRgb)
                {
                    bitmapFormat = (int)OpenGL.GL_RGB;
                    openGLFormat = (int)OpenGL.GL_BGR;
                }
                else if (bitmap.PixelFormat == PixelFormat.Format32bppArgb)
                {
                    bitmapFormat = (int)OpenGL.GL_RGBA;
                    openGLFormat = (int)OpenGL.GL_BGRA;
                }
                else
                {
                    bitmap.Dispose();
                    continue;
                }

                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                                                        ImageLockMode.ReadOnly,
                                                        bitmap.PixelFormat);

                gl.TexImage2D(OpenGL.GL_TEXTURE_2D, 
                                0, 
                                bitmapFormat,
                                bitmap.Width, 
                                bitmap.Height, 
                                0,
                                (uint)openGLFormat, 
                                OpenGL.GL_UNSIGNED_BYTE,
                                bitmapData.Scan0);

                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, OpenGL.GL_LINEAR);
                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, OpenGL.GL_LINEAR);
                //gl.Hint(OpenGL.GL_GENERATE_MIPMAP_HINT_SGIS, OpenGL.GL_FASTEST);
                //gl.GenerateMipmapEXT(OpenGL.GL_TEXTURE_2D);

                bitmap.UnlockBits(bitmapData);

                textures.Add(new Texture(textureOpenGLNames[i], Path.GetFileNameWithoutExtension(textureFiles[i].Name), bitmap, textureFiles[i].FullName));
            }
        }

        /// <summary>
        /// Recursively gets a list of all texture files in the given directory, as well as in any subdirectories.
        /// </summary>
        /// <param name="dirInfo">The given directory.</param>
        /// <returns>A list of all texture files in the given directory.</returns>
        private static List<FileInfo> GetAllTextureFilesFromSubDirs(DirectoryInfo dirInfo)
        {
            List<FileInfo> result = new List<FileInfo>();

            foreach (var file in dirInfo.GetFiles())
            {
                if (file.Extension == ".bmp"
                    || file.Extension == ".gif"
                    || file.Extension == ".exif"
                    || file.Extension == ".jpg"
                    || file.Extension == ".png"
                    || file.Extension == ".tiff")
                {
                    result.Add(file);
                }
            }

            foreach (var dir in dirInfo.GetDirectories())
            {
                result.AddRange(GetAllTextureFilesFromSubDirs(dir));
            }

            return result;
        }

        /// <summary>
        /// Gets a texture.
        /// </summary>
        /// <param name="name">The name of the texture to get.</param>
        public static Texture GetTexture(string name)
        {
            return textures.FirstOrDefault(n => n.Name == name);
        }

        /// <summary>
        /// Gets all textures.
        /// </summary>
        /// <returns></returns>
        public static List<Texture> GetAllTextures()
        {
            return new List<Texture>(textures);
        }

        /// <summary>
        /// Determines whether a texture currently exists.
        /// </summary>
        /// <param name="name">The name of the texture to check for.</param>
        /// <returns>True if the texture exists, otherwise false.</returns>
        public static bool TextureExists(string name)
        {
            return textures.Any(n => n.Name == name);
        }
    }
}
