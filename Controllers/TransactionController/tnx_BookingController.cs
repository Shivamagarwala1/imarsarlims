﻿using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.transactionController
{

    [Route("api/[controller]")]
    [ApiController]
    public class tnx_BookingController : BaseController<tnx_Booking>
    {
        private readonly ContextClass db;
        private readonly ItnxBookingServices _tnxBookingServices;

        public tnx_BookingController(ContextClass context, ILogger<BaseController<tnx_Booking>> logger, ItnxBookingServices tnxBookingServices) : base(context, logger)
        {
            db = context;
            this._tnxBookingServices = tnxBookingServices;
        }
        protected override IQueryable<tnx_Booking> DbSet => db.tnx_Booking.AsNoTracking().OrderBy(o => o.transactionId);

        [HttpPost("GetPatientData")]
        public async Task<ServiceStatusResponseModel> GetPatientData(patientDataRequestModel patientdata)
        {
            try
            {
                var result = await _tnxBookingServices.GetPatientData(patientdata);
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
        [HttpPost("GetDispatchData")]
        public async Task<ServiceStatusResponseModel> GetDispatchData(ODataQueryOptions<DispatchDataModel> queryOptions, DispatchDataRequestModel patientdata)
        {
            try
            {
                var result = await _tnxBookingServices.GetDispatchData(queryOptions, patientdata);
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
        [HttpGet("GetHistoresult")]
        public async Task<ServiceStatusResponseModel> GetHistoresult(int testid)
        {
            try
            {
                var result = await _tnxBookingServices.GetHistoresult(testid);
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

        [HttpGet("GetMicroresult")]
        public async Task<ServiceStatusResponseModel> GetMicroresult(int testid,int reportStatus)
        {
            try
            {
                var result = await _tnxBookingServices.GetMicroresult(testid, reportStatus);
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

        [HttpGet("GetPaymentDetails")]
        public async Task<ServiceStatusResponseModel> GetPaymentDetails(string workOrderId)
        {
            try
            {
                var result = await _tnxBookingServices.GetPaymentDetails(workOrderId);
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
        [HttpPost("SaveSettelmentDetail")]
        public async Task<ServiceStatusResponseModel> SaveSettelmentDetail(List<settelmentRequestModel> settelments)
        {
            try
            {
                var result = await _tnxBookingServices.SaveSettelmentDetail(settelments);
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

        [HttpPost("GetPatientDocumnet")]
        public IActionResult GetPatientDocumnet(string workOrderId)
        {
            try
            {
                var result = _tnxBookingServices.GetPatientDocumnet(workOrderId);
                byte[] fileBytes = Convert.FromBase64String(result);

                // Check the type of content based on the first few bytes (signature)
                string contentType = "application/pdf"; // Default assumption is PDF
                if (fileBytes.Length > 4 && fileBytes[0] == 0xFF && fileBytes[1] == 0xD8) // JPEG Check
                {
                    contentType = "image/jpeg";
                }
                else if (fileBytes.Length > 4 && fileBytes[0] == 0x89 && fileBytes[1] == 0x50) // PNG Check
                {
                    contentType = "image/png";
                }
                else if (fileBytes.Length > 4 && fileBytes[0] == 0x47 && fileBytes[1] == 0x49) // GIF Check
                {
                    contentType = "image/gif";
                }

                // Create a MemoryStream with the byte array
                MemoryStream ms = new MemoryStream(fileBytes);

                // Return the file with the appropriate content type
                return new FileStreamResult(ms, contentType)
                {
                    FileDownloadName = contentType.Contains("pdf") ? "ProfitLossReport.pdf" : "ImageFile" // Change the filename based on the content type
                };

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("PrintBarcode")]
        public async Task<ServiceStatusResponseModel> PrintBarcode(string BarcodeNO)
        {
            try
            {
                var barcodedata = await (from tbi in db.tnx_BookingItem
                                         join tb in db.tnx_Booking on tbi.workOrderId equals tb.workOrderId
                                         where tbi.barcodeNo == BarcodeNO
                                         select new
                                         {
                                             tb.name,
                                             age = string.Concat(tb.ageYear, " Y ", tb.gender),
                                             tbi.barcodeNo,
                                             tb.workOrderId,
                                             tbi.sampleTypeName,
                                             bookingDate = tb.bookingDate.ToString("yyyy-MMM-dd hh:mm tt")
                                         }).ToListAsync();

                string returnStr = "";
                foreach (var item in barcodedata)
                {
                    returnStr += (returnStr == "" ? "" : "^") + item.name + "," +
                                item.age + ",a," + item.barcodeNo +
                                 "" + "," +
                                 item.workOrderId + "," + item.sampleTypeName
                                  + "," + item.bookingDate;
                }

                //  Window.location = "barcode://?cmd=" + returnStr;
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = returnStr
                };

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

        [HttpGet("GetPatientDetail")]
        public async Task<ServiceStatusResponseModel> GetPatientDetail(string workorderId)
        {
            try
            {
                var result = await _tnxBookingServices.GetPatientDetail(workorderId);
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
        //for popup
        [HttpGet("GetTestInfo")]
        public async Task<ServiceStatusResponseModel> GetTestInfo(string TestId)
        {
            try
            {
                var result = await _tnxBookingServices.GetTestInfo(TestId);
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

        [HttpGet("GetbarcodeChangedetail")]
        public async Task<ServiceStatusResponseModel> GetbarcodeChangedetail(string WorkOrderId)
        {
            try
            {
                var result = await _tnxBookingServices.GetbarcodeChangedetail(WorkOrderId);
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

        [HttpPost("UpdateBarcode")]
        public async Task<ServiceStatusResponseModel> UpdateBarcode(List<barcodeChangeRequest> NewBarcodeData)
        {
            try
            {
                var result = await _tnxBookingServices.UpdateBarcode(NewBarcodeData);
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


        [HttpGet("TatReport")]
        public async Task<ServiceStatusResponseModel> TatReport(DateTime FromDate, DateTime ToDate, int centreId,int departmentId,int itemid,string TatType)
        {
            try
            {
                var result = await _tnxBookingServices.TatReport(FromDate, ToDate,centreId, departmentId,itemid,TatType);
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
        [HttpGet("TatReportExcel")]
        public  IActionResult TatReportExcel(DateTime FromDate, DateTime ToDate, int centreId, int departmentId, int itemid, string TatType)
        {
            try
            {
                var result =  _tnxBookingServices.TatReportExcel(FromDate, ToDate, centreId, departmentId, itemid, TatType);
                return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "TatReport.xlsx");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("TatReportpdf")]
        public IActionResult TatReportpdf(DateTime FromDate, DateTime ToDate, int centreId, int departmentId, int itemid, string TatType)
        {
            try
            {
                var result = _tnxBookingServices.TatReportpdf(FromDate, ToDate, centreId, departmentId, itemid, TatType);
                MemoryStream ms = new MemoryStream(result);

                return new FileStreamResult(ms, "application/pdf")
                {
                    FileDownloadName = "TATReport.pdf"
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetMethodChangedetail")]
        public async Task<ServiceStatusResponseModel> GetMethodChangedetail(string WorkOrderId)
        {
            try
            {
                var result = await _tnxBookingServices.GetMethodChangedetail(WorkOrderId);
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
        [HttpPost("UpdateMethod")]
        public async Task<ServiceStatusResponseModel> UpdateMethod(List<methodChangeRequestModel> methoddata)
        {
            try
            {
                var result = await _tnxBookingServices.UpdateMethod(methoddata);
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

        [HttpPost("GetWorkSheetData")]
        public async Task<ServiceStatusResponseModel> GetWorkSheetData(WorkSheetRequestModel worksheetdata)
        {
            try
            {
                var result = await _tnxBookingServices.GetWorkSheetData(worksheetdata);
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
        [HttpGet("PrintWorkSheet")]
        public IActionResult PrintWorkSheet(string TestIds)
        {
            try
            {
                var result =  _tnxBookingServices.PrintWorkSheet(TestIds);
                MemoryStream ms = new MemoryStream(result);

                return new FileStreamResult(ms, "application/pdf")
                {
                    FileDownloadName = "WorkSheet.pdf"
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetSampleTypedetail")]
        public async Task<ServiceStatusResponseModel> GetSampleTypedetail(string WorkOrderId)
        {
            try
            {
                var result = await _tnxBookingServices.GetSampleTypedetail(WorkOrderId);
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
        [HttpPost("UpdateSampleType")]
        public async Task<ServiceStatusResponseModel> UpdateSampleType(List<SampltypeChangeRequestModel> sampletypedata)
        {
            try
            {
                var result = await _tnxBookingServices.UpdateSampleType(sampletypedata);
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
        [HttpPost("MachineResult")]
        public async Task<ServiceStatusResponseModel> MachineResult(MachineResultRequestModel machineResult)
        {
            try
            {
                var result = await _tnxBookingServices.MachineResult(machineResult);
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

        [HttpGet("GetReportDateChangeData")]
        public async Task<ServiceStatusResponseModel> GetReportDateChangeData(string WorkOrderId)
        {
            try
            {
                var result = await _tnxBookingServices.GetReportDateChangeData(WorkOrderId);
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

        [HttpPost("ReportDateChange")]
        public async Task<ServiceStatusResponseModel> ReportDateChange(List<DateChangeRequestModel> DateData)
        {
            try
            {
                var result = await _tnxBookingServices.ReportDateChange(DateData);
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

        [HttpPost("SendWhatsapp")]
        public async Task<ServiceStatusResponseModel> SendWhatsapp(string workOrderId, int Userid,string Mobileno,int header)
        {
            try
            {
                var result = await _tnxBookingServices.SendWhatsapp(workOrderId, Userid,Mobileno, header);
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
        [HttpGet("WhatsappNo")]
        public async Task<ServiceStatusResponseModel> WhatsappNo(string workOrderId)
        {
            try
            {
                var result = await _tnxBookingServices.WhatsappNo(workOrderId);
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

        [HttpPost("SendEmail")]
        public async Task<ServiceStatusResponseModel> SendEmail(string workOrderId, int Userid,string EmailId,int header)
        {
            try
            {
                var result = await _tnxBookingServices.SendEmail(workOrderId, Userid , EmailId, header);
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
        [HttpGet("SendEmailId")]
        public async Task<ServiceStatusResponseModel> SendEmailId(string workOrderId)
        {
            try
            {
                var result = await _tnxBookingServices.SendEmailId(workOrderId);
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

        [HttpPost("CollectionReport")]
        public IActionResult CollectionReport(collectionReportRequestModel collectionData)
        {
            try
            {
                var result = _tnxBookingServices.CollectionReport(collectionData);
                MemoryStream ms = new MemoryStream(result);

                return new FileStreamResult(ms, "application/pdf")
                {
                    FileDownloadName = "CollectionReport.pdf"
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CollectionReportData")]
        public async Task<ServiceStatusResponseModel> CollectionReportData(collectionReportRequestModel collectionData)
        {
            try
            {
                var result = await _tnxBookingServices.CollectionReportData(collectionData);
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
        [HttpPost("CollectionReportExcel")]
        public IActionResult CollectionReportExcel(collectionReportRequestModel collectionData)
        {
            try
            {
                var result = _tnxBookingServices.CollectionReportExcel(collectionData);
                return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CollectionReport.xlsx");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CollectionReportSummury")]
        public IActionResult CollectionReportSummury(collectionReportRequestModel collectionData)
        {
            try
            {
                var result = _tnxBookingServices.CollectionReportSummury(collectionData);
                MemoryStream ms = new MemoryStream(result);

                return new FileStreamResult(ms, "application/pdf")
                {
                    FileDownloadName = "CollectionReportSummury.pdf"
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CollectionReportDataSummury")]
        public async Task<ServiceStatusResponseModel> CollectionReportDataSummury(collectionReportRequestModel collectionData)
        {
            try
            {
                var result = await _tnxBookingServices.CollectionReportDataSummury(collectionData);
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
        [HttpPost("CollectionReportExcelSummury")]
        public IActionResult CollectionReportExcelSummury(collectionReportRequestModel collectionData)
        {
            try
            {
                var result = _tnxBookingServices.CollectionReportExcelSummury(collectionData);
                return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CollectionReportSummury.xlsx");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("DiscountReport")]
        public IActionResult DiscountReport(collectionReportRequestModel collectionData)
        {
            try
            {
                var result = _tnxBookingServices.DiscountReport(collectionData);
                MemoryStream ms = new MemoryStream(result);

                return new FileStreamResult(ms, "application/pdf")
                {
                    FileDownloadName = "DiscountReport.pdf"
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

       

        [HttpPost("DiscountReportData")]
        public async Task<ServiceStatusResponseModel> DiscountReportData(collectionReportRequestModel collectionData)
        {
            try
            {
                var result = await _tnxBookingServices.DiscountReportData(collectionData);
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
      

        [HttpPost("DiscountReportExcel")]
        public IActionResult DiscountReportExcel(collectionReportRequestModel collectionData)
        {
            try
            {
                var result = _tnxBookingServices.DiscountReportExcel(collectionData);
                return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DiscountReport.xlsx");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost("DiscountReportSummury")]
        public IActionResult DiscountReportSummury(collectionReportRequestModel collectionData)
        {
            try
            {
                var result = _tnxBookingServices.DiscountReportSummury(collectionData);
                MemoryStream ms = new MemoryStream(result);

                return new FileStreamResult(ms, "application/pdf")
                {
                    FileDownloadName = "DiscountReportSummury.pdf"
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost("DiscountReportDataSummury")]
        public async Task<ServiceStatusResponseModel> DiscountReportDataSummury(collectionReportRequestModel collectionData)
        {
            try
            {
                var result = await _tnxBookingServices.DiscountReportDataSummury(collectionData);
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


        [HttpPost("DiscountReportExcelSummury")]
        public IActionResult DiscountReportExcelSummury(collectionReportRequestModel collectionData)
        {
            try
            {
                var result = _tnxBookingServices.DiscountReportExcelSummury(collectionData);
                return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DiscountReportSummury.xlsx");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("patientDataDiscount")]
        public async Task<ServiceStatusResponseModel> patientDataDiscount(string workorderId)
        {
            try
            {
                var result = await _tnxBookingServices.patientDataDiscount(workorderId);
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

        [HttpPost("DiscountAfterBill")]
        public async Task<ServiceStatusResponseModel> DiscountAfterBill(DicountAfterBillRequestModel DiscountData)
        {
            try
            {
                var result = await _tnxBookingServices.DiscountAfterBill(DiscountData);
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

        [HttpPost("TestRefund")]
        public async Task<ServiceStatusResponseModel> TestRefund(testRefundModel RefundData)
        {
            try
            {
                var result = await _tnxBookingServices.TestRefund(RefundData);
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



        [HttpPost("ClientRevenueReport")]
        public IActionResult ClientRevenueReport(collectionReportRequestModel collectionData)
        {
            try
            {
                var result = _tnxBookingServices.ClientRevenueReport(collectionData);
                MemoryStream ms = new MemoryStream(result);

                return new FileStreamResult(ms, "application/pdf")
                {
                    FileDownloadName = "ClientRevenueReport.pdf"
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost("ClientRevenueReportData")]
        public async Task<ServiceStatusResponseModel> ClientRevenueReportData(collectionReportRequestModel collectionData)
        {
            try
            {
                var result = await _tnxBookingServices.ClientRevenueReportData(collectionData);
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


        [HttpPost("ClientRevenueReportExcel")]
        public IActionResult ClientRevenueReportExcel(collectionReportRequestModel collectionData)
        {
            try
            {
                var result = _tnxBookingServices.ClientRevenueReportExcel(collectionData);
                return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DiscountReport.xlsx");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost("ClientRevenueReportSummury")]
        public IActionResult ClientRevenueReportSummury(collectionReportRequestModel collectionData)
        {
            try
            {
                var result = _tnxBookingServices.ClientRevenueReportSummury(collectionData);
                MemoryStream ms = new MemoryStream(result);

                return new FileStreamResult(ms, "application/pdf")
                {
                    FileDownloadName = "ClientRevenueReportSummury.pdf"
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost("ClientRevenueReportDataSummury")]
        public async Task<ServiceStatusResponseModel> ClientRevenueReportDataSummury(collectionReportRequestModel collectionData)
        {
            try
            {
                var result = await _tnxBookingServices.ClientRevenueReportDataSummury(collectionData);
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


        [HttpPost("ClientRevenueReportExcelSummury")]
        public IActionResult ClientRevenueReportExcelSummury(collectionReportRequestModel collectionData)
        {
            try
            {
                var result = _tnxBookingServices.ClientRevenueReportExcelSummury(collectionData);
                return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ClientRevenueReportSummury.xlsx");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetPendingPayment")]
        public async Task<ServiceStatusResponseModel> GetPendingPayment(string workOrderId)
        {
            try
            {
                var result = await _tnxBookingServices.GetPendingPayment(workOrderId);
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
