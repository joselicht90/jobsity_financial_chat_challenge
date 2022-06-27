using Josbity.Models;
using JobsityCommons.Models;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using JobsityFinancialChat.Data;
using Microsoft.EntityFrameworkCore;

namespace JobsityFinancialChat.Hubs
{
    public class JosbityChatHub : Hub
    {


        public readonly JobsityContext _context;
        private IConfiguration Configuration { get; }
        private HttpClient Client { get; }

        public JosbityChatHub(IConfiguration configuration, HttpClient client, JobsityContext context)
        {
            Client = client;
            Configuration = configuration;
            _context = context;
        }

        /// <summary>
        /// On message sended
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(Message message)
        {
            await Clients.All.SendAsync("receiveMessage", message);
            var botResponse = await DetectBotCommand(message.Text);
            if (botResponse.Detected)
            {
                if (botResponse.IsSuccessful)
                    await Clients.All.SendAsync("receiveMessage", StockBotMessage($"{botResponse.Symbol} quote is {botResponse.Close} per share"));
                else
                    await Clients.All.SendAsync("receiveMessage", StockBotMessage($"Something went wrong. {botResponse.ErrorMessage}"));
            }

        }

        private async Task<(Command command, string commandValue)?> GetCommand(string input)
        {
            int posFrom = input.IndexOf('/');
            if (posFrom == 0) //if found char
            {
                int posTo = input.IndexOf('=', posFrom + 1);
                if (posTo != -1) //if found char
                {
                    string command = input.Substring(posFrom + 1, posTo - posFrom - 1);
                    string value = input.Substring(posTo + 1, input.Length - posTo - 1);

                    if (string.IsNullOrEmpty(value))
                        throw new Exception("You must write a value to the command");

                    var commanInDb = await _context.Commands.Where(x => x.CommandName.ToLower() == command.ToLower()).SingleOrDefaultAsync();
                    if (commanInDb == null)
                    {
                        throw new Exception("The command issued is not available.");
                    }

                    return (command: commanInDb, commandValue: value);
                }
                throw new Exception("You must write a value to the command");
            }

            return null;
        }

        /// <summary>
        /// Detects if the bot command has been issued
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task<BotResponse> DetectBotCommand(string message)
        {
            try
            {
                string _apiUrl = Environment.GetEnvironmentVariable("BOT_API_URL") ?? Configuration.GetValue<string>("BotApiUrl");

                var _getCommand = await GetCommand(message);

                if (_getCommand != null)
                {
                    Command _command = _getCommand.Value.command;
                    string _value = _getCommand.Value.commandValue;
                    using (HttpResponseMessage response = Client.GetAsync($"{_apiUrl}/api/JobsityStockBot/{_command.StockBotEndpoint}{_value}").Result)
                    using (HttpContent content = response.Content)
                    {
                        string serviceResponse = content.ReadAsStringAsync().Result;
                        if (response.StatusCode != System.Net.HttpStatusCode.OK)
                            return new BotResponse { Detected = true, IsSuccessful = false, ErrorMessage = response.StatusCode.ToString() };

                        var stock = JsonConvert.DeserializeObject<Stock>(serviceResponse);

                        if (stock == null)
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
