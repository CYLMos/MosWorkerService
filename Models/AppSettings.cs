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
    }
}
