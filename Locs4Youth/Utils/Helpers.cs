using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using Vereyon.Web;

namespace Locs4Youth.Utils
{
    #region HTMLHelper
    public class HTMLHelper
    {
        public static string Clean(string dirtyHtml)
        {
            var sanitizer = new HtmlSanitizer();
            sanitizer.Tag("strong").RemoveEmpty();
            sanitizer.Tag("em").RemoveEmpty();
            sanitizer.Tag("ul").RemoveEmpty();
            sanitizer.Tag("li").RemoveEmpty();
            sanitizer.Tag("p").RemoveEmpty();
            sanitizer.Tag("ol").RemoveEmpty();
            return sanitizer.Sanitize(dirtyHtml);
        }
    }
    #endregion

    #region ImageHelper
    public class ImageHelper
    {
        private const string EXTENSION = ".jpg";

        /// <summary>
        /// Deletes an image from disk
        /// </summary>
        /// <param name="id">The name of image</param>
        /// <param name="dir">The directory</param>
        public static void DeleteImage(int id, string dir)
        {
            try
            {
                File.Delete(Path.Combine(dir, id + EXTENSION));
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// Gets avatar URL
        /// </summary>
        /// <param name="id">The name of image</param>
        /// <param name="dir">The directory</param>
        /// <returns>Avatar link</returns>
        public static string GetAvatar(string dir, int id)
        {
            string avatar = null, path = dir + id + ".jpg";
            if (File.Exists(path))
            {
                avatar = id + ".jpg";
            }
            else
            {
                avatar = "default.png";
            }
            return avatar;
        }

        /// <summary>
        /// Saves an image to disk
        /// </summary>
        /// <param name="img">The image</param>
        /// <param name="id">The name of image</param>
        /// <param name="dir">The directory</param>
        public static void SaveImage(Bitmap img, int id, string dir)
        {
            string path = Path.Combine(dir, id + EXTENSION);
            img.Save(path, ImageFormat.Jpeg);

            // Dispose image
            img.Dispose();
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
    #endregion
}