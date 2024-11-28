namespace iMARSARLIMS.Response_Model
{
    public class PackageItemDetailResponseModle
    {
       public string testcode {  get; set; }
       public int itemId { get; set; }
        public int deptId { get; set; }
        public string departmentName { get; set; }
        public string investigationName { get; set; }
        public byte itemType { get; set; }
        public double mrp { get; set; }
        public double rate { get; set; }
        public double discount { get; set; }
        public double netAmount { get; set; }
    }
}
