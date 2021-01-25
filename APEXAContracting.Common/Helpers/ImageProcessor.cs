using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace APEXAContracting.Common.Helpers
{
    public class ImageProcessor : IDisposable
    {
        private System.Drawing.Image _image = null;
        private System.Drawing.Image InternalImage
        {
            get
            {
                return this._image;
            }
            set
            {
                if (this._image != null)
                {
                    this._image.Dispose();
                }

                this._image = value;
            }
        }

        #region Constructor
        public ImageProcessor(Stream stream)
        {
            this.InternalImage = System.Drawing.Image.FromStream(stream);
        }
        public ImageProcessor(string filename)
        {
            System.IO.FileInfo file = new FileInfo(filename);
            if (file.Exists)
            {
                this.InternalImage = System.Drawing.Image.FromFile(filename);
            }
        }
        #endregion

        #region Resize
        public void Resize(int width, int height)
        {
            this.Resize(width, height, false);
        }
        public void Resize(int width, int height, bool lockRatio)
        {
            this.Resize(width, height, lockRatio, false);
        }
        /// <summary>
        /// Resizes image
        /// </summary>
        /// <param name="width">Width of new image</param>
        /// <param name="height">Height of new image</param>
        /// <param name="lockRatio">Maintain aspect ratio - default false</param>
        /// <param name="maximize">Fill canvas - default no</param>
        public void Resize(int width, int height, bool lockRatio, bool maximize)
        {
            if (lockRatio)
            {
                double ratio = (maximize)
                    ? Math.Max((double)width / this.InternalImage.Width, (double)height / this.InternalImage.Height)
                    : Math.Min(1.0, Math.Min((double)width / this.InternalImage.Width, (double)height / this.InternalImage.Height));
                width = Convert.ToInt32(this.InternalImage.Width * ratio);
                height = Convert.ToInt32(this.InternalImage.Height * ratio);
            }

            Bitmap retval = new Bitmap(width, height);

            Graphics graphics = Graphics.FromImage(retval);
            graphics.FillRectangle(new SolidBrush(Color.White), 0, 0, width, height);
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.DrawImage(this.InternalImage, 0, 0, retval.Width, retval.Height);

            this.InternalImage = retval;
        }

        public static ImageProcessor Resize(Stream stream, int width, int height, bool lockRatio)
        {
            return ImageProcessor.Resize(stream, width, height, lockRatio, false);
        }
        public static ImageProcessor Resize(Stream stream, int width, int height, bool lockRatio, bool maximize)
        {
            ImageProcessor retval = new ImageProcessor(stream);

            retval.Resize(width, height, lockRatio, maximize);

            return retval;
        }

        public static ImageProcessor Resize(string path, int width, int height, bool lockRatio)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                return ImageProcessor.Resize(stream, width, height, lockRatio);
            }
        }

        public static ImageProcessor Resize(string path, int width, int height, bool lockRatio, bool maximize)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                return ImageProcessor.Resize(fs, width, height, lockRatio, maximize);
            }
        }

        #endregion

        #region Save
        public void Save(string filename)
        {
            this.Save(filename, this.InternalImage.RawFormat);
        }
        public void Save(string filename, ImageFormat format)
        {
            this.InternalImage.Save(filename, format);
        }
        #endregion

        #region RawFormat
        public ImageFormat RawFormat
        {
            get
            {
                return this.InternalImage.RawFormat;
            }
        }
        #endregion
        
        #region Width
        public int Width
        {
            get
            {
                return this.InternalImage.Width;
            }
        }
        #endregion
        
        #region Height
        public int Height
        {
            get
            {
                return this.InternalImage.Height;
            }
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this.InternalImage.Dispose();
        }

        #endregion
    }
}
