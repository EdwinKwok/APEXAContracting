using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace APEXAContracting.Common.Helpers
{
    public class ImageHelper
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageBytes"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static byte[] ResizeImage(byte[] imageBytes, Size size)
        {
            MemoryStream ms = new MemoryStream(imageBytes);
            Image resizeImage = Image.FromStream(ms, true);
            ms.Close();

            Image resizedImage = ResizeImage(resizeImage, size);

            int cutWidth = resizedImage.Width > resizedImage.Height ? resizedImage.Height : resizedImage.Width;
            int cutHeight = cutWidth;
            Image cutImage = CutImage(resizedImage, 0, 0, cutWidth, cutHeight);

            ms = new MemoryStream();
            cutImage.Save(ms, ImageFormat.Png);
            byte[] bytes = ms.ToArray();
            ms.Close();

            return bytes;
        }

        /// <summary>
        /// onle special the height of the image
        /// </summary>
        /// <param name="imageBytes"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static byte[] ResizeImage(byte[] imageBytes, int height)
        {
            MemoryStream ms = new MemoryStream(imageBytes);
            Image resizeImage = Image.FromStream(ms, true);
            ms.Close();


            //cal the width of the image
            decimal resizeWidth = Math.Ceiling(resizeImage.Width * (decimal.Parse(height.ToString()) / decimal.Parse(resizeImage.Height.ToString())));

            Size resize = new Size(Int32.Parse(Math.Round(resizeWidth, 0).ToString()), height);
            Image resizedImage = ResizeImage(resizeImage, resize);

            ms = new MemoryStream();
            resizedImage.Save(ms, ImageFormat.Png);
            byte[] bytes = ms.ToArray();
            ms.Close();
            return bytes;
        }
        /// <summary>
        /// resize the image base on orginal aspect ratio
        /// </summary>
        /// <param name="imgToResize"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Image ResizeImage(Image imgToResize, Size size)
        {
            if (imgToResize.Height == size.Height && imgToResize.Width == size.Width)
                return imgToResize;

            //get the size of the source image    
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            //get the ratio of source image   
            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            //get the destination size     
            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(size.Width, size.Height);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            int beginX = 0;
            if (size.Width > destWidth)
                beginX = (size.Width - destWidth) / 2;
            int beginY = 0;
            if (size.Height > destHeight)
                beginY = (size.Height - destHeight) / 2;
            //drawing the image     
            g.DrawImage(imgToResize, beginX, beginY, destWidth, destHeight);
            g.Dispose();

            return (System.Drawing.Image)b;
        }

        /// <summary>
        /// cut image 
        /// </summary>
        /// <param name="fromImage"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Image CutImage(Image fromImage, int offsetX, int offsetY, int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height);

            Graphics graphic = Graphics.FromImage(bitmap);

            graphic.DrawImage(fromImage, 0, 0, new Rectangle(offsetX, offsetY, width, height), GraphicsUnit.Pixel);

            Image CutImage = Image.FromHbitmap(bitmap.GetHbitmap());

            return CutImage;
        }

        public enum ImageResolution
        {
            High = 1,
            Medium = 2,
            Low = 3
        }

        public static void ResizeImage(string filePath, ImageResolution imgRes)
        {
            int width = 400;
            int height = 400;
            switch (imgRes)
            {
                case ImageResolution.Medium:
                    width = 400;
                    height = 400;
                    break;
                case ImageResolution.Low:
                    width = 100;
                    height = 100;
                    break;
            }

            ImageProcessor img = new ImageProcessor(filePath);

            if (img.Width > width || img.Height > height)
            {
                img.Resize(width, height, true);

                img.Save(filePath, ImageFormat.Jpeg);
            }
        }

        /// <summary>
        ///  Based on the input file extension, convert it to acceptable mime type. 
        ///  https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Complete_list_of_MIME_types
        ///  Work for download/view image file or other kind of documents which has been saved in Azure Blob Storage.
        /// </summary>
        /// <param name="fileExtension">File's extension. Got from BlobName or original file name.</param>
        /// <returns>return string of Mime type name.</returns>
        public static string GetFileMimeType(string fileExtension) {
            string result = string.Empty;

            switch (fileExtension.Trim().ToLower()) {
                case "bmp": result = "image/bmp"; break;
                case "gif": result = "image/gif"; break;
                case "jpeg": result = "image/jpeg"; break;
                case "jpg": result = "image/jpeg"; break;
                case "png": result = "image/png"; break;
                case "svg": result = "image/svg+xml"; break;
                case "tif": result = "image/tiff"; break;
                case "tiff": result = "image/tiff"; break;
                case "webp": result = "image/webp"; break;
                case "csv": result = "text/csv"; break;
                case "css": result = "text/css"; break;
                case "doc": result = "application/msword"; break;
                case "docx": result = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"; break;
                case "htm": result = "text/html"; break;
                case "html": result = "text/html"; break;
                case "ico": result = "image/vnd.microsoft.icon"; break;
                case "pdf": result = "application/pdf"; break;
                case "ppt": result = "application/vnd.ms-powerpoint"; break;
                case "pptx": result = "application/vnd.openxmlformats-officedocument.presentationml.presentation"; break;
                case "swcf": result = "application/x-shockwave-flash"; break;
                case "txt": result = "text/plain"; break;
                case "xls": result = "application/vnd.ms-excel"; break;
                case "xlsx": result = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; break;
                case "xml": result = "application/xml"; break;
                default: result = "application/octet-stream"; break;
            }
            return result;
        }
    }
}
