using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.TransactionController
{
    [Route("api/[controller]")]
    [ApiController]
    public class tnx_BookingItemController : BaseController<tnx_BookingItem>
    {
        private readonly ContextClass db;
        private readonly Itnx_BookingItemServices _tnx_BookingItemServices;

        public tnx_BookingItemController(ContextClass context, ILogger<BaseController<tnx_BookingItem>> logger, Itnx_BookingItemServices tnx_BookingItemServices) : base(context, logger)
        {
            db = context;
            this._tnx_BookingItemServices = tnx_BookingItemServices;
        }
        protected override IQueryable<tnx_BookingItem> DbSet => db.tnx_BookingItem.AsNoTracking().OrderBy(o => o.id);


        [HttpGet("GetitemDetailRate")]
        public async Task<ServiceStatusResponseModel> GetitemDetailRate(int ratetype)
        {
            try
            {
                var result = await _tnx_BookingItemServices.GetitemDetailRate(ratetype);
                return result;
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        [HttpPost("GetSampleProcessingData")]
        public async Task<ServiceStatusResponseModel> GetSampleProcessingData(SampleProcessingRequestModel sampleProcessingRequestModel)
        {
            try
            {
                var result = await _tnx_BookingItemServices.GetSampleProcessingData(sampleProcessingRequestModel);
                return result;
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
        [HttpPost("UpdateSampleStatus")]
        public async Task<ActionResult<ServiceStatusResponseModel>> UpdateSampleStatus(List<SampleProcessingResponseModel> sampleProcessingResponseModel)
        {
            try
            {
                var result = await _tnx_BookingItemServices.UpdateSampleStatus(sampleProcessingResponseModel);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("UpdateSampletransfer")]
        public async Task<ActionResult<ServiceStatusResponseModel>> UpdateSampletransfer(List<tnx_Sra> SRA)
        {
            try
            {
                var result = await _tnx_BookingItemServices.UpdateSampletransfer(SRA);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("UpdateBatchReceive")]
        public async Task<ActionResult<ServiceStatusResponseModel>> UpdateBatchReceive(List<BatchStatusRecieveRequestModel> batchStatusRecieveRequestModel)
        {
            try
            {
                var result = await _tnx_BookingItemServices.UpdateBatchReceive(batchStatusRecieveRequestModel);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost("GetTestObservations")]
        public async Task<List<ResultEntryResponseModle>> GetTestObservations(ResultEntryRequestModle resultEntryRequestModle)
        {
            try
            {
                var result = await _tnx_BookingItemServices.GetTestObservations(resultEntryRequestModle);
                return result;
            }
            catch (Exception ex)
            {
                return new List<ResultEntryResponseModle>();
                //return BadRequest(ex.Message);
            }
        }

        [HttpPost("SaveTestObservations")]
        public async Task<ActionResult<ServiceStatusResponseModel>> SaveTestObservations(List<ResultSaveRequestModle> resultSaveRequestModle)
        {
            try
            {
                var result = await _tnx_BookingItemServices.SaveTestObservations(resultSaveRequestModle);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //histo result entry
        [HttpPost("SaveHistoResult")]
        public async Task<ActionResult<ServiceStatusResponseModel>> SaveHistoResult(HistoResultSaveRequestModle histoResultSaveRequestModle)
        {
            try
            {
                var result = await _tnx_BookingItemServices.SaveHistoResult(histoResultSaveRequestModle);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
