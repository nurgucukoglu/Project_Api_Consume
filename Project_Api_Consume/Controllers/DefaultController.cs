using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project_Api_Consume.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Project_Api_Consume.Controllers
{
    public class DefaultController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DefaultController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task< IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:44350/api/Category"); //get url aldık çünkü get kullanıcaz.

            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsondata = await responseMessage.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<CategoryViewModel>>(jsondata); //gelen json veriyi list şeklinde görüntülemek için deserialize ettik.
                return View(result);
            }
            else
            {
                ViewBag.responseMessage = "Bir hata oluştu";
                return View();
            }
        }

        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryViewModel p)
        {
            p.Status = true;

            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(p); //burdaki verileri apiye göndericem
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("https://localhost:44350/api/Category", content); //(isteğin yapılacağı adres,content)

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            else
            {
                ViewBag.responseMessage = "Bir Hata Oluştu";
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCategory(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"https://localhost:44350/api/Category/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsondata = await responseMessage.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<CategoryViewModel>(jsondata); //tek veri geleceği için list kullanmadık.  //güncellenecek verileri getirirken serialize
                return View(result);
            }
            else
            {
                ViewBag.responseMessage = "Bir Hata Oluştu";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(CategoryViewModel p)
        {
            p.Status = true;
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(p);  //veri gönderiyoruz apiye deserialize

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync("https://localhost:44350/api/Category/", content);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.responseMessage = "Bir Hata Oluştu";
                return View();
            }
        }

        public async Task<IActionResult> DeleteCategory(int id)
        {
            var client = _httpClientFactory.CreateClient();
            await client.DeleteAsync($"https://localhost:44350/api/Category/{id}");
            return RedirectToAction("Index");
        }

    }
}
