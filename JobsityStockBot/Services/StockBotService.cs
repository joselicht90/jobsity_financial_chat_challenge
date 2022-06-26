using JobsityCommons.Models;

namespace JobsityStockBot.Services
{
    public class StockBotService : IStockBotService
    {
        public HttpClient Client { get; }

        public StockBotService(HttpClient _client)
        {
            Client = _client;
        }

        /// <summary>
        /// Download csv and deserialize into object
        /// </summary>
        /// <param name="stockCode"></param>
        /// <returns></returns>
        public Stock GetStockByCode(string stockCode)
        {
            using (HttpResponseMessage response = Client.GetAsync($"https://stooq.com/q/l/?s={stockCode}&f=sd2t2ohlcv&h&e=csv").Result)
            using (HttpContent content = response.Content)
            {
                var callResponse = content.ReadAsStringAsync().Result;
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new ArgumentException(callResponse);
                var data = callResponse.Substring(callResponse.IndexOf(Environment.NewLine, StringComparison.Ordinal) + 2);
                var processedArray = data.Split(',');
                return new Stock()
                {
                    Symbol = processedArray[0],
                    Date = !processedArray[1].Contains("N/D") ? Convert.ToDateTime(processedArray[1]) : default,
                    Time = !processedArray[2].Contains("N/D") ? Convert.ToDateTime(processedArray[2]).TimeOfDay : default,
                    Open = !processedArray[3].Contains("N/D") ? Convert.ToDouble(processedArray[3]) : default,
                    High = !processedArray[4].Contains("N/D") ? Convert.ToDouble(processedArray[4]) : default,
                    Low = !processedArray[5].Contains("N/D") ? Convert.ToDouble(processedArray[5]) : default,
                    Close = !processedArray[6].Contains("N/D") ? Convert.ToDouble(processedArray[6]) : default,
                    Volume = !processedArray[7].Contains("N/D") ? Convert.ToDouble(processedArray[7]) : default,
                };
            }
        }
    }
}
