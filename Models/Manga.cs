namespace Mangaka.Models
{
    public class Manga
    {
        public string? Title { get; set; }
        public string? Url { get; set; }
        public string? Img { get; set; }
        public List<string> Chapter { get; set; } = new List<string>();
    }
}
