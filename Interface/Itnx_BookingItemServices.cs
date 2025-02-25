using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using iMARSARLIMS.Services;
using Microsoft.AspNetCore.Mvc;

namespace iMARSARLIMS.Interface
{
    public interface Itnx_BookingItemServices
    {
        Task<ServiceStatusResponseModel> GetSampleProcessingData(SampleProcessingRequestModel sampleProcessingRequestModel);
        Task<ServiceStatusResponseModel> UpdateSampleStatus(List<SampleProcessingResponseModel> sampleProcessingResponseModel);
        Task<ActionResult<ServiceStatusResponseModel>> UpdateSampletransfer(List<tnx_Sra> SRA);
        Task<ActionResult<ServiceStatusResponseModel>> UpdateBatchReceive(List<BatchStatusRecieveRequestModel> batchStatusRecieveRequestModel);
        Task<List<ResultEntryResponseModle>> GetTestObservations(ResultEntryRequestModle resultEntryRequestModle);
        Task<ServiceStatusResponseModel> SaveTestObservations(List<ResultSaveRequestModle> resultSaveRequestModle);
        Task<ServiceStatusResponseModel> GetitemDetailRate(int ratetype);
        Task<ServiceStatusResponseModel> GetitemDetail(int ratetype, int itemId);
        Task<ServiceStatusResponseModel> GetPackageTestDetail(int itemId);
        Task<ActionResult<ServiceStatusResponseModel>> SaveHistoResult(HistoResultSaveRequestModle histoResultSaveRequestModle);
        Task<ServiceStatusResponseModel> GetOldPatient(string searchValue);
        Task<ServiceStatusResponseModel> GetPatientEditInfo(string searchValue);
        Task<ServiceStatusResponseModel> GetPatientEditTest(string searchValue);
        Task<ServiceStatusResponseModel> UpdatePatientinfo(UpdatePatientInfoRequestModel patientInfo);
        Task<ServiceStatusResponseModel> UpdatePatientTest(List<tnx_BookingItem> Updatetestdetail);
    }
}
