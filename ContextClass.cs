using iMARSARLIMS.Model.Account;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection.Emit;

namespace iMARSARLIMS
{
    public class ContextClass : DbContext
    {
        public DbSet<area_master> area_master {  get; set; }
        public DbSet<centreMaster> centreMaster {  get; set; }
        public DbSet<tnx_Booking> tnx_Booking { get; set; }
        public DbSet<tnx_BookingStatus> tnx_BookingStatus { get; set; }
        public DbSet<tnx_BookingItem> tnx_BookingItem { get; set; }
        public DbSet<tnx_InvestigationRemarks> tnx_InvestigationRemarks { get; set; }
        public DbSet<tnx_Observations> tnx_Observations { get; set; } 
        public DbSet<tnx_Observations_Log> tnx_Observations_Log { get; set; }
        public DbSet<tnx_Observations_Histo> tnx_Observations_Histo { get; set; }
        public DbSet<tnx_Observations_Micro_Flowcyto> tnx_Observations_Micro_Flowcyto { get; set; } 
        public DbSet<tnx_Observations_Micro_flowcyto_Log> tnx_Observations_Micro_flowcyto_Log { get; set; }         
        public DbSet<tnx_outhousedeatils> tnx_outhousedeatils { get; set; }
        public DbSet<tnx_ReceiptDetails> tnx_ReceiptDetails { get; set; }
        public DbSet<tnx_Sra> tnx_Sra { get; set; }
        public DbSet<tnx_Invoice_Payment> tnx_Invoice_Payment { get; set; }
        public DbSet<bank_master> bank_master { get; set; }
        public DbSet<centerTypeMaster> centerTypeMaster { get; set; }
        public DbSet<cityMaster> cityMaster { get; set; }
        public DbSet<containerColorMaster> containerColorMaster { get; set; }
        public DbSet<rateTypeMaster> rateTypeMaster { get; set; }
        public DbSet<designationMaster> designationMaster { get; set; }
        public DbSet<discountReasonMaster> discountReasonMaster { get; set; }
        public DbSet<districtMaster> districtMaster { get; set; }
        public DbSet<doctorApprovalMaster> doctorApprovalMaster { get; set; }
        public DbSet<degreeMaster> degreeMaster { get; set; }
        public DbSet<documentTypeMaster> documentTypeMaster { get; set; }
        public DbSet<documentMaster> documentMaster { get; set; }
        public DbSet<empCenterAccess> empCenterAccess { get; set; }
        public DbSet<empMaster> empMaster { get; set; }
        public DbSet<empDepartmentAccess> empDepartmentAccess { get; set; }
        public DbSet<empRoleAccess> empRoleAccess { get; set; }
        public DbSet<tnx_Allergy_ResultEntry> tnx_Allergy_ResultEntry { get; set; }
        public DbSet<Allergy_SubType_Master> Allergy_SubType_Master { get; set; }
        public DbSet<Allergy_TypeMaster> Allergy_TypeMaster { get; set; }
        public DbSet<barcode_series> barcode_series { get; set; }
        public DbSet<changeCentreLog> changeCentreLog { get; set; }
        public DbSet<centreCheckList> centreCheckList { get; set; }
        public DbSet<empLoginDetails> empLoginDetails { get; set; }
        public DbSet<discountTypeMaster> discountTypeMaster { get; set; }
        public DbSet<centreCheckListMapping> centreCheckListMapping { get; set; }
        public DbSet<centreLedgerRemarks> centreLedgerRemarks { get; set; }
        public DbSet<doctorApprovalDepartments> doctorApprovalDepartments { get; set; }
        public DbSet<idMaster> idMaster { get; set; }
        public DbSet<patientDemographicLog> patientDemographicLog { get; set; }
        public DbSet<logoDetails> logoDetails { get; set; }
        public DbSet<patientReportEmail> patientReportEmail { get; set; }
        public DbSet<sms> sms { get; set; }
        public DbSet<roleMaster> roleMaster { get; set; }
        public DbSet<observationComment> observationComment { get; set; }
        public DbSet<whatsapp> whatsapp { get; set; }
        public DbSet<itemDocumentMapping> itemDocumentMapping { get; set; }
        public DbSet<itemCommentMaster> itemCommentMaster { get; set; }
        public DbSet<itemRateMaster> itemRateMaster { get; set; }
        public DbSet<centerAccess> centerAccess { get; set; }
        public DbSet<centreWelcomeEmail> centreWelcomeEmail { get; set; }
        public DbSet<testInterpretation> testInterpretation { get; set; }
        public DbSet<testInterpretationLog> testInterpretationLog { get; set; }
        public DbSet<formulaMaster> formulaMaster { get; set; }
        public DbSet<tnx_BookingPatient> tnx_BookingPatient { get; set; }
        public DbSet<machineMaster> machineMaster { get; set; }
        public DbSet<organismAntibioticMaster> organismAntibioticMaster { get; set; }
        public DbSet<organismAntibioticTagMaster> organismAntibioticTagMaster { get; set; }
        public DbSet<helpMenuMaster> helpMenuMaster { get; set; }
        public DbSet<helpMenuMapping> helpMenuMapping { get; set; }
        public DbSet<zoneMaster> zoneMaster { get; set; }
        public DbSet<stateMaster> stateMaster { get; set; }
        public DbSet<labReportHeader> labReportHeader { get; set; }
        public DbSet<titleMaster> titleMaster { get; set; }
        public DbSet<doctorReferalMaster> doctorReferalMaster { get; set; }
        public DbSet<sampletype_master> sampletype_master { get; set; }
        public DbSet<itemMaster> itemMaster { get; set; }
        public DbSet<item_outsourcemaster> item_outsourcemaster { get; set; }
        public DbSet<outSourcelabmaster> outSourcelabmaster { get; set; }
        public DbSet<sampleRejectionReason> sampleRejectionReason { get; set; }
        public DbSet<menuMaster> menuMaster { get; set; }
        public DbSet<roleMenuAccess> roleMenuAccess { get; set; }
        public DbSet<rateTypeTagging> rateTypeTagging { get; set; }
        public DbSet<itemObservationMaster> itemObservationMaster { get; set; }
        public DbSet<ItemObservationMapping> ItemObservationMapping { get; set; }
        public DbSet<itemSampleTypeMapping> itemSampleTypeMapping { get; set; }
        public DbSet<machine_result> machine_result { get; set; }
        public DbSet<observationReferenceRanges> observationReferenceRanges { get; set; }
        public DbSet<SingleStringResponseModel> SingleStringResponseModel { get; set; }
        public DbSet<ResultEntryResponseModle> ResultEntryResponseModle { get; set; }
        public DbSet<tnx_Observations_Histo_Log> tnx_Observations_Histo_Log { get; set; }
        public DbSet<rateTypeWiseRateList> rateTypeWiseRateList { get; set; }
        public DbSet<machineObservationMapping> machineObservationMapping { get; set; }
        public DbSet<labDepartment> labDepartment { get; set; }
        public DbSet<centreInvoice> centreInvoice { get; set; }
        public DbSet<CentrePayment> CentrePayment { get; set; }


        public ContextClass(DbContextOptions<ContextClass> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<area_master>().HasKey(x => x.id);
            modelBuilder.Entity<area_master>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<centreMaster>().HasKey(x => x.id);
            modelBuilder.Entity<centreMaster>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<tnx_Booking>().HasKey(x => x.transactionId);
            modelBuilder.Entity<tnx_Booking>().Property(x => x.transactionId).ValueGeneratedOnAdd();

            modelBuilder.Entity<tnx_BookingStatus>().HasKey(x => x.id);
            modelBuilder.Entity<tnx_BookingStatus>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<tnx_BookingItem>().HasKey(x => x.id);
            modelBuilder.Entity<tnx_BookingItem>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<tnx_InvestigationRemarks>().HasKey(x => x.id);
            modelBuilder.Entity<tnx_InvestigationRemarks>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<tnx_Observations>().HasKey(x => x.id);
            modelBuilder.Entity<tnx_Observations>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<tnx_Observations_Log>().HasKey(x => x.id);
            modelBuilder.Entity<tnx_Observations_Log>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<tnx_Observations_Histo>().HasKey(x => x.histoObservationId);
            modelBuilder.Entity<tnx_Observations_Histo>().Property(x => x.histoObservationId).ValueGeneratedOnAdd();

            modelBuilder.Entity<tnx_Observations_Micro_Flowcyto>().HasKey(x => x.id);
            modelBuilder.Entity<tnx_Observations_Micro_Flowcyto>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<tnx_Observations_Micro_flowcyto_Log>().HasKey(x => x.testId);
            modelBuilder.Entity<tnx_Observations_Micro_flowcyto_Log>().Property(x => x.testId).ValueGeneratedOnAdd();

            modelBuilder.Entity<tnx_outhousedeatils>().HasKey(x => x.id);
            modelBuilder.Entity<tnx_outhousedeatils>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<tnx_ReceiptDetails>().HasKey(x => x.id);
            modelBuilder.Entity<tnx_ReceiptDetails>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<tnx_Sra>().HasKey(x => x.id);
            modelBuilder.Entity<tnx_Sra>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<tnx_Invoice_Payment>().HasKey(x => x.id);
            modelBuilder.Entity<tnx_Invoice_Payment>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<bank_master>().HasKey(x => x.id);
            modelBuilder.Entity<bank_master>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<centerTypeMaster>().HasKey(x => x.id);
            modelBuilder.Entity<centerTypeMaster>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<cityMaster>().HasKey(x => x.id);
            modelBuilder.Entity<cityMaster>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<containerColorMaster>().HasKey(x => x.id);
            modelBuilder.Entity<containerColorMaster>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<rateTypeMaster>().HasKey(x => x.id);
            modelBuilder.Entity<rateTypeMaster>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<designationMaster>().HasKey(x => x.id);
            modelBuilder.Entity<designationMaster>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<discountReasonMaster>().HasKey(x => x.id);
            modelBuilder.Entity<discountReasonMaster>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<districtMaster>().HasKey(x => x.id);
            modelBuilder.Entity<districtMaster>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<doctorApprovalMaster>().HasKey(x => x.id);
            modelBuilder.Entity<doctorApprovalMaster>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<degreeMaster>().HasKey(x => x.id);
            modelBuilder.Entity<degreeMaster>().Property(x => x.id).ValueGeneratedOnAdd();
            
            modelBuilder.Entity<documentTypeMaster>().HasKey(x => x.id);
            modelBuilder.Entity<documentTypeMaster>().Property(x => x.id).ValueGeneratedOnAdd();
           
            modelBuilder.Entity<documentMaster>().HasKey(x => x.id);
            modelBuilder.Entity<documentMaster>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<empCenterAccess>().HasKey(x => x.id);
            modelBuilder.Entity<empCenterAccess>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<empMaster>().HasKey(x => x.id);
            modelBuilder.Entity<empMaster>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<empDepartmentAccess>().HasKey(x => x.id);
            modelBuilder.Entity<empDepartmentAccess>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<empRoleAccess>().HasKey(x => x.id);
            modelBuilder.Entity<empRoleAccess>().Property(x => x.id).ValueGeneratedOnAdd();
            
            modelBuilder.Entity<tnx_Allergy_ResultEntry>().HasKey(x => x.id);
            modelBuilder.Entity<tnx_Allergy_ResultEntry>().Property(x => x.id).ValueGeneratedOnAdd();
          
            modelBuilder.Entity<Allergy_SubType_Master>().HasKey(x => x.id);
            modelBuilder.Entity<Allergy_SubType_Master>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Allergy_TypeMaster>().HasKey(x => x.id);
            modelBuilder.Entity<Allergy_TypeMaster>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<barcode_series>().HasKey(x => x.id);
            modelBuilder.Entity<barcode_series>().Property(x => x.id).ValueGeneratedOnAdd();
            
            modelBuilder.Entity<changeCentreLog>().HasKey(x => x.id);
            modelBuilder.Entity<changeCentreLog>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<centreCheckList>().HasKey(x => x.id);
            modelBuilder.Entity<centreCheckList>().Property(x => x.id).ValueGeneratedOnAdd();
            
            modelBuilder.Entity<empLoginDetails>().HasKey(x => x.id);
            modelBuilder.Entity<empLoginDetails>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<discountTypeMaster>().HasKey(x => x.id);
            modelBuilder.Entity<discountTypeMaster>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<centreCheckListMapping>().HasKey(x => x.id);
            modelBuilder.Entity<centreCheckListMapping>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<centreLedgerRemarks>().HasKey(x => x.id);
            modelBuilder.Entity<centreLedgerRemarks>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<doctorApprovalDepartments>().HasKey(x => x.id);
            modelBuilder.Entity<doctorApprovalDepartments>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<idMaster>().HasKey(x => x.id);
            modelBuilder.Entity<idMaster>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<patientDemographicLog>().HasKey(x => x.id);
            modelBuilder.Entity<patientDemographicLog>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<logoDetails>().HasKey(x => x.id);
            modelBuilder.Entity<logoDetails>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<patientReportEmail>().HasKey(x => x.id);
            modelBuilder.Entity<patientReportEmail>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<sms>().HasKey(x => x.id);
            modelBuilder.Entity<sms>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<roleMaster>().HasKey(x => x.id);
            modelBuilder.Entity<roleMaster>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<observationComment>().HasKey(x => x.id);
            modelBuilder.Entity<observationComment>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<whatsapp>().HasKey(x => x.id);
            modelBuilder.Entity<whatsapp>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<itemDocumentMapping>().HasKey(x => x.id);
            modelBuilder.Entity<itemDocumentMapping>().Property(x => x.id).ValueGeneratedOnAdd();
            
            modelBuilder.Entity<itemCommentMaster>().HasKey(x => x.id);
            modelBuilder.Entity<itemCommentMaster>().Property(x => x.id).ValueGeneratedOnAdd();
            
            modelBuilder.Entity<itemRateMaster>().HasKey(x => x.id);
            modelBuilder.Entity<itemRateMaster>().Property(x => x.id).ValueGeneratedOnAdd();
            
            modelBuilder.Entity<centerAccess>().HasKey(x => x.id);
            modelBuilder.Entity<centerAccess>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<centreWelcomeEmail>().HasKey(x => x.id);
            modelBuilder.Entity<centreWelcomeEmail>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<testInterpretation>().HasKey(x => x.id);
            modelBuilder.Entity<testInterpretation>().Property(x => x.id).ValueGeneratedOnAdd();
            
            modelBuilder.Entity<testInterpretationLog>().HasKey(x => x.id);
            modelBuilder.Entity<testInterpretationLog>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<formulaMaster>().HasKey(x => x.id);
            modelBuilder.Entity<formulaMaster>().Property(x => x.id).ValueGeneratedOnAdd();
         
            modelBuilder.Entity<tnx_BookingPatient>().HasKey(x => x.patientId);
            modelBuilder.Entity<tnx_BookingPatient>().Property(x => x.patientId).ValueGeneratedOnAdd();
            
            modelBuilder.Entity<machineMaster>().HasKey(x => x.id);
            modelBuilder.Entity<machineMaster>().Property(x => x.id).ValueGeneratedOnAdd();
            
            modelBuilder.Entity<organismAntibioticMaster>().HasKey(x => x.id);
            modelBuilder.Entity<organismAntibioticMaster>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<organismAntibioticTagMaster>().HasKey(x => x.id);
            modelBuilder.Entity<organismAntibioticTagMaster>().Property(x => x.id).ValueGeneratedOnAdd();
            
            modelBuilder.Entity<helpMenuMaster>().HasKey(x => x.id);
            modelBuilder.Entity<helpMenuMaster>().Property(x => x.id).ValueGeneratedOnAdd();
            
            modelBuilder.Entity<helpMenuMapping>().HasKey(x => x.id);
            modelBuilder.Entity<helpMenuMapping>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<zoneMaster>().HasKey(x=>x.id);
            modelBuilder.Entity<zoneMaster>().Property(x => x.id).ValueGeneratedOnAdd();
        
            modelBuilder.Entity<stateMaster>().HasKey(x => x.id);
            modelBuilder.Entity<stateMaster>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<labReportHeader>().HasKey(x => x.id);
            modelBuilder.Entity<labReportHeader>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<titleMaster>().HasKey(x => x.id);
            modelBuilder.Entity<titleMaster>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<doctorReferalMaster>().HasKey(x => x.doctorId);
            modelBuilder.Entity<doctorReferalMaster>().Property(x => x.doctorId).ValueGeneratedOnAdd();

            modelBuilder.Entity<sampletype_master>().HasKey(x => x.id);
            modelBuilder.Entity<sampletype_master>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<itemMaster>().HasKey(x => x.id);
            modelBuilder.Entity<itemMaster>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<item_outsourcemaster>().HasKey(x => x.id);
            modelBuilder.Entity<item_outsourcemaster>().Property(x => x.id).ValueGeneratedOnAdd();
            
            modelBuilder.Entity<outSourcelabmaster>().HasKey(x => x.id);
            modelBuilder.Entity<outSourcelabmaster>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<sampleRejectionReason>().HasKey(x => x.id);
            modelBuilder.Entity<sampleRejectionReason>().Property(x => x.id).ValueGeneratedOnAdd();
            
            modelBuilder.Entity<menuMaster>().HasKey(x => x.id);
            modelBuilder.Entity<menuMaster>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<roleMenuAccess>().HasKey(x => x.id);
            modelBuilder.Entity<roleMenuAccess>().Property(x => x.id).ValueGeneratedOnAdd();
            
            modelBuilder.Entity<rateTypeTagging>().HasKey(x => x.id);
            modelBuilder.Entity<rateTypeTagging>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<itemObservationMaster>().HasKey(x => x.id);
            modelBuilder.Entity<itemObservationMaster>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<ItemObservationMapping>().HasKey(x => x.id);
            modelBuilder.Entity<ItemObservationMapping>().Property(x => x.id).ValueGeneratedOnAdd();
            
            modelBuilder.Entity<itemSampleTypeMapping>().HasKey(x => x.id);
            modelBuilder.Entity<itemSampleTypeMapping>().Property(x => x.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<machine_result>().HasKey(x => x.id);
            modelBuilder.Entity<machine_result>().Property(x => x.id).ValueGeneratedOnAdd();
            
            modelBuilder.Entity<observationReferenceRanges>().HasKey(x => x.id);
            modelBuilder.Entity<observationReferenceRanges>().Property(x => x.id).ValueGeneratedOnAdd();
            
            modelBuilder.Entity<tnx_Observations_Histo_Log>().HasKey(x => x.histoObservationId);
            modelBuilder.Entity<tnx_Observations_Histo_Log>().Property(x => x.histoObservationId).ValueGeneratedOnAdd();
  
            modelBuilder.Entity<rateTypeWiseRateList>().HasKey(x => x.id);
            modelBuilder.Entity<rateTypeWiseRateList>().Property(x => x.id).ValueGeneratedOnAdd();
            
            modelBuilder.Entity<machineObservationMapping>().HasKey(x => x.id);
            modelBuilder.Entity<machineObservationMapping>().Property(x => x.id).ValueGeneratedOnAdd();
            
            modelBuilder.Entity<labDepartment>().HasKey(x => x.id);
            modelBuilder.Entity<labDepartment>().Property(x => x.id).ValueGeneratedOnAdd();
            
            modelBuilder.Entity<centreInvoice>().HasKey(x => x.id);
            modelBuilder.Entity<centreInvoice>().Property(x => x.id).ValueGeneratedOnAdd();
         
            modelBuilder.Entity<CentrePayment>().HasKey(x => x.id);
            modelBuilder.Entity<CentrePayment>().Property(x => x.id).ValueGeneratedOnAdd();



            modelBuilder.Entity<SingleStringResponseModel>().HasNoKey();
            modelBuilder.Entity<ResultEntryResponseModle>().HasNoKey();
            //modelBuilder.Entity<SampleProcessingResponseModel>().HasNoKey();
            //modelBuilder.Entity<SampleProcessingRequestModel>().HasNoKey();
            //modelBuilder.Entity<LoginRequestModel>().HasNoKey();

        }
    }

}
