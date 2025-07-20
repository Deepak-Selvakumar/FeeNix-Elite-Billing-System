
namespace FmsEliteBilling.Model
{
    public class LogListVM
    {
        public int id { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Level { get; set; }
        public string? Exception { get; set; }
        public string? RenderedMessage { get; set; }
        public string? Properties { get; set; }
    }
}