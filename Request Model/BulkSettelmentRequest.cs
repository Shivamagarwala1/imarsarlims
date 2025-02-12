namespace iMARSARLIMS.Request_Model
{
    public class BulkSettelmentRequest
    {
        public int Centreid { get; set; }
        public string Name { get; set; }
       public string Workorderid { get; set; }
        public double discount { get; set; }
        
        public double dueAmt { get; set; }
        public double SettelmentAmt { get; set; }

    }
}
