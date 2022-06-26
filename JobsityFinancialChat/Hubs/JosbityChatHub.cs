using Josbity.Models;
using JobsityCommons.Models;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace JobsityFinancialChat.Hubs
{
    public class JosbityChatHub : Hub
    {
        private IConfiguration Configuration { get; }
        private HttpClient Client { get; }

        public JosbityChatHub(IConfiguration configuration, HttpClient client)
        {
            Client = client;
            Configuration = configuration;
        }

        /// <summary>
        /// On message sended
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(Message message)
        {
            await Clients.All.SendAsync("receiveMessage", message);
            var botResponse = DetectBotCommand(message.Text);
            if (botResponse.Detected)
            {
                if (botResponse.IsSuccessful)
                    await Clients.All.SendAsync("receiveMessage", StockBotMessage($"{botResponse.Symbol} quote is {botResponse.Close} per share"));
                else
                    await Clients.All.SendAsync("receiveMessage", StockBotMessage($"Something went wrong. {botResponse.ErrorMessage}"));
            }
                
        }

        /// <summary>
        /// Detects if the bot command has been issued
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private BotResponse DetectBotCommand(string message)
        {
            try
            {
                string _apiUrl = Environment.GetEnvironmentVariable("BOT_API_URL") ?? Configuration.GetValue<string>("BotApiUrl");

                if (message.ToLower().Contains("/stock="))
                {
                    string code = message.Replace("/stock=", "");
                    using (HttpResponseMessage response = Client.GetAsync($"{_apiUrl}/api/JobsityStockBot/GetStock?stockCode={code}").Result)
                    using (HttpContent content = response.Content)
                    {
                        string serviceResponse = content.ReadAsStringAsync().Result;
                        if (response.StatusCode != System.Net.HttpStatusCode.OK)
                            return new BotResponse { Detected = true, IsSuccessful = false, ErrorMessage = response.StatusCode.ToString() };

                        var stock = JsonConvert.DeserializeObject<Stock>(serviceResponse);

                        if(stock == null)
                        {
                            return new BotResponse { Detected = false };
                        }

                        return new BotResponse { Detected = true, IsSuccessful = true, Symbol = stock.Symbol, Close = stock.Close.ToString() };
                    }
                }
                return new BotResponse { Detected = false };
            }
            catch (Exception ex)
            {
                return new BotResponse { Detected = true, IsSuccessful = false, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// Create a Bot message
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        internal Message StockBotMessage(string text)
        {
            return new Message
            {
                UserName = "StockBot",
                Text = text,
                When = DateTime.Now
            };
        }
    }
}
