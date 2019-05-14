using CalendarEvents.Models;
using RestSharp;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace CalendarEvents
{
    public static class ApiEvents
    {
        const string CLIENT_ID = "eb3dd05a-2d79-48d3-9385-66c6300ce6f3";
        const string CLIENT_SECRET = "QpWuT28m3AdbsAUGwjDy1GK4Yub0PMjZYqdoEIqwWTQ=";

        static int _pageSize;
        static string _endPoint;

        static ApiEvents()
        {
            _endPoint = ConfigurationManager.AppSettings["EndPoint"];
            _pageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"]);
        }

        /// <summary>
        /// Get API events with paging and ordering.
        /// </summary>
        public static async Task<EventsModel> GetEvents(string token, int skip = 0)
        {
            var client = new RestClient(_endPoint);
            var request = new RestRequest("api/Events");

            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddParameter("$orderBy", "data/eventdate/iv/endDate desc");
            request.AddParameter("$top", _pageSize);
            request.AddParameter("$skip", skip);

            var response = await client.ExecuteTaskAsync<EventsModel>(request);
            return response.Data;
        }

        /// <summary>
        /// Create a new Event through the API.
        /// </summary>
        public static async Task<EventModel> CreateEvent(string token, EventModel model)
        {
            model.id = Guid.NewGuid();

            var client = new RestClient(_endPoint);
            var request = new RestRequest("api/Events", Method.POST);

            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddJsonBody(model);

            var response = await client.ExecuteTaskAsync<EventModel>(request);
            return response.Data;
        }

        /// <summary>
        /// Get access token for client id and secret.
        /// </summary>
        public static async Task<Token> GetToken()
        {
            var client = new RestClient(_endPoint);
            var request = new RestRequest("api/Auth");

            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json-patch+json");
            request.AddJsonBody(new { clientId = CLIENT_ID, clientSecret = CLIENT_SECRET });

            var response = await client.ExecutePostTaskAsync<Token>(request);
            return response.Data;
        }
    }
}