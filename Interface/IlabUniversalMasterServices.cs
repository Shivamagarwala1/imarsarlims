using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface IlabUniversalMasterServices
    {
        Task<ServiceStatusResponseModel> SaveUpdateOutsourceLab(outSourcelabmaster OutsourceLab);
        Task<ServiceStatusResponseModel> UpdateOutsourceLabStatus(int id, byte status, int userId);
        Task<ServiceStatusResponseModel> UpdateSampleTypeStatus(int id, byte status, int userId);
        Task<ServiceStatusResponseModel> SaveUpdateSampletype(sampletype_master SampleType);
        Task<ServiceStatusResponseModel> UpdateTestMethodStatus(int id, byte status, int userId);
        Task<ServiceStatusResponseModel> SaveUpdateTestMethod(TestMethodMaster testMethod);
        Task<ServiceStatusResponseModel> UpdateSampleReasonStatus(int id, byte status, int userId);
        Task<ServiceStatusResponseModel> SaveUpdateSampleRerunReason(SampleRerunReason SampleReason);
        Task<ServiceStatusResponseModel> UpdateFooterTextStatus(int id, byte status, int userId);
        Task<ServiceStatusResponseModel> SaveUpdateFooterText(labReportFooterText FooterText);
        Task<ServiceStatusResponseModel> GetFooterText();
        Task<ServiceStatusResponseModel> UpdateSampleRemarkStatus(int id, byte status, int userId);
        Task<ServiceStatusResponseModel> SaveUpdatesampleRemark(SampleremarkMaster SampleRemark);

    }
}
