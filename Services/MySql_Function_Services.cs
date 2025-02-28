using iMARSARLIMS.Controllers;
using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Response_Model;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Services
{
    public class MySql_Function_Services
    {
        private readonly ContextClass db;
        public MySql_Function_Services(ContextClass context, ILogger<BaseController<tnx_BookingPatient>> logger)
        {
            db = context;
        }
        public string Getworkorderid(int centreId, string type)
        {
            string sql = "SELECT get_workorderid({0}, {1});";
            int param1 = centreId;
            string param2 = type;

            var result = db.Set<SingleStringResponseModel>()
           .FromSqlRaw(sql, param1, param2)
           .AsEnumerable()
           .FirstOrDefault()?.Value;
            return result;
        }
        public string GetBarcodeno(int centreId)
        {
            string sql = "Get_barcodeSeries();";
            

            var result = db.Set<SingleStringResponseModel>()
           .FromSqlRaw(sql)
           .AsEnumerable()
           .FirstOrDefault()?.Value;
            return result;
        }

    }
}
