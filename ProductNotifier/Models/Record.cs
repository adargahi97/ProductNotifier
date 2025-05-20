using System.ComponentModel.DataAnnotations.Schema;

namespace ProductNotifier.Models
{
    [Table("Records")]
    public class Record
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Artist { get; set; }
        public double Price { get; set; }
        public string? Source { get; set; }
        public DateTime? DateAdded { get; set; }
        public string Link { get; set; }
    }
}
