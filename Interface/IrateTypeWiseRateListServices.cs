using iMARSARLIMS.Controllers;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;

namespace iMARSARLIMS.Interface
{
    public interface IrateTypeWiseRateListServices
    {
        Task<ServiceStatusResponseModel> SaveRateList(List<rateTypeWiseRateList> RateTypeWiseRateList);
        Task<ServiceStatusResponseModel> SaveRateListitemWise(List<rateTypeWiseRateList> RateTypeWiseRateList);
        byte[] GetRateListExcel(int RatetypeId);
        Task<ServiceStatusResponseModel> GetRateTypeRateListData(int ratetypeid, int deptId);
        Task<ServiceStatusResponseModel> GetItemrateListData(int itemid);
        Task<ServiceStatusResponseModel> SaveRateListFromExcel(IFormFile ratelistexcel);
    }
}
