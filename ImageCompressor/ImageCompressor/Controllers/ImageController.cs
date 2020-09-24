using ImageCompressor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static ImageCompressor.Helper.ImageResize;

namespace ImageCompressor.Controllers
{
    public class ImageController : ApiController
    {

        [Route("~/ImageResizer")]
        public HttpResponseMessage Post(ImageSet imageSet)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                string filePath = "~/images/";
                byte[] resizedImage = null;

                if (!string.IsNullOrWhiteSpace(imageSet.ImageURL))
                {
                    using (WebClient client = new WebClient())
                    {
                        List<ImageSize> imageResizeList = new List<ImageSize>();
                        ImageSize imageResize = new ImageSize();
                        //Check if the specified string contains white-space characters

                        client.Headers[HttpRequestHeader.Accept] = "application/json";
                        client.Headers[HttpRequestHeader.ContentType] = "application/json";
                        byte[] data = client.DownloadData(imageSet.ImageURL);

                        imageResize.Height = imageSet.Height;
                        imageResize.Width = imageSet.Width;
                        imageResize.FileName = "4040.jpg";
                        imageResizeList.Add(imageResize);
                        resizedImage = ImageResizer(data, filePath, imageResizeList);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { ResizedImage = resizedImage });
                }
                else
                {
                    if (imageSet?.ImageArray != null && imageSet.ImageArray.Any())
                    {
                        List<ImageSize> imageResizeList = new List<ImageSize>();
                        ImageSize imageResize = new ImageSize
                        {
                            Height = imageSet.Height,
                            Width = imageSet.Width,
                            FileName = "4040.jpg"
                        };
                        imageResizeList.Add(imageResize);
                        resizedImage = ImageResizer(imageSet.ImageArray, filePath, imageResizeList);
                        return Request.CreateResponse(HttpStatusCode.OK, new { ResizedImage = resizedImage });
                    }
                    return Request.CreateResponse(HttpStatusCode.NoContent);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

    }
}
