namespace Microservices.Services.PhotoStock.Dtos
{
    public class PhotoDto
    {
        public PhotoDto(string url)
        {
            URL = url;
        }

        public string URL { get; set; }
    }
}