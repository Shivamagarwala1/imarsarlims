namespace iMARSARLIMS.Request_Model
{

    public class UpdatePatientTestRequestModel
    {
        public int id { get; set; }
        public int patientId { get; set; }
        public string workOrderId { get; set; }
        public int transactionId { get; set; }
        public int title_id { get; set; }
        public string name { get; set; }
        public int ageDay { get; set; }
        public int ageMonth { get; set; }
        public int ageYear { get; set; }
        public DateTime dob { get; set; }
        public string gender { get; set; }
        public string emailId { get; set; }
        public int refID1 { get; set; }
        public int refID2 { get; set; }
        public string address { get; set; }
        public int pinCode { get; set; }
        public int OtherLabReferID { get; set; }
        public string otherLabRefer { get; set; }
        public string uploadDocument { get; set; }
        public int itemId { get; set; }
        public string investigationName  { get; set; }
        public double mrp { get; set; }
        public double discount { get; set; }
        public double netAmount { get; set; }
        public int itemType { get; set; }
        public DateTime deliveryDate { get; set; }
        public byte isUrgent { get; set; }

    }
}
