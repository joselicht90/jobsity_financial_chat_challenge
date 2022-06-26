namespace JobsityCommons.Models
{
    /// <summary>
    /// Stock class
    /// </summary>
    public class Stock
    {
        public string Symbol { get; set; } = "";
        public DateTime Date { get; set; } = default;
        public TimeSpan Time { get; set; } = default;
        public double Open { get; set; } = default;
        public double High { get; set; } = default;
        public double Low { get; set; } = default;
        public double Close { get; set; } = default;
        public double Volume { get; set; } = default;
    }
}
