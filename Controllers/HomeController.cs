using Mangaka.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Mangaka.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            string apiUrl = "https://www.animeallstar20.com/feeds/posts/default/-/Nuevo?max-results=30&orderby=published&alt=json";
            var response = await _httpClient.GetStringAsync(apiUrl);
            var data = JObject.Parse(response);
            var entries = data["feed"]?["entry"];
            var mangas = new List<Manga>();
            if (entries != null && entries.Any())
            {
                foreach (var entry in entries)
                {
                    Manga manga = new Manga();
                    var alternativeLinks = entry["link"];
                    var title = entry["title"]?["$t"]?.ToString();
                    var img = entry["content"]?["$t"]?.ToString();
                    Regex regex = new Regex("src=\"(.*?)\"");
                    MatchCollection matches = regex.Matches(img);
                    img = matches.Count > 0 ? matches[0].Groups[1].Value : null;

                    if (!string.IsNullOrEmpty(title))
                    {
                        manga.Title = title;
                    }
                    if (!string.IsNullOrEmpty(img))
                    {
                        manga.Img = img;
                    }
                    if (alternativeLinks != null)
                    {
                        foreach (var link in alternativeLinks)
                        {
                            if (link["rel"]?.ToString() == "alternate")
                            {
                                var url = link["href"]?.ToString();
                                if (!string.IsNullOrEmpty(url))
                                {
                                    manga.Url = url;
                                }
                            }
                        }
                    }
                    mangas.Add(manga);
                }
            }
            ViewBag.Mangas = mangas;
            return View();
        }
    }
}
