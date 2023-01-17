using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Project_Api_Consume.Models;
using System.Collections.Generic;

namespace Project_Api_Consume.Controllers
{
    public class CurrencyController : Controller
    {
        public async Task< IActionResult> Index()
        {
            

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://booking-com.p.rapidapi.com/v1/metadata/exchange-rates?currency=TRY&locale=en-gb"),
                Headers =
    {
        { "X-RapidAPI-Key", "a3378a37b0msh5b71648c7f86cd8p163e36jsnf2a7310249f9" },
        { "X-RapidAPI-Host", "booking-com.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var currencies = JsonConvert.DeserializeObject<CurrencyListViewModel>(body);
                return View(currencies.exchange_rates);
            }
          
        }
    }
}
///deserialize: JSON => MODEL
///serialize: MODEL => JSON
