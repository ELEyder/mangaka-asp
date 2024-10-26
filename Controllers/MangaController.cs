using Mangaka.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace Mangaka.Controllers
{
    public class MangaController : Controller
    {
        public string apiUrl = "https://www.animeallstar20.com/feeds/posts/default/-/Nuevo?max-results=60&orderby=published&alt=json";
        public readonly HttpClient _httpClient;
        public MangaController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index(string titleManga)
        {
            Manga manga = new Manga();
            var response = await _httpClient.GetStringAsync(apiUrl);
            var data = JObject.Parse(response);
            var entries = data["feed"]?["entry"];
            if (entries != null && entries.Any())
            {
                foreach (var entry in entries)
                {
                    var title = entry["title"]?["$t"]?.ToString();
                    if (title != titleManga.Replace("%", "-")) continue;
                    var alternativeLinks = entry["link"];
                    var img = entry["content"]?["$t"]?.ToString();
                    Regex regex = new Regex("src=\"(.*?)\"");
                    MatchCollection matches = regex.Matches(img);
                    foreach (Match match in matches.Skip(1))
                    {
                        string chapter = match.Groups[1].Value;
                        manga.Chapter.Add(chapter);
                    }
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
                }
            }
            ViewBag.Manga = manga;
            return View();
        }

    }
}
