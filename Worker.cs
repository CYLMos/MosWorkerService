using MosWorkerService.Models;
using System.Text.RegularExpressions;

namespace MosWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly AppSettings _appSettings;

        public Worker(ILogger<Worker> logger, AppSettings appSettings)
        {
            _logger = logger;
            _appSettings = appSettings;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await FetchStockData();
                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task FetchStockData()
        {
            var dateString = DateTime.Now.ToString("yyyyMMdd");

            foreach (var stockNumber in _appSettings.StockNumber)
            {
                var url = UrlGenerator(dateString, stockNumber);

                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(url);
                    var content = await response.Content.ReadAsStringAsync();
                }
            }
        }

        private string UrlGenerator(string date, string stockNumber)
        {
            var url = _appSettings.FetchUrl;
            var regex = new Regex(Regex.Escape("%s"));

            url = regex.Replace(url, date, 1);
            url = regex.Replace(url, stockNumber, 1);

            return url;
        }
    }
}