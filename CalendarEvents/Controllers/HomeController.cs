using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication6.Models;

namespace WebApplication6.Controllers
{
    public class HomeController : Controller
    {
        int _page_size;

        public HomeController()
        {
            _page_size = int.Parse(ConfigurationManager.AppSettings["PageSize"]);
        }

        public ActionResult Create()
        {
            return View();
        }

        public async Task<ActionResult> Index()
        {
            var data = await GetData();
            return View(data);
        }

        [HttpPost]
        public async Task<ActionResult> Index(events_model model)
        {
            var data = await GetData(model.page * model.page_size);
            data.page = model.page;
            return View(data);
        }

        [HttpPost]
        public async Task<ActionResult> Create(event_model model)
        {
            var token = await GetTokenCookie();

            var client = new RestClient("https://interview.cpdv.ninja/eb3dd05a-2d79-48d3-9385-66c6300ce6f3/");

            var request = new RestRequest("api/Events", Method.POST);
            request.AddHeader("Authorization", $"Bearer {token}");

            model.id = Guid.NewGuid();
            request.AddJsonBody(model);

            var response = await client.ExecuteTaskAsync<event_model>(request);

            if (!response.IsSuccessful)
            {
                foreach (var item in response.Data.details)
                    ModelState.AddModelError(response.Data.details.IndexOf(item).ToString(), item);

                return View();
            }
            else
                return RedirectToAction("Index");
        }

        async Task<events_model> GetData(int skip = 0)
        {
            var token = await GetTokenCookie();
            var client = new RestClient("https://interview.cpdv.ninja/eb3dd05a-2d79-48d3-9385-66c6300ce6f3/");

            var request = new RestRequest("api/Events");
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddParameter("$top", _page_size);
            request.AddParameter("$skip", skip);

            var response = await client.ExecuteTaskAsync<events_model>(request);
            return response.Data;
        }

        async Task<string> GetTokenCookie()
        {
            var token_cookie = Request.Cookies.Get("access_token");
            if (token_cookie == null)
            {
                var token = await GetToken();
                token_cookie = new System.Web.HttpCookie("access_token", token.access_token);
                token_cookie.Expires = DateTime.Now.AddSeconds(token.expires_in);
                Response.Cookies.Add(token_cookie);
            }
            return token_cookie.Value;
        }

        async Task<token> GetToken()
        {
            var client = new RestClient("https://interview.cpdv.ninja/eb3dd05a-2d79-48d3-9385-66c6300ce6f3/");

            var request = new RestRequest("api/Auth");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json-patch+json");

            request.AddJsonBody(new { clientId = "eb3dd05a-2d79-48d3-9385-66c6300ce6f3", clientSecret = "QpWuT28m3AdbsAUGwjDy1GK4Yub0PMjZYqdoEIqwWTQ=" });

            var response = await client.ExecutePostTaskAsync<token>(request);
            if (!response.IsSuccessful)
                throw new Exception(response.ErrorMessage);

            return response.Data;
        }
    }
}