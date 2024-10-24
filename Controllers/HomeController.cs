using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            string url = "https://www.animeallstar20.com/feeds/posts/default/-/Nuevo?max-results=30&orderby=published&alt=json";
            var response = await _httpClient.GetStringAsync(url);
            string json = response;
            var data = JObject.Parse(json);
            var entries = data["feed"]?["entry"];
            var links = new List<string>();
            var titles = new List<string>();
            if (entries != null && entries.Any())
            {
                foreach (var entry in entries)
                {
                    var title = entry["title"]?["$t"]?.ToString();
                    if (!string.IsNullOrEmpty(title))
                    {
                        titles.Add(title);
                    }
                        var alternativeLinks = entry["link"];
                    if (alternativeLinks != null)
                    {
                        foreach (var link in alternativeLinks)
                        {
                            if (link["rel"]?.ToString() == "alternate")
                            {
                                var href = link["href"]?.ToString();
                                if (!string.IsNullOrEmpty(href))
                                {
                                    links.Add(href);
                                }
                            }
                        }
                    }
                }
            }

            ViewBag.Titles = titles;
            ViewBag.Links = links;
            return View();
        }
    }
}
