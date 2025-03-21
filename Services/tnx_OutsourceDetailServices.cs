using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace iMARSARLIMS.Services
{
    public class tnx_OutsourceDetailServices : Itnx_OutsourceDetailServices
    {
        private readonly ContextClass db;
        public tnx_OutsourceDetailServices(ContextClass context, ILogger<BaseController<tnx_OutsourceDetail>> logger)
        {
            db = context;
        }

        async Task<ServiceStatusResponseModel> Itnx_OutsourceDetailServices.SaveUpdateOutsourceData(List<tnx_OutsourceDetail> outSourcedata)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var outSourcedataSave = outSourcedata.Where(o => o.id == 0).ToList();
                    db.tnx_OutsourceDetail.AddRange(outSourcedataSave);
                    await db.SaveChangesAsync();
                    var outSourcedataUpdate = outSourcedata.Where(o => o.id != 0).ToList();
                    db.tnx_OutsourceDetail.UpdateRange(outSourcedataUpdate);
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Saved Successful"
                    };
                }
                catch(Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = ex.Message
                    };
                }
            }
        }

        async Task<ServiceStatusResponseModel> Itnx_OutsourceDetailServices.GetOutsourceData(DateTime FromDate, DateTime Todate, string SearchValue)
        {
            try
            {
                var Outsourcedata = await (from tb in db.tnx_Booking
                                           join tbi in db.tnx_BookingItem on tb.workOrderId equals tbi.workOrderId
                                           join iom in db.item_outsourcemaster on tbi.itemId equals iom.itemId
                                           join tosGroup in db.tnx_OutsourceDetail on tbi.id equals tosGroup.testId into tosLeftJoin
                                           from tos in tosLeftJoin.DefaultIfEmpty() // Left Join
                                           where tb.bookingDate >= FromDate && tb.bookingDate <= Todate
                                           //  && (SearchValue != "" &&(tb.workOrderId == SearchValue || tb.name == SearchValue)) 
                                           select new
                                           {
                                               testid = tbi.id,
                                               tbi.isApproved,
                                               tb.centreId,
                                               tb.transactionId,
                                               tb.workOrderId,
                                               PatientName = tb.name,
                                               tbi.itemId,
                                               Age = string.Concat(tb.ageYear, " Y ", tb.ageMonth, " M ", tb.ageDay, "D/" + tb.gender),
                                               tbi.barcodeNo,
                                               tbi.rate,
                                               outsourcerate=0,
                                               tbi.investigationName,
                                               outSourcelabs = (from om in db.item_outsourcemaster
                                                                join ol in db.outSourcelabmaster on om.LabId equals ol.id
                                                                where om.itemId == tbi.itemId
                                                                select new { ol.id, ol.labName,om.rate,outsourceRate= om.rate }).ToList(),
                                               OutsourceLabId = tos == null ? 0 : tos.outSourceLabID,
                                               OutsourceLabName = tos == null ? "" : tos.outSourceLabName,
                                               outsourceDetailId= tos == null ? 0 : tos.id
                                           }).ToListAsync();

                var groupedData = Outsourcedata
                    .GroupBy(m => m.testid) // Group by testid
                    .Select(group => new
                    {
                        TestId = group.Key, // testid is now the key
                        WorkOrderId = group.FirstOrDefault()?.workOrderId, // You can use any representative value if needed
                        PatientName = group.FirstOrDefault()?.PatientName,
                        ItemId = group.FirstOrDefault()?.itemId,
                        Age = group.FirstOrDefault()?.Age,
                        BarcodeNo = group.FirstOrDefault()?.barcodeNo,
                        InvestigationName = group.FirstOrDefault()?.investigationName,
                        OutsourceLabName = group.FirstOrDefault()?.OutsourceLabName,
                        OutsourceLabId = group.FirstOrDefault()?.OutsourceLabId,
                        OutSourcelabs = group.SelectMany(x => x.outSourcelabs).ToList(),
                        OutSourceDetailId= group.FirstOrDefault()?.outsourceDetailId,
                        isApprove = group.FirstOrDefault()?.isApproved,
                        Rate=group.FirstOrDefault()?.rate,
                        centreId = group.FirstOrDefault()?.centreId,
                        TransactionId = group.FirstOrDefault()?.transactionId,
                        OutSourceRate = group.FirstOrDefault()?.outsourcerate
                    })
                    .OrderBy(parent => parent.TestId)
                    .ToList();

                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = groupedData
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

        public byte[] GetOutsourceReportExcel(DateTime FromDate, DateTime Todate)
        {
            var Outsourcedata = (from tb in db.tnx_Booking
                                          join tbi in db.tnx_BookingItem on tb.workOrderId equals tbi.workOrderId
                                          join tod in db.tnx_OutsourceDetail on tbi.id equals tod.testId 
                                          where tb.bookingDate >= FromDate && tb.bookingDate <= Todate
                                          select new
                                          {
                                              testid = tbi.id,
                                              tbi.isApproved,
                                              tb.workOrderId,
                                              PatientName = tb.name,
                                              Age = string.Concat(tb.ageYear, " Y ", tb.ageMonth, " M ", tb.ageDay, "D/" + tb.gender),
                                              tbi.barcodeNo,
                                              tbi.investigationName,
                                              OutsourceLabName =  tod.outSourceLabName
                                          }).ToList();
                var ExcelByte = MyFunction.ExportToExcel(Outsourcedata, "OutSourceReport");
                return ExcelByte;
            
        }
    }
}