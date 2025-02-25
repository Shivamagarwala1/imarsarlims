﻿using iMARSARLIMS.Controllers;
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
        public async Task<List<ResultEntryResponseModle>> GetTestObservation(int testId, string Gender, int fromAge, int toAge, int centreId)
        {

            string sql = "call GetTestObservationTestIdWise({0},{1},{2},{3},{4});";
            int param1 = testId;
            string param2 = Gender;
            int param3 = fromAge;
            int param4 = toAge;
            int param5 = centreId;

            var result = db.Set<ResultEntryResponseModle>()
           .FromSqlRaw(sql, param1, param2, param3, param4, param5)
           .AsEnumerable().ToList();
            return result;
        }

       
    }
}
