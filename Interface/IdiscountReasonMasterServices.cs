using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface IdiscountReasonMasterServices
    {
        Task<ServiceStatusResponseModel> SaveUpdateDiscountReason(discountReasonMaster DiscountReason);
        Task<ServiceStatusResponseModel> UpdateDiscountReasonStatus(int id, byte status, int userId);
        Task<ServiceStatusResponseModel> SaveUpdateDiscountType(discountTypeMaster Discount);
        Task<ServiceStatusResponseModel> UpdateDiscountTypeStatus(int id, byte status, int userId);
    }
}
