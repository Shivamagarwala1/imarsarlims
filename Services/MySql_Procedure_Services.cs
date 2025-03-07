using iMARSARLIMS.Controllers;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Services
{
    public class MySql_Procedure_Services
    {
        private readonly ContextClass db;
        public MySql_Procedure_Services(ContextClass context, ILogger<BaseController<tnx_BookingItem>> logger)
        {
            db = context;
        }
        public async Task<ServiceStatusResponseModel> ratelistDataItemWise(int itemId)
        {

            string sql = "call ratelistDataItemWise({0});";
            int param1 = itemId;
            

            var result = db.Set<rateListData>()
           .FromSqlRaw(sql, param1)
           .AsEnumerable().ToList();
            if (result.Count > 0)
            {
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = result
                };
            }
            else
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = "No observation mapped"
                };
            }
        }
        
        public List<TatReportData> TatReportexcel(DateTime FromDate, DateTime ToDate, int centreId, int departmentId, int itemid, string TatType)
        {
            string sql = "call TatReport({0},{1},{2},{3},{4},{5});";
            DateTime param1 = FromDate;
            DateTime param2 = ToDate;
            int param3 = centreId;
            int param4 = departmentId;
            int param5 = itemid;
            string param6 = TatType;

            var result = db.Set<TatReportData>()
           .FromSqlRaw(sql, param1, param2, param3, param4, param5, param5)
           .AsEnumerable().ToList();
            if (result.Count > 0)
            {
                return  result;
            }
            else
            {
                return new List<TatReportData> { };
            }
        }
        public async Task<ServiceStatusResponseModel> GetTestObservation(string testId, string Gender, int fromAge, int toAge, int centreId)
        {

            string sql = "call GetTestObservationTestIdWise({0},{1},{2},{3},{4});";
            string param1 = testId;
            string param2 = Gender;
            int param3 = fromAge;
            int param4 = toAge;
            int param5 = centreId;

            var result = db.Set<ResultEntryResponseModle>()
           .FromSqlRaw(sql, param1, param2, param3, param4, param5)
           .AsEnumerable().ToList();
            if (result.Count > 0)
            {
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = result
                };
            }
            else
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = "No observation mapped"
                };
            }
        }
        

    
    }
}
