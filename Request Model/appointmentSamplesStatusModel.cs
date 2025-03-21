using Microsoft.EntityFrameworkCore.Diagnostics;

namespace iMARSARLIMS.Request_Model
{
    public class appointmentSamplesStatusModel
    {
        public int testid { get; set; }
        public string barcodeno { get; set; }
        public string iscamplecollected  { get; set; }
        public int collectedBy { get; set; }
        
    }
}
