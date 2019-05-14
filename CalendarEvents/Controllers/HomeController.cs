using CalendarEvents.Models;
using System;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Web.Mvc;

namespace CalendarEvents.Controllers
{
    /// <summary>
    /// Controller for Calendar Events
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Show empty Create view.
        /// </summary>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Show calendar events for the first time.
        /// </summary>
        public async Task<ActionResult> Index()
        {
            var token = await GetTokenCache();
            var data = await ApiEvents.GetEvents(token);
            return View(data);
        }

        [HttpPost]
        /// <summary>
        /// Show calendar events for paging.
        /// </summary>
        public async Task<ActionResult> Index(EventsModel model)
        {
            var token = await GetTokenCache();
            var data = await ApiEvents.GetEvents(token, model.page * model.page_size);
            data.page = model.page;
            return View(data);
        }

        [HttpPost]
        /// <summary>
        /// Create a new event and redirects to Index view if successfull or show error if not.
        /// </summary>
        public async Task<ActionResult> Create(EventModel model)
        {
            var token = await GetTokenCache();
            var result = await ApiEvents.CreateEvent(token, model);

            if (result.statusCode != 0)
            {
                foreach (var item in result.details)
                    ModelState.AddModelError(string.Empty, item);

                return View();
            }
            else
                return RedirectToAction("Index");
        }

        /// <summary>
        /// Check whether access_token cache is created and stores if not.
        /// </summary>
        async Task<string> GetTokenCache()
        {
            var tokenCache = (Token)HttpContext.Cache.Get("access_token");
            if (tokenCache == null)
            {
                tokenCache = await ApiEvents.GetToken();
                HttpContext.Cache.Add("access_token", tokenCache, null, DateTime.Now.AddSeconds(tokenCache.expires_in),
                    Cache.NoSlidingExpiration, CacheItemPriority.AboveNormal, null);
            }
            return tokenCache.access_token;
        }
    }
}