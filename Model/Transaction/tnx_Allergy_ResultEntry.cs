using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Transaction
{
    [Table(nameof(tnx_Allergy_ResultEntry))]
    public class tnx_Allergy_ResultEntry: Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int test_id { get; set; }
        public int bookingItemId { get; set; }
        public int transactionId { get; set; }
        public int allergyType { get; set; }
        public string allergyTypeName { get; set; }
        public int allergySubType { get; set; }
        public string allergySubTypeName { get; set; }
        public string reading { get; set; }
        public string displayReading { get; set; }
        public string firstRange { get; set; }
        public string secondRange { get; set; }
        public string thirdRange { get; set; }
        public double min { get; set; }
        public double max { get; set; }

    }
}
