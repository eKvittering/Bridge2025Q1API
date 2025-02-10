namespace Webminux.Optician
{
    public class ImageDto
    {
        public string ImageBase64String { get; set; }

        public ImageDto(string imageBase64String)
        {
            ImageBase64String = imageBase64String;
        }
    }
}
