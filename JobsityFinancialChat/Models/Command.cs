using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Josbity.Models
{
    public class Command
    {
        public int Id { get; set; }
        [Required]
        public string CommandName { get; set; } = string.Empty;
        [Required]
        public string CommandDescription { get; set; } = string.Empty;
        [Required]
        public string StockBotEndpoint { get; set; } = string.Empty;

    }
}
