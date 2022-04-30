namespace MosWorkerService.Models
{
    public class CsvModel
    {
        public string StockNumber { get; set; }
        public string Date { get; set; }
        public string TradingVolume { get; set; }
        public string Transaction { get; set; }
        public string OpeningPrice { get; set; }
        public string HightestPrice { get; set; }
        public string LowestPrice { get; set; }
        public string ClosingPrice { get; set; }
        public string Change { get; set; }
        public string Turnover { get; set; }
    }
}
