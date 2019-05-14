using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CalendarEventsUnitTests
{
    [TestClass]
    public class ApiEventsUnitTest
    {
        [TestMethod]
        public async Task TestGetToken()
        {
            var token = await CalendarEvents.ApiEvents.GetToken();
            Assert.IsNotNull(token.access_token);
        }

        [TestMethod]
        public async Task TestGetEvents()
        {
            var token = await CalendarEvents.ApiEvents.GetToken();
            var events = await CalendarEvents.ApiEvents.GetEvents(token.access_token);
            Assert.IsTrue(events.statusCode == 0);
        }
    }
}
