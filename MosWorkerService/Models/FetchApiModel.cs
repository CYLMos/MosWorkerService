namespace MosWorkerService.Models
{
    public class FetchApiModel
    {
        public string state { get; set; }
        public string date { get; set; }
        public string title { get; set; }
        public List<string> fields { get; set; }
        public List<List<string>> data { get; set; }
        public List<string> notes { get; set; }

        public CsvModel TransferToCsvModel()
        {
            var month = date?.Substring(4, 2);
            var day = date?.Substring(6, 2);

            var todayData = data.FirstOrDefault(subData => subData[0].Contains($"{month}/{day}"));

            var csvModel = new CsvModel()
            {
                Date = todayData[0],
                TradingVolume = todayData[1],
                Transaction = todayData[2],
                OpeningPrice = todayData[3],
                HightestPrice = todayData[4],
                LowestPrice = todayData[5],
                ClosingPrice = todayData[6],
                Change = todayData[7],
                Turnover = todayData[8]
            };

            return csvModel;
        }
    }
}
