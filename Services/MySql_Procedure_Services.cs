using iMARSARLIMS.Controllers;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Response_Model;
using iText.Layout.Element;
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
