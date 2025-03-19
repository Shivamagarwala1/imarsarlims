using Org.BouncyCastle.Bcpg;

namespace iMARSARLIMS.Request_Model
{
    public class testRefundModel
    {
        public List<int> testIds {  get; set; }
        public int refundBy { get; set; }
        public string refundReason { get; set; }
    }
}
