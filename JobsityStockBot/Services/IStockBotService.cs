using JobsityCommons.Models;

namespace JobsityStockBot.Services
{
    public interface IStockBotService
    {
        Stock GetStockByCode(string stockCode);
    }
}
