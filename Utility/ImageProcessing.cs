using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;
using System.Linq;

namespace Utility
{
    public class ImageProcessing
    {
        long qulityImageConfig;
        public ImageProcessing(long _qulityImageConfig)
        {
            qulityImageConfig = _qulityImageConfig;
        }
        public bool CheckImageRatio(string orginalFullFileName)
        {
            using (var openfile = System.Drawing.Image.FromFile(orginalFullFileName))
            {
                using (var image = new Bitmap(openfile))
                {
                    int diff = image.Width - image.Height;
                    image.Dispose();
                    openfile.Dispose();
                    if (diff > 10 || diff < -10)
                    {
                        return false;
                    }
                    else { return true; }
                    
                }
                
            }
        }
        public void ReSizeImage(string orginalFullFileName, string createdFullFileName, int size)
        {
           // const long quality = 90L;
            using (var openfile = System.Drawing.Image.FromFile(orginalFullFileName))
            {
                using (var image = new Bitmap(openfile))
                {
                    int width, height;
                    if (image.Width > image.Height)
                    {
                        width = size;
                        height = Convert.ToInt32(image.Height * size / (double)image.Width);
                    }
                    else
                    {
                        width = Convert.ToInt32(image.Width * size / (double)image.Height);
                        height = size;
                    }

                    using (var resizeBitmap = ResizeImage(image, width, height))
                    {
                        //EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
                        //myEncoderParameters.Param[0] = myEncoderParameter;
                        //bmp1.Save(@"e:\TestPhotoQualityFifty.jpg", jpgEncoder, myEncoderParameters);

                        var qualityParamId = System.Drawing.Imaging.Encoder.Quality;
                        var encoderParameters = new EncoderParameters(1);
                        // کیفیت عکس 
                        encoderParameters.Param[0] = new EncoderParameter(qualityParamId, qulityImageConfig);
                        var codec = ImageCodecInfo.GetImageDecoders()
                            .FirstOrDefault(codec => codec.FormatID == ImageFormat.Jpeg.Guid);
                     //   resized.Save(output, codec, encoderParameters);


                        resizeBitmap.Save(createdFullFileName ,codec, encoderParameters);
                        resizeBitmap.Dispose();
                    }

                    image.Dispose();
                    openfile.Dispose();
                }
            }

        }
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }

            return null;
        }
        public void VaryQualityLevel()
        {
            // Get a bitmap.
            Bitmap bmp1 = new Bitmap(@"c:\TestPhoto.jpg");
            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);

            // Create an Encoder object based on the GUID
            // for the Quality parameter category.
            System.Drawing.Imaging.Encoder myEncoder =
                System.Drawing.Imaging.Encoder.Quality;

            // Create an EncoderParameters object.
            // An EncoderParameters object has an array of EncoderParameter
            // objects. In this case, there is only one
            // EncoderParameter object in the array.
            EncoderParameters myEncoderParameters = new EncoderParameters(1);

            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
            myEncoderParameters.Param[0] = myEncoderParameter;
            bmp1.Save(@"e:\TestPhotoQualityFifty.jpg", jpgEncoder, myEncoderParameters);

            myEncoderParameter = new EncoderParameter(myEncoder, 100L);
            myEncoderParameters.Param[0] = myEncoderParameter;
            bmp1.Save(@"e:\TestPhotoQualityHundred.jpg", jpgEncoder, myEncoderParameters);

            // Save the bitmap as a JPG file with zero quality level compression.
            myEncoderParameter = new EncoderParameter(myEncoder, 0L);
            myEncoderParameters.Param[0] = myEncoderParameter;
            bmp1.Save(@"e:\TestPhotoQualityZero.jpg", jpgEncoder, myEncoderParameters);
        }
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

          //  destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            destImage.SetResolution(25.0F, 25.0F);
     
            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                //graphics.SmoothingMode = SmoothingMode.HighSpeed;
                //graphics.PixelOffsetMode = PixelOffsetMode.Half;
                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);

                    #region MyRegion
                    //string firstText = "Hello";

                    //PointF firstLocation = new PointF(10f, 10f);

                    //using (Font arialFont = new Font("Arial", 10))
                    //{
                    //    graphics.DrawString(firstText, arialFont, Brushes.Blue, firstLocation);
                    //   // graphics.DrawString(secondText, arialFont, Brushes.Red, secondLocation);
                    //}

                    #endregion

                    wrapMode.Dispose();
                }
                graphics.Dispose();
            } // 
            destImage.SetResolution(25.0F, 25.0F);
            return destImage;
        }

        //var resized = new Bitmap(width, height);
        //using (var graphics = Graphics.FromImage(resized))
        //{
        //    graphics.CompositingQuality = CompositingQuality.HighSpeed;
        //    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //    graphics.CompositingMode = CompositingMode.SourceCopy;
        //    graphics.DrawImage(image, 0, 0, width, height);
        //    //using (var output = File.Open(createdFullFileName, FileMode.Create))
        //    //{
        //    var output = File.Open(createdFullFileName, FileMode.Create);
        //        var qualityParamId = System.Drawing.Imaging.Encoder.Quality;
        //        var encoderParameters = new EncoderParameters(1);
        //        encoderParameters.Param[0] = new EncoderParameter(qualityParamId, quality);
        //        var codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(codec => codec.FormatID == ImageFormat.Jpeg.Guid);
        //        try
        //        {
        //            resized.Save(output, codec, encoderParameters);

        //           // resized.Save(createdFullFileName, System.Drawing.Imaging.ImageFormat.Jpeg);
        //        }
        //        catch (Exception ex)
        //        {

        //            throw;
        //        }

        //    //}
        //}
    }


}
