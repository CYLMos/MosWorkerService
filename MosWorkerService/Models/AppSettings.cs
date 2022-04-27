using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosWorkerService.Models
{
    public class AppSettings
    {
        public List<string> StockNumber { get; set; } = new List<string>();
        public string FetchUrl { get; set; } = string.Empty;
        public string FetchTime { get; set; } = "16:00:00";
        public string CsvPath { get; set; } = AppContext.BaseDirectory;
    }
}
