using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Account
{
    [Table(nameof(razorPayOrderRequest))]
    public class razorPayOrderRequest
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public decimal Amount { get; set; }
        [MaxLength(50)]
        public string Orderid { get; set; }
        [MaxLength(50)]
        public string Receipt { get; set; }
        [MaxLength(15)]
        public string workorderId { get; set; }
        public int centreID { get; set; }
        [MaxLength(50)]
        public string payType { get; set; }
        public DateTime Requestdate { get; set; }
        public int status { get; set; }
        public string paymentId { get; set; }

    }
}
