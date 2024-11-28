using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;

namespace iMARSARLIMS.Interface
{
    public interface Itnx_BookingItemServices
    {
        Task<List<SampleProcessingResponseModel>> GetSampleProcessingData(SampleProcessingRequestModel sampleProcessingRequestModel);
        Task<ServiceStatusResponseModel> UpdateSampleStatus(List<SampleProcessingResponseModel> sampleProcessingResponseModel);
        Task<ActionResult<ServiceStatusResponseModel>> UpdateSampletransfer(List<tnx_Sra> SRA);
        Task<ActionResult<ServiceStatusResponseModel>> UpdateBatchReceive(List<BatchStatusRecieveRequestModel> batchStatusRecieveRequestModel);
        Task<List<ResultEntryResponseModle>> GetTestObservations(ResultEntryRequestModle resultEntryRequestModle);
        Task<ServiceStatusResponseModel> SaveTestObservations(List<ResultSaveRequestModle> resultSaveRequestModle);

        Task<ActionResult<ServiceStatusResponseModel>> SaveHistoResult(HistoResultSaveRequestModle histoResultSaveRequestModle);
    }
}
