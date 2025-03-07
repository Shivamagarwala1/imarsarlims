using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Response_Model
{
    public class TatReportData
    {
    public string? BookingDate { get; set; }
    public string? PatientName { get; set; }
    public int itemId { get; set; }
    public string? TestName { get; set; }
    public string? RefDoctor { get; set; }
    public string? Department { get; set; }
    public int DeptId { get; set; }
    public int centreId { get; set; }
    public string? CentreCode { get; set; }
    public string? centreName { get; set; }
    public string? WorkorderId { get; set; }
    public string? SampleCollectionDate { get; set; }
    public string? SampleReceivedDate { get; set; }
    public string? ResultDate { get; set; }
    public string? ApproveDate { get; set; }
    public int BTOS { get; set; }
    public int STOD { get; set; }
    public int DTOR { get; set; }
    public int RTOA { get; set; }
    public int BTOA { get; set; }
    public int DeliveryTime { get; set; }
    public string? Status { get; set; }
    }
}
