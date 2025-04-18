﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Response_Model
{
    public class SampleProcessingResponseModel
    {
        
        public int centreId { get; set; }
        public string? centreName { get; set; }
        public int deptId { get; set; }
        public string? departmentName { get; set; }
        public int patientId { get; set; }
        public string? workOrderId { get; set; }
        public string? PatientName { get; set; }
        public string? Age { get; set; }    
        public int itemId { get; set; }
        public string? investigationName { get; set; }
        public string? barcodeNo { get; set; }
        public int sampleTypeId { get; set; }
        public string? sampleTypeName { get; set; }
        public string? isSampleCollected { get; set; }
        public string? Comment { get; set; }
        public int transactionId { get; set; }
        public DateTime createdDateTime { get; set; }
        public string? RejectionReason { get; set; }
        public int? empId { get; set; }
        public int Urgent { get; set; }
        public string? rowcolor { get;set; }
        public int? isRemoveItem { get; set; }
        [NotMapped]
        public object? sampletypedata { get; set; }
        public string? containercolor { get; set; }
        public int isremark { get; set; }
        public string? bookingdate { get; set; }
        public string? samplecollectiondate { get; set; }
        public string? SampleRecievedDate { get; set; }
        public int resultdone { get; set; }
        public int? approved { get; set; }
        public int? Hold { get; set; }
        public int ReportPrinted { get; set; }

    }
}
