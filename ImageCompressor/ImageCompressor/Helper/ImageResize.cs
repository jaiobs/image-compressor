using ImageCompressor.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;

namespace ImageCompressor.Helper
{
    public class ImageResize
    {
        public static byte[] ResizeImage(int size, Bitmap curImage)
        {
            Bitmap newImage = null;
            Graphics imgDest = null;
            try
            {
                //variables for image dimension/scale
                double newHeight = 0;
                double newWidth = 0;
                double scale = 0;

                //Determine image scaling
                if (curImage.Height > curImage.Width)
                {
                    scale = Convert.ToSingle(size) / curImage.Height;
                }
                else
                {
                    scale = Convert.ToSingle(size) / curImage.Width;
                }
                if (scale < 0 || scale > 1) { scale = 1; }
                if (curImage.PropertyIdList.Contains(OrientationKey))
                {
                    var orientation = (int)curImage.GetPropertyItem(OrientationKey).Value[0];
                    switch (orientation)
                    {
                        case NormalOrientation:
                            // No rotation required.
                            break;
                        case MirrorHorizontal:
                            curImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
                            break;
                        case UpsideDown:
                            curImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            break;
                        case MirrorVertical:
                            curImage.RotateFlip(RotateFlipType.Rotate180FlipX);
                            break;
                        case MirrorHorizontalAndRotateRight:
                            curImage.RotateFlip(RotateFlipType.Rotate90FlipX);
                            break;
                        case RotateLeft:
                            curImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            break;
                        case MirorHorizontalAndRotateLeft:
                            curImage.RotateFlip(RotateFlipType.Rotate270FlipX);
                            break;
                        case RotateRight:
                            curImage.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            break;
                        default:
                            break;
                            //throw new NotImplementedException("An orientation of " + orientation + " isn't implemented.");
                    }
                }
                //New image dimension
                newHeight = (double)decimal.Divide((decimal)curImage.Height, (decimal)curImage.Width) * size;//Math.Floor(Convert.ToSingle(curImage.Height) * scale);
                newWidth = size;

                //Create new object image
                newImage = new Bitmap(curImage, Convert.ToInt32(newWidth), Convert.ToInt32(newHeight));
                imgDest = Graphics.FromImage(newImage);
                imgDest.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                imgDest.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                imgDest.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                imgDest.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                ImageCodecInfo[] info = ImageCodecInfo.GetImageEncoders();
                EncoderParameters param = new EncoderParameters(1);
                param.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
                //Save image file
                //newImage.Save(saveFilePath, info[1], param);
                Stream image = new MemoryStream();
                newImage.Save(image, ImageFormat.Jpeg);
                System.Drawing.Image img = System.Drawing.Image.FromStream(image);
                //img.Save(System.IO.Path.GetTempPath() + "\\myImage.Jpg", ImageFormat.Jpeg);
                ImageConverter converter = new ImageConverter();
                byte[] imgArray = (byte[])converter.ConvertTo(img, typeof(byte[]));
                return imgArray;
            }
            catch (Exception)
            {
                curImage.Dispose();
                newImage.Dispose();
                imgDest.Dispose();
                return null;
            }
        }

        public static byte[] ReadImageFile(string imageLocation)
        {
            FileInfo fileInfo = new FileInfo(imageLocation);
            long imageFileLength = fileInfo.Length;
            FileStream fs = new FileStream(imageLocation, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            byte[] imageData = br.ReadBytes((int)imageFileLength);
            return imageData;
        }

        public static byte[] ImageResizer(byte[] myBytes, string BasePath, List<ImageSize> imageResizeList)
        {
            try
            {
                List<ImageResizeModel> list = new List<Models.ImageResizeModel>();
                ImageResizeModel imageResizeModel;
                Stream sourcePath = new MemoryStream(myBytes);
                foreach (var image in imageResizeList)
                {
                    Bitmap original = new Bitmap(sourcePath);

                    imageResizeModel = new ImageResizeModel
                    {
                        ReSizedImage = ResizeImage(image.Height, original)
                    };
                    list.Add(imageResizeModel);
                }
                byte[] img = list.FirstOrDefault().ReSizedImage;
                return img;
            }
            catch (Exception)
            {
                return null;
            }
        }



        public class ImageSize
        {
            public int Height { get; set; }
            public int Width { get; set; }
            public string FileName { get; set; }
        }



        private const int OrientationKey = 0x0112;
        private const int NormalOrientation = 1;
        private const int MirrorHorizontal = 2;
        private const int UpsideDown = 3;
        private const int MirrorVertical = 4;
        private const int MirrorHorizontalAndRotateRight = 5;
        private const int RotateLeft = 6;
        private const int MirorHorizontalAndRotateLeft = 7;
        private const int RotateRight = 8;
    }
}