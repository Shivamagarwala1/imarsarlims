namespace iMARSARLIMS.Request_Model
 {
    public class UpdatePatientInfoRequestModel
     {
       public int patientId {get; set;}
       public string workOrderId {get; set;}
       public int transactionId {get; set;}
       public int title_id {get; set;}
       public string name {get; set;}
       public int ageDay {get; set;}
       public int ageMonth {get; set;}
       public short ageYear {get; set;}
       public DateTime dob {get; set;} 
       public string gender {get; set;}
       public string emailId {get; set;}
       public int refID1 {get; set;}
       public int refID2 {get; set;}
       public string address {get; set;}
       public int pinCode {get; set;}
       public int OtherLabReferID  {get; set;}
       public string otherLabRefer  {get; set;}
       public string uploadDocument  { get; set; }
        public string mobileno {get; set;}
    }
}
