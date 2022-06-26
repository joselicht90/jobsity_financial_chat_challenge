namespace JobsityCommons.Models
{
    /// <summary>
    /// Bot Response entity
    /// </summary>
    public class BotResponse
    {
        public bool IsSuccessful { get; set; }
        public bool Detected { get; set; }
        public string ErrorMessage { get; set; }
        public string Symbol { get; set; }
        public string Close { get; set; }
    }
}
