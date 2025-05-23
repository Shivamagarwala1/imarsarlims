﻿namespace iMARSARLIMS.Request_Model
{
    public class MicroResultSaveRequestModel
    {
        public int id { get; set; }
        public int testId { get; set; }
        public int labTestId { get; set; }
        public int transactionId { get; set; }
        public string? observationName { get; set; }
        public string? result { get; set; }
        public short machineID { get; set; }
        public string? flag { get; set; }
        public byte? isBold { get; set; }
        public byte? reportType { get; set; }
        
        public string? colonyCount { get; set; }
        public string? positivity { get; set; }
        public string? intensity { get; set; }
        public byte? reportStatus { get; set; }
        public int? approvedBy { get; set; }
        public string? approvedName { get; set; }
        public string? comments { get; set; }
        public int isApproved { get; set; }
        public string createdBy { get; set; }
        public int createdById { get; set; }
        public int? appcovaldoctorId { get; set; }
        public List<organismdata>? selectedorganism {  get; set; }
    }

    public class antibioticData
    {
        public short antibiticId { get; set; }
        public string? antibitiName { get; set; }
        public string? interpretation { get; set; }
        public string? mic { get; set; }
    }
    public class organismdata
    {
        public short organismId { get; set; }
        public string? organismName { get; set; }
        public List<antibioticData>? selectedAntibiotic { get; set; }
    }
}
