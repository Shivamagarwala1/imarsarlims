﻿using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Account;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using iMARSARLIMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Bcpg;

namespace iMARSARLIMS.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class CentrePaymentController : BaseController<CentrePayment>
    {
        private readonly ContextClass db;
        private readonly ICentrePaymentServices _CentrePaymentServices;


        public CentrePaymentController(ContextClass context, ILogger<BaseController<CentrePayment>> logger, ICentrePaymentServices CentrePaymentServices) : base(context, logger)
        {
            db = context;
            this._CentrePaymentServices = CentrePaymentServices;
        }
        protected override IQueryable<CentrePayment> DbSet => db.CentrePayment.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("SubmitPayment")]
        public async Task<ServiceStatusResponseModel> SubmitPayment(CentrePaymentRequestModel centrePayments)
        {
            try
            {
                var result = await _CentrePaymentServices.SubmitPayment(centrePayments);
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

        [HttpGet("LedgerStatus")]
        public async Task<ServiceStatusResponseModel> LedgerStatus(List<int> CentreId)
        {
            try
            {
                var result = await _CentrePaymentServices.LedgerStatus(CentreId);
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

        [HttpGet("ClientLedgerStatus")]
        public async Task<ServiceStatusResponseModel> ClientLedgerStatus(List<int> CentreId, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                var result = await _CentrePaymentServices.ClientLedgerStatus(CentreId, FromDate, ToDate);
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

        [HttpGet("GetPatientBillDetail")]
        public async Task<ServiceStatusResponseModel> GetPatientBillDetail(string Workorderid)
        {
            try
            {
                var result = await _CentrePaymentServices.GetPatientBillDetail(Workorderid);
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
        [HttpPost("CancelPatientReciept")]
        public async Task<ServiceStatusResponseModel> CancelPatientReciept(string Workorderid, int Userid)
        {
            try
            {
                var result = await _CentrePaymentServices.CancelPatientReciept(Workorderid, Userid);
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
        [HttpGet("GetPatientpaymentDetail")]
        public async Task<ServiceStatusResponseModel> GetPatientpaymentDetail(string Workorderid)
        {
            try
            {
                var result = await _CentrePaymentServices.GetPatientpaymentDetail(Workorderid);
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
        [HttpPost("ChangePatientpaymentDetail")]
        public async Task<ServiceStatusResponseModel> ChangePatientpaymentDetail(List<ChangePaymentMode> PaymentMode)
        {
            try
            {
                var result = await _CentrePaymentServices.ChangePatientpaymentDetail(PaymentMode);
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


        [HttpPost("PaymentApproveReject")]
        public async Task<ServiceStatusResponseModel> PaymentApproveReject(CentrePaymetVerificationRequestModel CentrePaymetVerificationRequest)
        {
            try
            {
                var result = await _CentrePaymentServices.PaymentApproveReject(CentrePaymetVerificationRequest);
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

        [HttpGet("GetRateList")]
        public async Task<ServiceStatusResponseModel> GetRateList(int CentreId)
        {
            try
            {
                var result = await _CentrePaymentServices.GetRateList(CentreId);
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
        [HttpGet("GetRateListPdf")]
        public IActionResult GetRateListPdf(int centreid)
        {
            try
            {
                var result =  _CentrePaymentServices.GetRateListPdf(centreid);
                MemoryStream ms = new MemoryStream(result);
                return new FileStreamResult(ms, "application/pdf")
                {
                    FileDownloadName = $"RateList.pdf"
                };
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"An error occurred while processing the request: {ex.Message}");
            }
        }


        [HttpPost("TransferRateToRate")]
        public async Task<ServiceStatusResponseModel> TransferRateToRate(int FromRatetypeid, int ToRatetypeid)
        {
            try
            {
                var result = await _CentrePaymentServices.TransferRateToRate(FromRatetypeid, ToRatetypeid);
                return result;
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                };
            }
        }


        [HttpGet("ClientDepositReport")]
        public async Task<ServiceStatusResponseModel> ClientDepositReport(List<int> centreid, DateTime FromDate, DateTime ToDate, string Paymenttype)
        {
            try
            {
                var result = await _CentrePaymentServices.ClientDepositReport(centreid, FromDate, ToDate, Paymenttype);
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



        [HttpGet("GetWorkOrderdetailCentreChange")]
        public async Task<ServiceStatusResponseModel> GetWorkOrderdetailCentreChange(string WorkOrderid)
        {
            try
            {
                var result = await _CentrePaymentServices.GetWorkOrderdetailCentreChange(WorkOrderid);
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
        [HttpGet("GetWorkOrderNewRate")]
        public async Task<ServiceStatusResponseModel> GetWorkOrderNewRate(string WorkOrderid,int RatetypeId)
        {
            try
            {
                var result = await _CentrePaymentServices.GetWorkOrderNewRate(WorkOrderid, RatetypeId);
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

        [HttpPost("ChangeBillingCentre")]
        public async Task<ServiceStatusResponseModel> ChangeBillingCentre(string WorkOrderId, int Centre, int RateType)
        {
            try
            {
                var result = await _CentrePaymentServices.ChangeBillingCentre(WorkOrderId,Centre, RateType);
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

        [HttpGet("GetPatientForSettelmet")]
        public async Task<ServiceStatusResponseModel> GetPatientForSettelmet(int CentreId, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                var result = await _CentrePaymentServices.GetPatientForSettelmet(CentreId,FromDate,ToDate);
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

        [HttpPost("UpdatePatientSettelment")]
        public async Task<ServiceStatusResponseModel> UpdatePatientSettelment(List<BulkSettelmentRequest> SettelmentData)
        {
            try
            {
                var result = await _CentrePaymentServices.UpdatePatientSettelment(SettelmentData);
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

        [HttpPost("CentreRateChange")]
        public async Task<ServiceStatusResponseModel> CentreRateChange(int Centre, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                var result = await _CentrePaymentServices.CentreRateChange(Centre,FromDate,ToDate);
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

        [HttpGet("LedgerStatement")]
        public async Task<ServiceStatusResponseModel> LedgerStatement(int CentreId, DateTime FromDate, DateTime ToDate,string type)
        {
            try
            {
                var result = await _CentrePaymentServices.LedgerStatement(CentreId, FromDate, ToDate,type);
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

    }
}
