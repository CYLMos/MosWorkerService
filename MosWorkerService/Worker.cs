using MosWorkerService.Models;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using CsvHelper;
using System.Globalization;

namespace MosWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly AppSettings _appSettings;
        private Timer? _timer = null;

        public Worker(ILogger<Worker> logger, AppSettings appSettings)
        {
            _logger = logger;
            _appSettings = appSettings;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            DateTime.TryParse(_appSettings.FetchTime, out var fetchTime);
            var nowTime = DateTime.Now;

            var gapTimespan = fetchTime.Subtract(nowTime);
            var waitTime = gapTimespan.TotalSeconds > 0 ? TimeSpan.FromSeconds(gapTimespan.TotalSeconds) : TimeSpan.Zero;

            _timer = new Timer(
                FetchProcess, null, TimeSpan.Zero, TimeSpan.FromSeconds(86400));

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // No need to implement here
        }

        private void FetchProcess(object state)
        {
            _ = FetchStockData();
        }

        private async Task FetchStockData()
        {
            var dateString = DateTime.Now.ToString("yyyyMMdd");
            var csvModelList = new List<CsvModel>();
            var httpClient = new HttpClient();

            foreach (var stockNumber in _appSettings.StockNumber)
            {
                var url = UrlGenerator(dateString, stockNumber);

                try
                {
                    var response = await httpClient.GetAsync(url);
                    var content = await response.Content.ReadAsStringAsync();

                    var fetchData = JsonConvert.DeserializeObject<FetchApiModel>(content);
                    var csvData = fetchData?.TransferToCsvModel();
                    csvModelList.Add(csvData);
                }
                catch (Exception ex)
                {
                    _logger.LogError(null, ex);
                }
            }

            httpClient.Dispose();

            WriteCsv(csvModelList);
        }

        private string UrlGenerator(string date, string stockNumber)
        {
            var url = _appSettings.FetchUrl;
            var regex = new Regex(Regex.Escape("%s"));

            url = regex.Replace(url, date, 1);
            url = regex.Replace(url, stockNumber, 1);

            return url;
        }

        private void WriteCsv(List<CsvModel> csvModelList)
        {
            bool exists = Directory.Exists(_appSettings.CsvPath);

            if (!exists)
                Directory.CreateDirectory(_appSettings.CsvPath);

            using (var writer = new StreamWriter($"{_appSettings.CsvPath}\\{DateTime.Now.ToString("yyyyMMdd")}.csv"))
            using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csvWriter.WriteRecords(csvModelList);
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            _timer?.Dispose();
        }
    }
}