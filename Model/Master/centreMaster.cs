using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iMARSARLIMS;

namespace iMARSARLIMS.Model.Master
{
    [Table(nameof(centreMaster))]
    public class centreMaster : Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required,MaxLength(50)]
        public string? centretype { get; set; }
        [Required,MaxLength(20)]
        public string centrecode { get; set; }
        [Required,MaxLength(50)]
        public string companyName { get; set; }
        [Required,MaxLength(10)]
        public string? mobileNo { get; set; }
        [MaxLength(15)]
        public string? landline { get; set; }
        public string? address { get; set; }

        public int pinCode { get; set; }
        public string? email { get; set; }
        [Required,MaxLength(100)]
        public string? ownerName { get; set; }
        public int proId { get; set; }
        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public int reportEmail { get; set; }
        public int parentCentreID { get; set; }
        public int processingLab { get; set; }
        public decimal creditLimt { get; set; }
        public int allowDueReport { get; set; }
        public int reportLock { get; set; }
        public int bookingLock { get; set; }
        public DateTime? unlockTime { get; set; }
        public int smsAllow { get; set; }
        public int emailAllow { get; set; }
        [Required, MaxLength(10)]
        public string paymentMode { get; set; }
        public int paymentModeId { get; set; }
        public string? reportHeader { get; set; }
        public string? reciptHeader { get; set; }
        public string? reciptFooter { get; set; }
        public int showISO { get; set; }
        public int showBackcover { get; set; }
        [MaxLength(100)]
        public string? reportBackImage { get; set; }
        public int? reporrtHeaderHeightY { get; set; }
        public int? patientYHeader { get; set; }
        public int? barcodeXPosition { get; set; }
        public int? barcodeYPosition { get; set; }
        public int? QRCodeXPosition { get; set; }
        public int? QRCodeYPosition { get; set; }
        public int? isQRheader { get; set; }
        public int? isBarcodeHeader { get; set; }
        public int? footerHeight { get; set; }
        public int? NABLxPosition { get; set; }
        public int? NABLyPosition { get; set; }
        public int? docSignYPosition { get; set; }
        public int? receiptHeaderY { get; set; }
        [MaxLength(20)]
        public string? PAN { get; set; }
        [MaxLength(16)]
        public string? adharNo { get; set; }
        [MaxLength(20)]
        public string? bankAccount { get; set; }
        [MaxLength(20)]
        public string? IFSCCode { get; set; }
        public int bankID { get; set; }
        public int salesExecutiveID { get; set; }
        public int? isDefault { get; set; }
        public int? isLab { get; set; }
        public int minBookingAmt { get; set; }
        public string? lockedBy { get; set; }
        public DateTime? LockDate { get; set; }
        public string? unlockBy { get; set; }
        public DateTime? unlockDate { get; set; }
        public int state { get; set; }
        [MaxLength(20)]
        public string? chequeNo { get; set; }
        [MaxLength(20)]
        public string? bankName { get; set; }
        public int chequeAmount { get; set; }
        public int creditPeridos { get; set; }
        public int showClientCode { get; set; }
        public int patientRate { get; set; }
        public int clientRate { get; set; }
        public int isLock { get; set; }
        public int isPrePrintedBarcode { get; set; }
        public List<centreCheckListMapping>? addCentreCheckListMapping { get; set; }


    }
}
