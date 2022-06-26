
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace JobsityTests
{
    public class JobsityBotTests
    {
        
        [Test]
        public async Task GetStock_Ok()
        {
            var application = new WebApplicationFactory<Program>();
            using (var client = application.CreateClient())
            {
                var code = "aapl.us";
                var response = await client.GetAsync($"api/JobsityStockBot/GetStock?stockCode={code}");
                response.EnsureSuccessStatusCode();
                Assert.NotNull(response);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }

        /// <summary>
        /// No code response test
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetStock_NotSuccess()
        {
            var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // set up servises
                });
            });
            using (var client = application.CreateClient())
            {
                var code = string.Empty;
                var response = await client.GetAsync($"api/JobsityStockBot/GetStock?stockCode={code}");
                var notSuccess = !response.IsSuccessStatusCode;
                Assert.True(notSuccess);
            }
        }
    }
}