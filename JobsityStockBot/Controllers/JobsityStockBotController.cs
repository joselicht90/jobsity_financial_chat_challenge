using JobsityCommons.Models;
using JobsityStockBot.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobsityStockBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsityStockBotController : Controller
    {
        private IStockBotService StockBotService;

        public JobsityStockBotController(IStockBotService stockInfoDomain)
        {
            StockBotService = stockInfoDomain;
        }

        /// <summary>
        /// Get stock by code
        /// </summary>
        /// <param name="stockCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetStock")]
        public ActionResult<Stock> GetStock(string stockCode)
        {
            try
            {
                var result = StockBotService.GetStockByCode(stockCode);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
