namespace Front.Models
{
    // objeto ticker tanto para ALPACA como para Yahoo Finance
    public class Ticker
    {
        public DateTime Datetime { get; set; }
        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Dividends { get; set; }
        public decimal StockSplits { get; set; }
        public long Volume { get; set; }

       //si predecimos
        public int Signal { get; set; }
    }
}
