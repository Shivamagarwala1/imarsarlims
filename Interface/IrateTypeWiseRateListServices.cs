using iMARSARLIMS.Controllers;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;

namespace iMARSARLIMS.Interface
{
    public interface IrateTypeWiseRateListServices
    {
        Task<ServiceStatusResponseModel> SaveRateList(List<rateTypeWiseRateList> RateTypeWiseRateList);
        Task<ServiceStatusResponseModel> SaveRateListFromExcel(IFormFile ratelistexcel);
    }
}
