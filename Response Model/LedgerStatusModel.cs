namespace iMARSARLIMS.Response_Model
{
    public class LedgerStatusModel
    {
        public int? ParentCentreID { get; set; }
        public bool? IsLock { get; set; }
        public decimal? CcreditLimt { get; set; }
        public string? CentreType { get; set; }
        public int? LCentreId { get; set; }
        public string? CentreCode { get; set; }
        public string? CompanyName { get; set; }
        public string? CentreAdd { get; set; }
        public string? CentreMobile { get; set; }
        public DateTime? CreditPeridos { get; set; }
        public bool? Cactive { get; set; }
        public string? UnlockBy { get; set; }
        public string? LockDate { get; set; }
        public string? UnlockDate { get; set; }
        public string? InvoiceNo { get; set; }
        public string? Remarks { get; set; }
        public string? CreatedDate { get; set; }
        public decimal? InvoiceAmt { get; set; }
        public decimal? CreationPayment { get; set; }
        public decimal? ApprovedPayment { get; set; }
        public decimal? CurrentMPayment { get; set; }
        public decimal? UnPaid { get; set; }
        public decimal? RemainingPayment { get; set; }
        public decimal? CurrentBuss { get; set; }
        public decimal? AvailableBalance { get; set; }
        public decimal? TodayBussiness { get; set; }
        public decimal? YesterDayBussiness { get; set; }
    }
}
