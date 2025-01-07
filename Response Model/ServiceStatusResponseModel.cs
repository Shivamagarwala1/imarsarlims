using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Response_Model
{
    public class ServiceStatusResponseModel
    {
        public bool Success { get; set; }
        [NotMapped]
        public object? Data { get; set; }
        public string? Message { get; set; }
        public string? Token { get; set; }
        public int? Count { get; set; }
    }
}
