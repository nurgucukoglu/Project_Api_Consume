using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using Project_Api_Consume.Models;

namespace Project_Api_Consume.Controllers
{
    public class MovieController : Controller
    {
        public async Task<IActionResult> Index()
        {
           List<MovieListViewModel> movies = new List<MovieListViewModel>();

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://imdb-top-100-movies.p.rapidapi.com/"),
                Headers =
    {
        { "X-RapidAPI-Key", "a3378a37b0msh5b71648c7f86cd8p163e36jsnf2a7310249f9" },
        { "X-RapidAPI-Host", "imdb-top-100-movies.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                movies = JsonConvert.DeserializeObject<List<MovieListViewModel>>(body);  //datayı getirdiğim için deserialize
            }
            return View(movies);
        }
    }
}
