using System.ComponentModel.DataAnnotations;

namespace iMARSARLIMS.Request_Model
{
    public class CentrePaymentRequestModel
    {
        public int id { get; set; }
        public int centreId { get; set; }
        public DateTime paymentDate { get; set; }
        public string? paymentMode { get; set; }
        public float? advancePaymentAmt { get; set; }
        public string? bank { get; set; }
        public string? tnxNo { get; set; }
        public DateTime tnxDate { get; set; }
        public string? remarks { get; set; }
        public string? rejectRemarks { get; set; }
        public short approved { get; set; }
        public int? createdBy { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? updateByID { get; set; }
        public int? apprvoedByID { get; set; }
        public byte? paymentType { get; set; }
        [MaxLength(300)]
        public string? fileName { get; set; }

    }
}
