namespace ImageCompressor.Models
{
    public class ImageResizeModel
    {
        public byte[] ReSizedImage { get; set; }
    }

    public class ImageSet
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public string ImageURL { get; set; }
        public byte[] ImageArray { get; set; }
    }
}