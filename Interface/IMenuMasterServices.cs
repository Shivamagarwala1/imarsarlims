using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.OData.Query;

namespace iMARSARLIMS.Interface
{
    public interface IMenuMasterServices
    {
        Task<ServiceStatusResponseModel> SaveMenu(menuMaster MenuMaster);
        Task<ServiceStatusResponseModel> UpdateMenuStatus(int menuId, byte Status);
        Task<ServiceStatusResponseModel> GetAllMenu(ODataQueryOptions<menuMaster> queryOptions);
    }
}
